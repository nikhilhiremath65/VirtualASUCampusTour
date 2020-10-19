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
    bool toursDisplayed;

    ArrayList tours;

    DB_Details dbDetails;
    DatabaseReference reference;

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject ErrorPanel;

    public Text ErrorMessage;

    public InputField TourNameText;
    public InputField AddLocationText;

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
        try
        {
            if (AddLocationText.text == "")
            {
                throw new Exception("Please enter location!");
            }
            this.tours.Add(AddLocationText.text);
            updateTourListOnAdd(AddLocationText.text);
            AddLocationText.text = null;
        }
        catch (Exception e)
        {
            // Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
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


        try
        {
            if (TourNameText.text == "")
            {
                throw new Exception("Please enter Tour Name!");
            }

            reference.Child(dbDetails.getTourDBName()).Child(TourNameText.text).RemoveValueAsync();

            reference.Child(dbDetails.getTourDBName()).Child(TourNameText.text).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
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
            SceneManager.LoadScene("ManagerTourView");
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
        for (int i = 0; i < tours.Count; i++)
        {
            if (tours[i].Equals(name.text))
            {
                tours.RemoveAt(i);
                break;
            }
        }

    }


}
