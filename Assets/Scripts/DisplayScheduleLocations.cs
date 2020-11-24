using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;

public class DisplayScheduleLocations : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;


    DB_Details dbDetails;
    DatabaseReference reference;
    bool locationsDisplayed;

    ArrayList locations;
    // Start is called before the first frame update
    void Start()
    {
        locations = new ArrayList();

        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getLocationData();
        locationsDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locationsDisplayed && locations.Count > 0)
        {
            createLocationsList();
        }
    }

    void getLocationData()
    {
        ScheduleNameTransfer s = ScheduleNameTransfer.Instance;
        string scheduleName = s.getScheduleName();

        Singleton singleton = Singleton.Instance();
        String user = singleton.getUserName();

        reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error fetching data");
            }
            else if (task.IsCompleted)
            {
                // getting schedules for a particular user.
                DataSnapshot snapshot = task.Result.Child(dbDetails.getScheduleDBName()).Child(user).Child(scheduleName);

                string str = snapshot.GetRawJsonValue();
                JObject jsonLocation = JObject.Parse(str);
                IList<string> keys = jsonLocation.Properties().Select(p => p.Name).ToList();
                //var values = jsonLocation.ToObject<Dictionary<string, object>>();

                foreach (string schedule in keys)
                {
                    this.locations.Add(new ScheduleLocation(schedule, (string)jsonLocation[schedule]));
                }
            }
        });
    }

    void createLocationsList()
    {
        foreach (ScheduleLocation s in locations)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s.Name;
            controller.Time.text = s.Time;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        locationsDisplayed = true;
    }
}
