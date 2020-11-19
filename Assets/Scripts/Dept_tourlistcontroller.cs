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

    ArrayList schedules;
    Dictionary<string, ArrayList> toursLocationsDictObj;
    Dictionary<string, int> toursLocationsUpdateStatusDictObj;
    PSLocationArraySingleton psObject = PSLocationArraySingleton.Instance();

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        schedules = new ArrayList();

        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getScheduleData();
        schedulesDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!schedulesDisplayed && schedules.Count > 0)
        {
            createScheduleList();
        }
    }

    void getScheduleData()
    {

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
                }
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
        fillDictionaryWithTours();
    }

    void fillDictionaryWithTours()
    {
        // dictionary object is not filled with tours
        if (!psObject.getToursLocationsObjectStatus())
        {
            toursLocationsDictObj = new Dictionary<string, ArrayList>();
            toursLocationsUpdateStatusDictObj = new Dictionary<string, int>();
            foreach (Department s in schedules)
            {
                toursLocationsDictObj.Add(s.Name, null);
                toursLocationsUpdateStatusDictObj.Add(s.Name, 0);
            }
            psObject.setToursLocationsObjectStatus(true); // dictionary object is filled
            psObject.setToursLocationsDictionary(toursLocationsDictObj); // set Dictionary object
            psObject.setToursLocationsUpdateStatusDictionary(toursLocationsUpdateStatusDictObj);

        }
        // dictionary object is filled with tours
        else
        {
            toursLocationsDictObj = psObject.getToursLocationDictionary();
            // check of manager changed any tours (added or deleted tour)

            if (toursLocationsDictObj.Count != schedules.Count)
            {
                toursLocationsDictObj.Clear();
                toursLocationsUpdateStatusDictObj.Clear();
                foreach (Department s in schedules)
                {
                    toursLocationsDictObj.Add(s.Name, null);
                    toursLocationsUpdateStatusDictObj.Add(s.Name, 0);
                }
            }
        }
    }
}
