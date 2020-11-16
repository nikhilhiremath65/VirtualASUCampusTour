using UnityEngine;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.MeshGeneration.Factories;

public class DragWayPoint : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public float timeDelayThreshold = 1.0f;

    public GameObject WayPoint;
    public TourLocation location;
    public GameObject Directions;
    public GameObject DragInfoPanel;

    private ScheduleDirections scheduleDirections;
    private int clicked;
    private Singleton singleton;


    void Start()
    {
        singleton = Singleton.Instance();
        scheduleDirections = Directions.GetComponentInChildren<ScheduleDirections>();
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
                        Vector2d latitudeLongitude = (WayPoint.transform.position - new Vector3(0, 5, 5)).GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                        _map.UpdateMap(latitudeLongitude, _map.Zoom);
                        singleton.setISDrag(!IsDrag);
                        WayPoint.transform.position = Conversions.GeoToWorldPosition(latitudeLongitude.x, latitudeLongitude.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz() + new Vector3(0, 5, 5);
                        location.Drag = !location.Drag;
                        DragInfoPanel.SetActive(true);
                    }
                    else
                    {
                        location.Drag = !location.Drag;
                        Vector2d latitudeLongitude = WayPoint.transform.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
                        scheduleDirections.setLocationCoOrdinates(latitudeLongitude, location.index);
                        singleton.setISDrag(!IsDrag);
                        DragInfoPanel.SetActive(false);
                        singleton.setUpdatePath(true);
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