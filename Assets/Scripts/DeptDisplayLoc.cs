using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine.UI;

public class DeptDisplayLoc : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    ArrayList gameObjectsList = new ArrayList();
    PSLocationArraySingleton psObject = PSLocationArraySingleton.Instance();
    ArrayList locationsTemp = new ArrayList();

    DB_Details dbDetails;
    DatabaseReference reference;
    bool locationsDisplayed;
    bool updateLocationsDisplayed;
    DataSnapshot snapshot;

    private Dictionary<string, string> scheduleData;


    ArrayList locations;

    public Text DepartmentTour;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        scheduleData = new Dictionary<string, string>();
        locations = new ArrayList();

        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getLocationData();
        locationsDisplayed = false;
        updateLocationsDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        PSLocationArraySingleton s = PSLocationArraySingleton.Instance();
        Singleton so = Singleton.Instance();
        string currentTourName = so.getTourName();

        Dictionary<string, int> toursLocationsStatusUpdate = s.getToursLocationsUpdateStatusDictionary();

        print("Current tour " + currentTourName + " Update status: " + toursLocationsStatusUpdate[currentTourName] +" Locations displayed: "+ locationsDisplayed + "Locations count: " + locations.Count);

        if (!locationsDisplayed && locations.Count > 0 && toursLocationsStatusUpdate[currentTourName] == 0)
        {
            createLocationsList();
        }

        else if(!updateLocationsDisplayed && toursLocationsStatusUpdate[currentTourName] ==1)
        {
            foreach (GameObject g in gameObjectsList)
            {

                g.Destroy();
            }
            gameObjectsList.Clear();
            Dictionary<string, ArrayList> toursLocations = s.getToursLocationDictionary();
            ArrayList updatedLocations = toursLocations[currentTourName];
            updateLocationsList(updatedLocations);
            s.setUpdateDeleteStatus(0);
        }
        
        //if (!updateLocationsDisplayed && s.getUpdateStatus() == 1)
        //{
        //    foreach (GameObject g in gameObjectsList)
        //    {
                
        //        g.Destroy();
        //    }
        //    gameObjectsList.Clear();

        //    // getting updated locations from dictionary

            

        //    Dictionary<string, ArrayList> toursLocations = s.getToursLocationDictionary();
        //    ArrayList updatedLocations = toursLocations[currentTourName];   
        //    updateLocationsList(updatedLocations);

        //}

        //if(gameObjectsList.Count >=1 && s.getUpdateStatus() == 1)
        //{
        //    foreach (GameObject g in gameObjectsList)
        //    {
             
        //        g.Destroy();
        //    }
        //    gameObjectsList.Clear();
        //}
    }

    void getLocationData()
    {
        PSLocationArraySingleton ps = PSLocationArraySingleton.Instance();
        Singleton s = Singleton.Instance();
        string scheduleName = s.getTourName();
        locationsTemp.Clear();

        DepartmentTour.text = scheduleName + " Locations";


        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error fetching data");
            }
            else if (task.IsCompleted)
            {
                // getting schedules for a particular user.


                snapshot = task.Result.Child(dbDetails.getTourDBName()).Child(scheduleName.ToString());


                scheduleData = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot.GetRawJsonValue());

                foreach (KeyValuePair<string, string> schedule in scheduleData)
                {
                    this.locations.Add(new DeptLocation(schedule.Key));
                    locationsTemp.Add(schedule.Key);
                    //print(schedule.Key);
                }
                Dictionary<string, ArrayList> toursLocations = ps.getToursLocationDictionary();
                ArrayList updatedLocations = toursLocations[scheduleName];

                if (updatedLocations == null)
                {
                    toursLocations[scheduleName] = locationsTemp;
                }


            }
        });
    }

    void fillLocationsInDictionary(string tourName, ArrayList locations)
    {
        Dictionary<string, ArrayList> toursLocations = psObject.getToursLocationDictionary();
        toursLocations[tourName] = locations; // set locations array for given tour


        // testing to see if locations are stored in dictionary or not

        //print("locations in tour:  " + tourName);
        //ArrayList locationsGet = toursLocations[tourName];
        //foreach (string location in locationsGet)
        //    print(location);

    }

    void createLocationsList()
    {
        Singleton so = Singleton.Instance();
        string scheduleName = so.getTourName();
        locationsTemp.Clear();
        foreach (DeptLocation s in locations)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab);
            gameObjectsList.Add(newSchedule);
            DeptTourListitem controller = newSchedule.GetComponent<DeptTourListitem>();
            string name1 = s.Name;
            controller.Name.text = name1;
            print("Adding location on screen" + name1);

            locationsTemp.Add(s.Name);

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        locationsDisplayed = true;
        print("Displaying locations for: " + scheduleName);
        print("LocationsTemm count" + locationsTemp.Count);
        fillLocationsInDictionary(scheduleName, locationsTemp);

    }

    void updateLocationsList(ArrayList updateLocations)
    {
        foreach (string s in updateLocations)
        {
            // Destroy(ListItemPrefab);
            // Destroy(ContentPanel);
            ListItemPrefab.SetActive(true);
            GameObject newToursLocation = Instantiate(ListItemPrefab);
            
            DeptTourListitem controller = newToursLocation.GetComponent<DeptTourListitem>();
           
            controller.Name.text = s;

            newToursLocation.transform.parent = ContentPanel.transform;
            newToursLocation.transform.localScale = Vector3.one;
        }
        updateLocationsDisplayed = true;


    }

  

    //public void onDelete(Text locationName)
    //{
    //    locations.Remove(locationName.text);
    //}
}
