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
    using UnityEditor;
    using UnityEngine.UI;
    using Proyecto26;
    using Models;
    using Firebase.Database;
    using Firebase;
    using Firebase.Unity.Editor;
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Random = UnityEngine.Random;

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

        DB_Details dbDetails;
        DatabaseReference reference;
        GameObject _directionsGO;

        public Transform Mapholder;


        protected virtual void Awake()
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

            path = false;

            if (startLocation.text != "")
            {
                locations.Add(new TourLocation(startLocation.text, 0));
            }
            else
            {
                GetCurrentLocation();
            }

            if (destLocation.text != "")
            {
                // Testing current location:
                Debug.Log(coordinates.Count);
                if (locations.Count < 1 && coordinates.Count < 1)
                {
                    coordinates.Add(new Vector2d(33.4209125, -111.9331915));
                }

                locations.Add(new TourLocation(destLocation.text, 0));
                getCoordinates();
            }
            else
            {
                ErrorMessage.text = "Please enter destination location!!!";
                ErrorPanel.SetActive(true);
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

            // pick a random color
            //Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            //_material.SetColor("_Color",newColor);

            _directionsGO.AddComponent<MeshRenderer>().material = _material;
            _directionsGO.transform.SetAsFirstSibling();
            // _directionsGO.transform.position = new Vector3(_directionsGO.transform.position.x,-1.26f, _directionsGO.transform.position.z);
            _directionsGO.transform.rotation = Mapholder.rotation;
            _directionsGO.transform.localScale = new Vector3(1.0f,0.0f,1.0f);
            _directionsGO.transform.position = new Vector3(_directionsGO.transform.position.x, 2.0f, _directionsGO.transform.position.y);
            _directionsGO.layer = 9;
            return _directionsGO;
        }

        private void UpdatePath()
        {
            if (coordinates.Count > 0 && (!path || _map.updatePath))
            {
                generatePath();
                path = true;
                _map.updatePath = false;
            }
            // Testing
            // Vector3 position = Conversions.GeoToWorldPosition(33.4209125, -111.9331915, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
            // locationIndicator.transform.position = position;

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

                for (int i = 0; i < count; i++)
                {
                    var instance = _instances[i];
                    instance.transform.position = Conversions.GeoToWorldPosition(wp[i].x, wp[i].y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
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
            coordinates.Add(new Vector2d(33.409431, -111.924427));
            coordinates.Add(new Vector2d(33.410072, -111.924586));
            try
            {
                // reference.GetValueAsync().ContinueWith(task =>
                // {
                //     if (task.IsFaulted)
                //     {
                //         throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                //     }
                //     else if (task.IsCompleted)
                //     {
                //         DataSnapshot snapshot = task.Result.Child(dbDetails.getBuildingDBname());

                //         string str = snapshot.GetRawJsonValue();
                //         JObject jsonLocation = JObject.Parse(str);

                //         foreach (TourLocation location in this.locations)
                //         {
                //             location.Latitute = (string)jsonLocation[location.Name]["Coordinates"]["Latitude"];
                //             location.Longitude = (string)jsonLocation[location.Name]["Coordinates"]["Longitude"];
                //             double lat = double.Parse(location.Latitute);
                //             double lon = double.Parse(location.Longitude);
                //             coordinates.Add(new Vector2d(lat, lon));
                //         }
                //     }
                // });
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

        IEnumerator GetCurrentLocation()
        {
            if (!Input.location.isEnabledByUser)
                yield break;

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                float latitude = Input.location.lastData.latitude;
                float longitude = Input.location.lastData.longitude;

                locations.Add(new Vector2d(latitude, longitude));
            }

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
        }
    }

}
