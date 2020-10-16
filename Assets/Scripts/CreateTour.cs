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

public class CreateTour : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public InputField TourNameText;
    public InputField AddLocationText;
    DB_Details dbDetails;
    DatabaseReference reference;

    ArrayList tours;
    bool toursDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        
        toursDisplayed = false;

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
