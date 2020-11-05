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

    public class ScheduleDirections : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        MeshModifier[] MeshModifiers;
        [SerializeField]
        Material _material;

        public GameObject WayPoint;
        public GameObject ErrorPanel;
        public Text ErrorMessage;

        private Directions _directions;
        private int _counter;
        private bool path;
        private ArrayList coordinates;
        private ArrayList locations;
        private string ScheduleName;
        private string UserName;
        private List<GameObject> _instances;

        DB_Details dbDetails;
        DatabaseReference reference;
        GameObject _directionsGO;



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
            Singleton singleton = Singleton.Instance();

            ScheduleName = singleton.getScheduleName();
            UserName = singleton.getUserName();

            path = false;
            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            getScheduleData();

            InvokeRepeating("UpdatePath", 2.0f, 0.3f);
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
            return _directionsGO;
        }

        private void UpdatePath()
        {
            if (coordinates.Count > 0 && (!path || _map.updatePath))
            {
                generatePath();
                path = true;
            }

        }

        private void generatePath()
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

        void getScheduleData()
        {
            try
            {
                reference.GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted)
                    {
                        throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result.Child(dbDetails.getScheduleDBName()).Child(UserName).Child(ScheduleName);

                        Dictionary<string, object> locationData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());
                        foreach (string location in locationData.Keys)
                        {
                            locations.Add(new TourLocation(location));
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

        void getCoordinates()
        {
            try
            {
                reference.GetValueAsync().ContinueWith(task => {
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
    }

}
