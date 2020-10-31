using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DisplayLocationsSchedule : MonoBehaviour
{
    bool locationsDisplayed;


    DB_Details dbDetails;
    DatabaseReference reference;

    private string ScheduleName;
    private string UserName;
    private Singleton singleton;

    private Dictionary<string, string> locationsData;
    public GameObject ContentPanel;
    public GameObject ErrorPanel;
    public GameObject ListItemPrefab;

    public Text ErrorMessage;

    public InputField ScheduleNameText;
    public InputField AddLocationText;
    public Text Hours;
    public Text Minutes;
    void Start()
    {
        locationsData = new Dictionary<string, string>();
        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        singleton = Singleton.Instance();
        ScheduleName = singleton.getScheduleName();
        UserName = singleton.getUserName();
        ScheduleNameText.text = ScheduleName;

        getScheduleData();

        locationsDisplayed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!locationsDisplayed && locationsData.Count > 0)
        {
            createScheduleList();
        }
    }

    void getScheduleData()
    {
        try
        {
            reference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click locations)");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result.Child(dbDetails.getScheduleDBName()).Child(UserName).Child(ScheduleName);

                    locationsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot.GetRawJsonValue());

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

    void createScheduleList()
    {
        foreach (string s in locationsData.Keys)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            LocationWithTimeItem controller = newSchedule.GetComponent<LocationWithTimeItem>();
            controller.Name.text = s;
            controller.Time.text = "Time : " + locationsData[s];
            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        locationsDisplayed = true;
    }

    void updateTourListOnAdd(string name, string time)
    {
        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationWithTimeItem controller = newSchedule.GetComponent<LocationWithTimeItem>();
        controller.Name.text = name;
        controller.Time.text = time;
        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;

    }


    public void onAddLocation()
    {
        try
        {
            if (AddLocationText.text == "")
            {
                throw new Exception("Please enter location!");
            }
            if (Hours.text == "hour" || Minutes.text == "minute")
            {
                throw new Exception("Please enter correct time!");
            }

            String time = Hours.text + ":" + Minutes.text;
            this.locationsData[AddLocationText.text] = time;
            updateTourListOnAdd(AddLocationText.text, time);
            AddLocationText.text = null;
        }
        catch (Exception e)
        {
            //Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
    }
    public void onDelete(Text locationName)
    {
        locationsData.Remove(locationName.text);
    }


    public void onSave()
    {
        ;

        //Creating JSON 
        JObject locationsObj = new JObject();

        foreach (string s in locationsData.Keys)
        {
            locationsObj[s] = locationsData[s];
        }
        string jsonData = locationsObj.ToString();

        try
        {
            if (ScheduleNameText.text == "")
            {
                throw new Exception("Please enter Schedule Name!");
            }

            reference.Child(dbDetails.getScheduleDBName()).Child(UserName).Child(ScheduleNameText.text).RemoveValueAsync();

            reference.Child(dbDetails.getScheduleDBName()).Child(UserName).Child(ScheduleNameText.text).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw new Exception("ERROR while appending values to database.");

                }
                else if (task.IsCompleted)
                {
                    Debug.Log("SUCCESS: DATA ADDED TO DATABASE");
                }
            });
            SceneManager.LoadScene("SchedulesScene");
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
