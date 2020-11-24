using UnityEngine;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;

public class LocationService : MonoBehaviour
{
    [SerializeField]
    AbstractMap _mapManager;

    public GameObject locationIndicator;
    public Transform camera;

    private bool _isInitialized = true;

    IEnumerator Start()
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

            if (_isInitialized)
            {
                // Make location indicator visible
                locationIndicator.SetActive(true);

                // Update camera view to focus on current location.
                Vector2d latitudeLongitude = new Vector2d(latitude, longitude);
                _mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);


                // Update position of location object wrt map.
                Vector3 position = Conversions.GeoToWorldPosition(latitude, longitude, _mapManager.CenterMercator, _mapManager.WorldRelativeScale).ToVector3xz();
            locationIndicator.transform.position =  new Vector3(position.x,locationIndicator.transform.position.y,position.z);

            }

        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    void Update()
    {
         float latitude;
        float longitude;

        if (!Input.location.isEnabledByUser)
        {
            latitude = 33.306964f;
            longitude = -111.677283f;
        }
        else
        {
            Input.location.Start();

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
        }

        if (_isInitialized)
        {
            // Update position of location object wrt map.
            Vector3 position = Conversions.GeoToWorldPosition(latitude, longitude, _mapManager.CenterMercator, _mapManager.WorldRelativeScale).ToVector3xz();
            locationIndicator.transform.position =  new Vector3(position.x,locationIndicator.transform.position.y,position.z);
        }
    }

    public void FocusLocationAndUpdatePointer()
    {

        float latitude;
        float longitude;

        if (!Input.location.isEnabledByUser)
        {
            latitude = 33.306964f;
            longitude = -111.677283f;
        }
        else
        {
            Input.location.Start();

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
        }

        if (_isInitialized)
        {
            // Update camera view to focus on current location.
            Vector2d latitudeLongitude = new Vector2d(latitude, longitude);
            _mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);

            // Update position of location object wrt map.
            Vector3 position = Conversions.GeoToWorldPosition(latitude, longitude, _mapManager.CenterMercator, _mapManager.WorldRelativeScale).ToVector3xz();
            //locationIndicator.transform.position =  new Vector3(position.x,locationIndicator.transform.position.y,position.z);
            camera.position = new Vector3(position.x,camera.position.y,position.z);

        }
    }
}