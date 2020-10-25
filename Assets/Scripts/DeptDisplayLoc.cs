using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;

public class DeptDisplayLoc : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;


    DB_Details dbDetails;
    DatabaseReference reference;
    bool locationsDisplayed;

    ArrayList locations;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        locations = new ArrayList();

        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        GetLocationData();
        locationsDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locationsDisplayed && locations.Count > 0)
        {
            CreateLocationsList();
        }
    }

    void GetLocationData()
    {
        DeptTournametransfer s = DeptTournametransfer.Instance;
        string tourName = s.getDeptTourName();

        reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error fetching data");
            }
            else if (task.IsCompleted)
            {
                // getting schedules for a particular user.
                DataSnapshot snapshot = task.Result.Child(dbDetails.getDeptTourDBName()).Child(tourName);

                Dictionary<string, string> scheduleData = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot.GetRawJsonValue());

                foreach (KeyValuePair<string, string> schedule in scheduleData)
                {
                    this.locations.Add(new ScheduleLocation(schedule.Key, schedule.Value));
                }
            }
        });
    }

    void CreateLocationsList()
    {
        foreach (ScheduleLocation s in locations)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        locationsDisplayed = true;
    }
}
