using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Examples
{
	public class DragableDirectionWaypoint : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		public Transform MoveTarget;
		private Vector3 screenPoint;
		private Vector3 offset;
		private Plane _yPlane;

		private Vector2d latlon;
		private bool drag;

		public void Start()
		{
			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
			_yPlane = new Plane(Vector3.up, Vector3.zero);
			drag = false;
		}

        void OnMouseDown()
        {
			_map.mapDrag = true;
        }

        void OnMouseUp()
        {
			_map.mapDrag = false;
		}

        void OnMouseDrag()
		{ 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 0.0f;
			if (_yPlane.Raycast(ray, out enter))
			{
				MoveTarget.position = ray.GetPoint(enter);
			}
			latlon = MoveTarget.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			drag = true;
		}

        private void LateUpdate()
        {
			if (drag)
            {
				MoveTarget.transform.position = Conversions.GeoToWorldPosition(latlon.x, latlon.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
			}
        }
    }
}
