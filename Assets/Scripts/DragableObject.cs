using UnityEngine;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;

public class DragableObject : MonoBehaviour
{
    //public GameObject map;

    //private Vector3 mOffset;

    //private float mZCoord;

    //private void Start()
    //{
    //    transform.position = GetMouseWorldPos() + mOffset;
    //}

    //void OnMouseDown()
    //{
    //    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //    mOffset = gameObject.transform.position - GetMouseWorldPos();
    //}

    //private Vector3 GetMouseWorldPos()
    //{
    //    Vector3 mousePoint = Input.mousePosition;

    //    mousePoint.z = mZCoord;

    //    return Camera.main.ScreenToWorldPoint(mousePoint);
    //}

    //void OnMouseDrag()
    //{
    //    transform.position = GetMouseWorldPos() + mOffset;  
    //}

}
