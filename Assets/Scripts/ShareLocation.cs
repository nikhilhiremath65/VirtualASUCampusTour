using UnityEngine;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.UI;
using System;

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

            if (location.text == "")
            {
                throw new Exception("Please enter location!");
            }

            if (Hours.text == "hour" || Minutes.text == "minute")
            {
                throw new Exception("Please enter time");
            }

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

                    var locationText = location.text.ToLower();
                    locationText = locationText.Replace(" ", "");

                    lat =(string)jsonLocation[locationText]["Coordinates"]["Latitude"];
                    lon =(string)jsonLocation[locationText]["Coordinates"]["Longitude"];

                }
            });

            if (lat == null || lon == null)
            {
                throw new Exception("Error fetching data, please try again!");
            }

            locationLink.text = lat + ":" + lon + ":" + Hours.text +":"+ Minutes.text;

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
}


