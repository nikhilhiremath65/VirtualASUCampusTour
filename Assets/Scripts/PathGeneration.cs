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

	public class PathGeneration : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		MeshModifier[] MeshModifiers;
		[SerializeField]
		Material _material;

		public GameObject StartWayPoint;
		public GameObject EndWayPoint;

		private Directions _directions;
		private int _counter;

		GameObject _directionsGO;

        private bool path;
        private Vector2d start;
        private Vector2d end;

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
				dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());;
			}

			Debug.Log("Count : " + dat.Count);
			var feat = new VectorFeatureUnity();
			feat.Points.Add(dat);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				mod.Run(feat, meshData, _map.WorldRelativeScale);
				Debug.Log("mesh Count : " + meshData.Triangles.Count);
			}

			Debug.Log("mesh Count : " + meshData.Triangles.Count);
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
			return _directionsGO;
		}

        private void UpdatePath()
        {
            if (path & _map.updatePath)
            {
				generatePath(start, end);
			}
        }

		private void generatePath(Vector2d start, Vector2d end)
        {
			this.start = start;
			this.end = end;

			var count = 2;
			var wp = new Vector2d[count];

			wp[0] = start;
			wp[1] = end;

			StartWayPoint.SetActive(true);
			EndWayPoint.SetActive(true);


			var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);

			StartWayPoint.transform.position = Conversions.GeoToWorldPosition(start.x, start.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
			EndWayPoint.transform.position = Conversions.GeoToWorldPosition(end.x, end.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();

			path = true;
			_map.updatePath = false;

		}

        public void Search()
        {

			start = _map.startLocation;
			end = _map.EndLocation;

            if (_map.generatePath)
            {
				generatePath(start, end);
			}
        }

        //public void test()
        //{
        //    //Input.location.Start();
        //    //start = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //    //Input.location.Start();

        //    start = new Vector2d(33.409089, -111.924104);
        //    end = new Vector2d(33.4209125, -111.9331915);


        //    generatePath(start, end);
        //}
    }

}
