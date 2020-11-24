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
    ArrayList locations;
    DB_Details dbDetails;
    DatabaseReference reference;

    private string TourName;
    private Singleton singleton;
    private PSLocationArraySingleton locationArraySingleton;
    private string sharedLocationName;
    private Coordinates sharedLocationCoordinates;

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
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        singleton = Singleton.Instance();
        TourName = singleton.getTourName();

        TourNameText.text = TourName;
        locationsDisplayed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!locationsDisplayed)
        {
            createTourListFromDictionary();
            //createTourList();
        }
    }


    void createTourListFromDictionary()
    {
        singleton = Singleton.Instance();
        string currentTourName = singleton.getTourName();

        PSLocationArraySingleton s = PSLocationArraySingleton.Instance();
        Dictionary<string, ArrayList> toursLocations = s.getToursLocationDictionary();

        locations = toursLocations[currentTourName];

        foreach (string location in locations)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
            controller.Name.text = location;

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
            locations.Add(LinkLocationText.text);
            updateTourListOnAdd(LinkLocationText.text);
            sharedLocationName = LinkLocationText.text;
            sharedLocationCoordinates =  new Coordinates(data[0], data[1]);
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
                locations.Add(AddLocationText.text);
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
        Dictionary<string, int> toursLocationsStatusUpdate = s.getToursLocationsUpdateStatusDictionary();

        singleton = Singleton.Instance();
        string currentTourName = singleton.getTourName();

        // updating location of the tour in dictionary, and setting update status in dictionary

        Dictionary<string, ArrayList> toursLocations = s.getToursLocationDictionary();
        toursLocations[currentTourName] = locations;
        toursLocationsStatusUpdate[currentTourName] = 1;
        if (sharedLocationName != null)
        {
            singleton.addSharedLocation(sharedLocationName, sharedLocationCoordinates);
        }
 
        SceneManager.LoadScene("DeptTourLoc");

    }
}
