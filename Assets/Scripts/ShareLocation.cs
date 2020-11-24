using UnityEngine;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.UI;
using System;
using System.Collections;

public class ShareLocation : MonoBehaviour
{

    DB_Details dbDetails;
    DatabaseReference reference;
    public InputField location;
    string lat;
    string lon;
    public GameObject ErrorPanel;
    public InputField locationLink;
    public Text ErrorMessage;
    public Text Hours;
    public Text Minutes;

    public Toggle locationToggle;

    // Start is called before the first frame update
    void Start()
    {
        dbDetails = new DB_Details();

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getCoordinates()
    {
        try
        {
            if (Hours.text == "hour" || Minutes.text == "minute")
            {
                ErrorPanel.transform.SetAsLastSibling();
                throw new Exception("Please enter time");
            }

            if (locationToggle.isOn)
            {
                GetCurrentLocation();
            }
            else
            {
                if (location.text == "")
                {
                    ErrorPanel.transform.SetAsLastSibling();
                    throw new Exception("Please enter location!");
                }

                reference.GetValueAsync().ContinueWith(task =>
                {
                    // if (task.IsFaulted)
                    // {
                    //     ErrorPanel.transform.SetAsLastSibling();
                    //     throw new Exception("ERROR while fetching data from database!!! Please refresh!");
                    // }
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result.Child(dbDetails.getBuildingDBname());

                        string str = snapshot.GetRawJsonValue();
                        JObject jsonLocation = JObject.Parse(str);

                        var locationText = location.text;

                        lat = (string)jsonLocation[locationText]["Coordinates"]["Latitude"];
                        lon = (string)jsonLocation[locationText]["Coordinates"]["Longitude"];
                    }
                });
            }

            if (lat != null && lon != null)
            {
            locationLink.text = lat + ":" + lon + ":" + Hours.text +":"+ Minutes.text;
        
            }


        }

        catch (InvalidCastException e)
        {
            Debug.Log(e.Message);
            // Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            // Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
    }

    private IEnumerator GetCurrentLocation()
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

            lat = latitude.ToString();
            lon = longitude.ToString();
        }
    }
}
