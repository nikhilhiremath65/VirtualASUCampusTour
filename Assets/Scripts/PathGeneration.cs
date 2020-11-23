namespace Mapbox.Unity.MeshGeneration.Factories
{
    using UnityEngine;
    using Mapbox.Directions;
    using System.Collections.Generic;
    using System.Linq;
    using Mapbox.Unity.Map;
    using Data;
    using Modifiers;
    using Mapbox.Utils;
    using Mapbox.Unity.Utilities;
    using System.Collections;
    using UnityEngine.UI;
    using Firebase.Database;
    using Firebase;
    using Firebase.Unity.Editor;
    using System;
    using Newtonsoft.Json.Linq;
    using Models;

    public class PathGeneration : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        MeshModifier[] MeshModifiers;
        [SerializeField]
        Material _material;


        public Text startLocation;
        public Text destLocation;
        public GameObject WayPoint;
        public GameObject locationIndicator;
        public GameObject ErrorPanel;
        public Text ErrorMessage;

        private Directions _directions;
        private int _counter;
        private bool path;
        private ArrayList coordinates;
        private ArrayList locations;
        private List<GameObject> _instances;
        private bool pathGenerated;
        private int completedOffSet;

        DB_Details dbDetails;
        DatabaseReference reference;
        GameObject _directionsGO;

        public Transform Mapholder;
        public Text startButtonText;

        public GameObject startButton;
        public GameObject startBox;
        public GameObject destBox;

        private Singleton singleton;
        private string TourName;
        private Vector2d CurrentPosition;
        private Dictionary<string, Coordinates> sharedLocations;


        public void Awake()
        {
            if (_map == null)
            {
                _map = FindObjectOfType<AbstractMap>();
            }
            _directions = MapboxAccess.Instance.Directions;
        }

        public void Start()
        {
            foreach (var modifier in MeshModifiers)
            {
                modifier.Initialize();
            }

            dbDetails = new DB_Details();
            locations = new ArrayList();
            coordinates = new ArrayList();
            _instances = new List<GameObject>();
            pathGenerated = false;
            path = false;
            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            singleton = Singleton.Instance();
            handleScene();

            InvokeRepeating("UpdatePath", 2.0f, 0.3f);
        }

        public void OnClickStart()
        {
            foreach (GameObject point in _instances)
            {
                point.Destroy();
            }

            _instances.Clear();
            locations.Clear();
            coordinates.Clear();

            if (pathGenerated)
            {
                pathGenerated = false;
                startButtonText.text = "start";
                _directionsGO.Destroy();
                path = true;
            }
            else
            {
                path = false;

                CurrentPosition = locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                coordinates.Add(CurrentPosition);
                locations.Add(new TourLocation("CurrentLocation", 0));

                int c = 1;
                if (startLocation.text != "")
                {
                    locations.Add(new TourLocation(startLocation.text, c));
                    c++;
                }

                if (destLocation.text != "")
                {
                    locations.Add(new TourLocation(destLocation.text, c));
                    getCoordinates();
                    startButtonText.text = "stop";
                    pathGenerated = true;
                }
                else
                {
                    ErrorMessage.text = "Please enter destination location!!!";
                    ErrorPanel.SetActive(true);
                }
            }
        }

        void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (response == null || null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }

            var meshData = new MeshData();
            var dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()); ;
            }

            var feat = new VectorFeatureUnity();
            feat.Points.Add(dat);

            foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
            {
                mod.Run(feat, meshData, _map.WorldRelativeScale);

            }

            CreateGameObject(meshData);
        }

        GameObject CreateGameObject(MeshData data)
        {
            if (_directionsGO != null)
            {
                _directionsGO.Destroy();
            }
            _directionsGO = new GameObject("direction waypoint " + " entity");
            var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
            mesh.subMeshCount = data.Triangles.Count;

            mesh.SetVertices(data.Vertices);
            _counter = data.Triangles.Count;
            for (int i = 0; i < _counter; i++)
            {
                var triangle = data.Triangles[i];
                mesh.SetTriangles(triangle, i);
            }

            _counter = data.UV.Count;
            for (int i = 0; i < _counter; i++)
            {
                var uv = data.UV[i];
                mesh.SetUVs(i, uv);
            }

            mesh.RecalculateNormals();

            _directionsGO.AddComponent<MeshRenderer>().material = _material;
            _directionsGO.transform.SetAsFirstSibling();
            _directionsGO.transform.rotation = Mapholder.rotation;
            _directionsGO.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            _directionsGO.transform.position = new Vector3(_directionsGO.transform.position.x, 2.0f, _directionsGO.transform.position.y);
            _directionsGO.layer = 9;
            return _directionsGO;
        }

        private void UpdatePath()
        {
            if (coordinates.Count > 1 && (!path || _map.updatePath))
            {
                CurrentPosition = locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                coordinates[0] = CurrentPosition;
                CheckLocation();
                generatePath();
                path = true;
                _map.updatePath = false;
            }

            if (!path && _directionsGO != null)
                _directionsGO.Destroy();
        }

        private void generatePath()
        {
            var count = coordinates.Count;
            var wp = new Vector2d[count];

            for (int i = 0; i < count; i++)
            {
                wp[i] = (Vector2d)coordinates[i];
            }

            if (coordinates.Count > 1)
            {
                var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
                _directionResource.Steps = true;
                _directions.Query(_directionResource, HandleDirectionsResponse);
            }

            if (!path)
            {
                for (int i = 0; i < count; i++)
                {
                    var prefab = WayPoint;
                    var instance = Instantiate(WayPoint) as GameObject;
                    //Text wayPointNumber = instance.GetComponentInChildren<Text>();
                    //wayPointNumber.text = (i).ToString();
                    _instances.Add(instance);

                    DragTourWayPoint dragWayPoint = instance.GetComponentInChildren<DragTourWayPoint>();
                    dragWayPoint.location = (TourLocation)locations[i];
                }
            }

            for (int i = 1; i < count; i++)
            {
                var instance = _instances[i];

                DragTourWayPoint dragWayPoint = instance.GetComponentInChildren<DragTourWayPoint>();
                TourLocation location = dragWayPoint.location;

                if (!location.Drag)
                {
                    //instance.transform.rotation = Mapholder.rotation;
                    instance.transform.position = Conversions.GeoToWorldPosition(wp[i].x, wp[i].y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz() + new Vector3(0, 5, 5);
                    instance.SetActive(true);
                    instance.transform.SetAsLastSibling();
                }

            }

            path = true;

        }

        public void setLocationCoOrdinates(Vector2d points, int index)
        {
            coordinates[index - completedOffSet + 1] = points;
            for (int i = 0; i < coordinates.Count; i++)
            {
                Debug.LogWarning(coordinates[i]);
            }
        }

        private void CheckLocation()
        {
            Vector2d point1 = (Vector2d)coordinates[0];
            Vector2d point2 = (Vector2d)coordinates[1];
            double dist = getDistance(point1.x, point1.y, point2.x, point2.y);

            if (dist < 0.0001)
            {
                coordinates.RemoveAt(1);
                _instances[1].Destroy();
                _instances.RemoveAt(1);
                completedOffSet++;
                if (coordinates.Count < 2)
                {
                    _directionsGO.Destroy();
                    startButtonText.text = "Start";
                }
            }
        }

        private double getDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

        private void handleScene()
        {
            switch(singleton.getARType()){
                case "tour": loadTourScene();
                    singleton.setARType(null);
                    break;
                case "schedule": loadScheduleScene();
                    singleton.setARType(null);
                    break;
                case null:
                    break;
            }
        }

        private void loadTourScene()
        {
            startButton.SetActive(false);
            startBox.SetActive(false);
            destBox.SetActive(false);

            TourName = singleton.getTourName();

            CurrentPosition = locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
            locations.Add(new TourLocation("CurrentLocation", 0));
            coordinates.Add(CurrentPosition);

            PSLocationArraySingleton pSLocationArraySingleton = PSLocationArraySingleton.Instance();
            Dictionary<string, ArrayList> toursLocationsDictObject = pSLocationArraySingleton.getToursLocationDictionary();
            sharedLocations = singleton.getSharedTourLocations();

            int i = 0;
            foreach (string location in toursLocationsDictObject[TourName])
            {
                locations.Add(new TourLocation(location, i));
                i++;
            }

            getCoordinates();
        }

        private void loadScheduleScene()
        {
            startButton.SetActive(false);
            startBox.SetActive(false);
            destBox.SetActive(false);

            CurrentPosition = locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
            locations.Add(new TourLocation("CurrentLocation", 0));
            coordinates.Add(CurrentPosition);

            getScheduleData();
        }

        void getCoordinates()
        {
            try
            {
                reference.GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result.Child(dbDetails.getBuildingDBname());
                        
                        string str = snapshot.GetRawJsonValue();
                        JObject jsonLocation = JObject.Parse(str);
                        for (int i = 1; i < locations.Count; i++)
                        {
                            TourLocation location = (TourLocation)locations[i];

                            if (sharedLocations != null && sharedLocations.ContainsKey(location.Name))
                            {
                                location.Latitute = sharedLocations[location.Name].Latitude;
                                location.Longitude = sharedLocations[location.Name].Longitude;
                            }
                            else
                            {
                                location.Latitute = (string)jsonLocation[location.Name]["Coordinates"]["Latitude"];
                                location.Longitude = (string)jsonLocation[location.Name]["Coordinates"]["Longitude"];
                            }
                            double lat = double.Parse(location.Latitute);
                            double lon = double.Parse(location.Longitude);
                            coordinates.Add(new Vector2d(lat, lon));
                        }
                    }
                });
            }
            catch (InvalidCastException e)
            {
                // Perform some action here, and then throw a new exception.
                ErrorMessage.text = e.Message;
                ErrorPanel.SetActive(true);
            }
            catch (Exception e)
            {
                // Perform some action here, and then throw a new exception.
                ErrorMessage.text = e.Message;
                ErrorPanel.SetActive(true);
            }
        }

        void getScheduleData()
        {
            string UserName = singleton.getUserName();
            string ScheduleName = singleton.getScheduleName();

            try
            {
                reference.GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result.Child(dbDetails.getScheduleDBName()).Child(UserName).Child(ScheduleName);

                        string str = snapshot.GetRawJsonValue();
                        JObject jsonLocation = JObject.Parse(str);
                        IList<string> keys = jsonLocation.Properties().Select(p => p.Name).ToList();
                        var values = jsonLocation.ToObject<Dictionary<string, object>>();

                        int i = 0;
                        foreach (string location in keys)
                        {
                            locations.Add(new TourLocation(location, i + 1));
                            i++;
                        }

                        getCoordinates();
                    }
                });
            }
            catch (InvalidCastException e)
            {
                // Perform some action here, and then throw a new exception.
                ErrorMessage.text = e.Message;
                ErrorPanel.SetActive(true);
            }
            catch (Exception e)
            {
                // Perform some action here, and then throw a new exception.
                ErrorMessage.text = e.Message;
                ErrorPanel.SetActive(true);
            }
        }
    }
}
