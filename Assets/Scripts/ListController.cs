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

public class ListController : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    DB_Details dbDetails;
    DatabaseReference reference;
    bool schedulesDisplayed;

    ArrayList gameObjectsList = new ArrayList();

    ArrayList schedules;

    // Start is called before the first frame update
    [Obsolete]
    void Start()
    {
        schedules = new ArrayList();

        dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getScheduleData();
        createTempSchedules();
        schedulesDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!schedulesDisplayed && schedules.Count > 0)
        {
            foreach (GameObject g in gameObjectsList)
            {

                g.Destroy();
            }
            gameObjectsList.Clear();
            createScheduleList();
        }
    }

    public void createTempSchedules()
    {
        ArrayList schedules1 = new ArrayList();
        schedules1.Add("schedule 0");
        schedules1.Add("schedules 1");
        foreach (string s in schedules1)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
            gameObjectsList.Add(newSchedule);
            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s;

            newSchedule.transform.SetParent(ContentPanel.transform);
            //newSchedule.transform.localScale = Vector3.one;
        }
    }

    void getScheduleData()
    {
        Singleton singleton = Singleton.Instance();
        String user = singleton.getUserName();

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
                DataSnapshot snapshot = task.Result.Child(dbDetails.getScheduleDBName()).Child(user);

                string str = snapshot.GetRawJsonValue();
                JObject jsonLocation = JObject.Parse(str);
                IList<string> keys = jsonLocation.Properties().Select(p => p.Name).ToList();
                //var values = jsonLocation.ToObject<Dictionary<string, object>>();

                foreach (string schedule in keys)
                {
                    this.schedules.Add(new Schedule(schedule));
                }
            }
        });
    }

    void createScheduleList()
    {
        foreach (Schedule s in schedules)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s.Name;

            newSchedule.transform.SetParent(ContentPanel.transform);
            //newSchedule.transform.localScale = Vector3.one;
        }
        schedulesDisplayed = true;
    }
}
