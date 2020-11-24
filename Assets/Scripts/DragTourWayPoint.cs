using UnityEngine;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Examples;

public class DragTourWayPoint : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public float timeDelayThreshold = 0.5f;

    public GameObject WayPoint;
    public TourLocation location;
    public GameObject Canvas;
    public GameObject DragInfoPanel;
    public GameObject Drag;
    public GameObject Camera;

    private PathGeneration tourDirections;
    private int clicked;
    private Singleton singleton;


    void Start()
    {
        singleton = Singleton.Instance();
        tourDirections = Canvas.GetComponentInChildren<PathGeneration>();
    }

    void Update()
    {
        checkForLongPress(timeDelayThreshold);
    }

    void checkForLongPress(float tim)
    {
        bool IsDrag = singleton.getIsDrag();
        if ((!IsDrag || location.Drag) && clicked != 0)
        {
            if (clicked == 1)
            { // If the user puts her finger on screen...
                timeLastPress = Time.time - timeLastPress;
            }
            if (clicked == 2)
            { // If the user raises her finger from screen
                timePressed = Time.time - timeLastPress;

                // Is the time pressed greater than our time delay threshold?
                if (timePressed > tim ) {
                    if (!location.Drag)
                    {
                        Drag.GetComponent<touchDrag>().enabled = false;
                        //Camera.GetComponent<ManualTouchCamera>().enabled = false;
                        Drag.GetComponent<MapDrag>().enabled = true;
                        Camera.transform.position = new Vector3(WayPoint.transform.position.x, Camera.transform.position.y, WayPoint.transform.position.z);


                        Vector2d latitudeLongitude = (WayPoint.transform.position - new Vector3(0, 5, 5)).GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                        Debug.LogWarning(latitudeLongitude);
                        //_map.UpdateMap(latitudeLongitude, _map.Zoom);
                        singleton.setISDrag(!IsDrag);
                        WayPoint.transform.position = Conversions.GeoToWorldPosition(latitudeLongitude.x, latitudeLongitude.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz() + new Vector3(0, 5, 5);
                        location.Drag = !location.Drag;
                        DragInfoPanel.SetActive(true);
                     
                    }
                    else
                    {
                        Drag.GetComponent<MapDrag>().enabled = false;
                        Drag.GetComponent<touchDrag>().enabled = true;
                        //Camera.GetComponent<ManualTouchCamera>().enabled = true;

                        location.Drag = !location.Drag;
                        Vector2d latitudeLongitude = WayPoint.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                        tourDirections.setLocationCoOrdinates(latitudeLongitude, location.index);
                        singleton.setISDrag(!IsDrag);
                        DragInfoPanel.SetActive(false);
                        _map.updatePath = true;
                        
                    }
                }
            }
            clicked = 0;
        }
    }

    void OnMouseDown()
    {
        clicked = 1;
    }

    private void OnMouseUp()
    {
        clicked = 2;
    }
}