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

            if (pathGenerated){
                pathGenerated = false;
                startButtonText.text = "start";
                _directionsGO.Destroy();
                path = true;
            }
            else{
            path = false;

                coordinates.Add(locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale));

                if (startLocation.text != "")
                {
                    locations.Add(new TourLocation(startLocation.text, 0));
                }

                if (destLocation.text != "")
                {
                    locations.Add(new TourLocation(destLocation.text, 0));
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
            _directionsGO.transform.localScale = new Vector3(1.0f,0.0f,1.0f);
            _directionsGO.transform.position = new Vector3(_directionsGO.transform.position.x, 2.0f, _directionsGO.transform.position.y);
            _directionsGO.layer = 9;
            return _directionsGO;
        }

        private void UpdatePath()
        {
            if (coordinates.Count > 1 && (!path || _map.updatePath))
            {
                coordinates[0] = locationIndicator.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                if (path)
                {
                    CheckLocation();
                }
                generatePath();
                path = true;
                _map.updatePath = false;

                locationIndicator.transform.rotation = Mapholder.transform.rotation;
            }
            if (!pathGenerated && _directionsGO!=null)
            _directionsGO.Destroy();
        }

        private void generatePath()
        {
            try
            {
                var count = coordinates.Count;
                var wp = new Vector2d[count];

                for (int i = 0; i < count; i++)
                {
                    wp[i] = (Vector2d)coordinates[i];
                }

                var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
                _directionResource.Steps = true;
                _directions.Query(_directionResource, HandleDirectionsResponse);

                if (!path)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var prefab = WayPoint;
                        var instance = Instantiate(WayPoint) as GameObject;
                        instance.layer = 9;
                        _instances.Add(instance);

                    }
                }

                for (int i = 1; i < count; i++)
                {
                    var instance = _instances[i];
                    instance.transform.position = Conversions.GeoToWorldPosition(wp[i].x, wp[i].y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
                    instance.transform.rotation = Mapholder.transform.rotation;
                    instance.SetActive(true);
                    instance.transform.SetAsLastSibling();
                }

                path = true;
            }
            catch (Exception e)
            {
                //Do nothing
            }

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

                        foreach (TourLocation location in this.locations)
                        {
                            location.Latitute = (string)jsonLocation[location.Name]["Coordinates"]["Latitude"];
                            location.Longitude = (string)jsonLocation[location.Name]["Coordinates"]["Longitude"];
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
    }

}
