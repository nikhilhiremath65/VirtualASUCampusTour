using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Dept_tourlistcontroller : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    DB_Details dbDetails;
    DatabaseReference reference;
    bool schedulesDisplayed;
    ArrayList toursNames = new ArrayList();

    PSLocationArraySingleton psObject = PSLocationArraySingleton.Instance();
    ArrayList schedules;
    Dictionary<string, ArrayList> toursLocationsDictObj;
    bool tourLocationsObjectStatus = false;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        schedulesDisplayed = false;
        schedules = new ArrayList();

        dbDetails = new DB_Details();

        toursLocationsDictObj = new Dictionary<string, ArrayList>();
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;



        tourLocationsObjectStatus = psObject.getToursLocationsObjectStatus();
        if (!tourLocationsObjectStatus)
        {
            getScheduleData();
            createScheduleList();
        }
        else
        {
            toursLocationsDictObj = psObject.getToursLocationDictionary();
            createDictScheduleList();
        }

        //getScheduleData();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!schedulesDisplayed)
        {
            createScheduleList();
        }

        else
        {
            //createDictScheduleList();
            print(schedulesDisplayed);
        }

        //Dictionary<string, ArrayList> toursLocationsTemp = psObject.getToursLocationDictionary();
        //foreach(string tour in toursLocationsTemp.Keys)
        //{
        //    print(tour);
        //    ArrayList locations = toursLocationsTemp[tour];
        //}
        

    }

    void getScheduleData()
    {
        // int totalTours = 0;
        
        

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
                DataSnapshot snapshot = task.Result.Child(dbDetails.getTourDBName());

                Dictionary<string, object> scheduleData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                foreach (string schedule in scheduleData.Keys)
                {
                    this.schedules.Add(new Department(schedule));
                    toursNames.Add(schedule);
                    print(schedule);
                    
                }

                foreach (string tour in toursNames)
                {
                    ArrayList tempLocationsArray = new ArrayList();
                    print(tour);
                    DataSnapshot snapshot1 = task.Result.Child(dbDetails.getTourDBName()).Child(tour);


                    Dictionary<string, string> locationsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot1.GetRawJsonValue());

                    foreach (KeyValuePair<string, string> location in locationsData)
                    {
                        tempLocationsArray.Add(location.Key);
                    }

                    toursLocationsDictObj.Add(tour, tempLocationsArray);
                }

                psObject.setToursLocationsDictionary(toursLocationsDictObj);
                psObject.setToursLocationsObjectStatus(true);
         
            }
        });
    }

   
 

    void createScheduleList()
    {
        foreach (Department s in schedules)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab);

            DeptTourListitem controller = newSchedule.GetComponent<DeptTourListitem>();
            string name1 = s.Name;
            controller.Name.text = name1;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        schedulesDisplayed = true;
    }

    void createDictScheduleList()
    {
        foreach (string s in toursNames)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab);

            DeptTourListitem controller = newSchedule.GetComponent<DeptTourListitem>();
            string name1 = s;
            controller.Name.text = name1;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        schedulesDisplayed = true;
    }
}
