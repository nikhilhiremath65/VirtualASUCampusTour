using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Models;

public class PSUpdateLocations : MonoBehaviour
{
    bool locationsDisplayed;

    ArrayList tours;

    DB_Details dbDetails;
    DatabaseReference reference;

    private string TourName;
    private Singleton singleton;
    private PSLocationArraySingleton locationArraySingleton;
    private Dictionary<string, Coordinates> sharedTourLocations;

    public GameObject ContentPanel;
    public GameObject NamePanel;
    public GameObject ErrorPanel;
    public GameObject ListItemPrefab;

    public Text ErrorMessage;

    public InputField TourNameText;
    public InputField AddLocationText;
    public InputField LinkLocationText;

    [Obsolete]
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();
        sharedTourLocations = new Dictionary<string, Coordinates>();
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        singleton = Singleton.Instance();
        TourName = singleton.getTourName();

        TourNameText.text = TourName;
        print(TourName);


        getTourData();

        locationsDisplayed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!locationsDisplayed && tours.Count > 0)
        {
            createTourList();
        }
    }

    void getTourData()
    {
        try
        {
            reference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result.Child(dbDetails.getTourDBName()).Child(TourName);

                    Dictionary<string, object> tourData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                    foreach (string tour in tourData.Keys)
                    {
                        this.tours.Add(tour);
                    }
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
        locationsDisplayed = true;
    }

    void updateTourListOnAdd(string name)
    {
        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
        controller.Name.text = name;
        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;

    }
    public void addLinkLocation()
    {
        try
        {
            if (LinkLocationText.text == "")
            {
                throw new Exception("Please enter location name!");

            }
            NamePanel.SetActive(false);
            String[] data = AddLocationText.text.Split(':');
            this.tours.Add(LinkLocationText.text);
            updateTourListOnAdd(LinkLocationText.text);
            sharedTourLocations.Add(LinkLocationText.text, new Coordinates(data[0], data[1]));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void onAddLocation()
    {
        try
        {
            if (AddLocationText.text.Contains(":"))
            {
                NamePanel.SetActive(true);
            }
            else
            {
                if (AddLocationText.text == "")
                {
                    throw new Exception("Please enter location!");
                }
                this.tours.Add(AddLocationText.text);
                updateTourListOnAdd(AddLocationText.text);
                AddLocationText.text = null;
            }
        }
        catch (Exception e)
        {
            // Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
    }


    public void onSave()
    {
        PSLocationArraySingleton s = PSLocationArraySingleton.Instance();
        s.setUpdateStatus(1);
        s.setLocations(tours);
        singleton.setSharedLocation(sharedTourLocations);
        SceneManager.LoadScene("DeptTourLoc");


        ArrayList allLocations = s.getLocations();
        foreach (string location in allLocations)
        {
            print(location + "\n");
        }

    }
}
