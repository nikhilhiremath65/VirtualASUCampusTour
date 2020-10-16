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

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public InputField TourNameText;
    public InputField AddLocationText;
    public Dropdown TiMe_hours;
    public Dropdown TiMe_mins;
    DB_Details dbDetails;
    DatabaseReference reference;

    ArrayList tours;
    bool toursDisplayed;

    public static string time;
    public static string time_h;
    public static string time_m;

    List<string> time_hours = new List<string>() { "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
    List<string> time_mins = new List<string>() { "00", "15", "30", "45" };

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();
        PopulateList();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        
        toursDisplayed = false;

    }

    void PopulateList()
    {
        TiMe_hours.AddOptions(time_hours);
        TiMe_mins.AddOptions(time_mins);
    }

    public void Dropdown_IndexChanged_hours(int index)
    {
        time_h = time_hours[index];

    }

    //dropdown index change function for minutes
    public void Dropdown_IndexChanged_mins(int index)
    {
        time_m = time_mins[index];
        //concatenate both the results
        time = time_h + time_m;
        //time = Selectedname.text;
    }

   
    public void onAddLocation()
    {

        print("Location" + AddLocationText.text + "added.");
        this.tours.Add(AddLocationText.text);
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

        string dummyString = "dummy";

        //Creating JSON 


        JObject locations = new JObject();
        foreach (string s in tours)
        {
            locations[s] = dummyString;
        }

        string jsonData = locations.ToString();


        //Append Values to database
        print("Wrting to database values : " + jsonData);
             
        try { 
        reference.Child(dbDetails.getTourDBName()).Child(TourNameText.text).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
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


}
