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
    //region variables
    public static string location;
    public static string scheduleName;

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public InputField TourNameText;
    public InputField AddLocationText;
    public Dropdown TiMe_hours;
    public Dropdown TiMe_mins;
    DB_Details dbDetails;
    DatabaseReference reference;

    ArrayList tours;
    ArrayList Timearr;
    bool toursDisplayed;

    public static string time;
    public static string time_h;
    public static string time_m;
    public string username;


    //end region

    List<string> time_hours = new List<string>() { "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
    List<string> time_mins = new List<string>() { "00", "15", "30", "45" };

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        Timearr = new ArrayList();
        dbDetails = new DB_Details();
        PopulateList();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://asu-ar-app.firebaseio.com/scheduleDataBase");

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        toursDisplayed = false;

    }

    //dropdown index change function for hours
    public void Dropdown_IndexChanged_hours(int index)
    {
        time_h = time_hours[index];

    }

    //dropdown index change function for minutes
    public void Dropdown_IndexChanged_mins(int index)
    {
        time_m = time_mins[index];
        //concatenate both the results
        for (int i = 0; i < tours.Count; i++)
        {
            time = time_h + time_m;
            Timearr[i] = time;
        }
    }

    void PopulateList()
    {
        TiMe_hours.AddOptions(time_hours);
        TiMe_mins.AddOptions(time_mins);
    }

    public void onAddLocation()
    {

        print("Location" + AddLocationText.text + "added.");
        this.tours.Add(AddLocationText.text);
        this.Timearr.Add(time);
        updateTourListOnAdd(AddLocationText.text);
        //print("Array values" + tours.Count);
        AddLocationText.text = null;

    }

    void updateTourListOnAdd(string name)
    {

        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
        controller.Name.text = name;
        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;
    }


    public void onSave()
    {

        string userName = "nikhil";
        string jsonData;

        //Creating JSON 

        for (int i = 0; i < tours.Count; i++)
        {
            JObject Timeobj = new JObject();

            Timeobj["Time"] = Timearr[i];

            JObject Locobj = new JObject();


            Locobj[tours[i]] = Timeobj;

            JObject Scheduleobj = new JObject();


            Scheduleobj[TourNameText.text] = Locobj;


            

        }

        jsonData = Scheduleobj.ToString();


        //Append Values to database
        print("Wrting to database values : " + jsonData);

        try
        {
            reference.Child(dbDetails.getScheduleDBName()).Child(userName).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    print("ERROR: when accessing Data from Database");

                }
                else if (task.IsCompleted)
                {
                    print("SUCCESS: DATA ADDED TO DATABASE");
                }


            });
        }
        catch (InvalidCastException e)
        {
            // Perform some action here, and then throw a new exception.
            throw new Exception("EXCEPTION: ERROR while appending values to database  ", e);
        }
        catch (Exception e)
        {
            // Perform some action here, and then throw a new exception.
            throw new Exception("EXCEPTION: ERROR while appending values to database  ", e);
        }
        SceneManager.LoadScene("ManagerTourView");
    }

    /*public void OnSubmit()
    {
        location = locationText.text;
        scheduleName = scheduleNameText.text;
        Schedule_name.text = scheduleName;
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("schedule = " + scheduleName);

        //testing
        //forTests();

        //posts the data onto the database
        PostToDatabase();
    }*/

    /*public void forTests()
    {
        location = "Hayden";
        time = "14:00";
        scheduleName = "sch1";
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("schedule = " + scheduleName);
    }*/

    /*private void PostToDatabase()
    {
        //User user = new User();
        JObject time_jobj = new JObject();
        time_jobj["Time"] = time;
        JObject location_jobj = new JObject();
        location_jobj[location] = time_jobj;
        print(location_jobj.ToString());


        /*if(RestClient.Get("https://fir-f7893.firebaseio.com/" + scheduleName  + ".json"))
        {

        }*/

        //connects to the firebase
       // RestClient.Put("https://virtualasucampustour.firebaseio.com/" + scheduleName + ".json", location_jobj.ToString());
    //}

    // Update is called once per frame
    void Update()
    {

    }
}