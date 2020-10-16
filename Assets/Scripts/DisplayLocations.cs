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

public class DisplayLocations : MonoBehaviour
{
    string TourName;
    ArrayList tours;
    bool toursDisplayed;
    DB_Details dbDetails;
    DatabaseReference reference;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public InputField TourNameText;
    public InputField AddLocationText;


    

    
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();
        TourName = "PolyCampusTour";
        TourNameText.text = TourName;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getTourData();
        toursDisplayed = false;
  
    }

    // Update is called once per frame
    void Update()
    {
        if (!toursDisplayed && tours.Count > 0)
        {
            createTourList();
        }
    }

    void getTourData()
    {

        reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error fetching data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result.Child(dbDetails.getTourDBName()).Child(TourName);
                
                Dictionary<string, object> tourData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                foreach (string tour in tourData.Keys)
                {
                    this.tours.Add(tour);
                }
                Debug.Log(tours.Count);
            }
        });
        Debug.Log("tours="+tours);
    }

    void createTourList()
    {

        
        foreach (string s in tours)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
            controller.Name.text = s;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        toursDisplayed = true;
    }

    void updateTourListOnAdd(string name)
    {

        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
        controller.Name.text = name;
        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;

    }


    public void onAddLocation()
    {
      
        print("Location" + AddLocationText.text + "added.");


        this.tours.Add(AddLocationText.text);
        updateTourListOnAdd(AddLocationText.text);
        AddLocationText.text = null;
        
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
        print("Writing to database values : " + jsonData);

        try
        {
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
