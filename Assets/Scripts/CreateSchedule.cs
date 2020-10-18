using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CreateSchedule : MonoBehaviour
{
    bool schedulesDisplayed;

    ArrayList locations;

    DB_Details dbDetails;
    DatabaseReference reference;

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject ErrorPanel;

    public Text ErrorMessage;

    public InputField ScheduleNameText;
    public InputField AddLocationText;

    public Text Hours;
    public Text Minutes;

    // Start is called before the first frame update
    void Start()
    {
        locations = new ArrayList();
        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // To check if schedules are already displayed
        schedulesDisplayed = false;
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
            this.locations.Add(new LocationWithTime(AddLocationText.text, time));
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

    void updateTourListOnAdd(string name, string time)
    {
        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationWithTimeItem controller = newSchedule.GetComponent<LocationWithTimeItem>();
        controller.Name.text = name;
        controller.Time.text = "Time : " + time;

        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;
    }


    public void onSave()
    {

        //Creating JSON
        JObject locations = new JObject();
        JObject time = new JObject();

        foreach (LocationWithTime location in this.locations)
        {
            time["Time"] = location.Time;
            locations[location.Name] = time;
        }

        string jsonData = locations.ToString();

        try
        {
            if (ScheduleNameText.text == "")
            {
                throw new Exception("Please enter Tour Name!");
            }

            //Temp until authentication is completed
            String user = "nhiremat";

            reference.Child(dbDetails.getTourDBName()).Child(user).Child(ScheduleNameText.text).RemoveValueAsync();

            reference.Child(dbDetails.getScheduleDBName()).Child(user).Child(ScheduleNameText.text).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
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

    public void onDelete(Text name)
    {
        int i = 0;
        foreach(LocationWithTime l in locations)
        {
            if(l.Name == name.text)
            {
                locations.RemoveAt(i);
                break;
            }
            i++;
        }
    }
}
