using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Dept_tourlistcontroller : MonoBehaviour
{

    public Text Name;
    Singleton singleton;
    public GameObject deletePanel;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    DB_Details dbDetails;
    DatabaseReference reference;
    bool schedulesDisplayed;

    ArrayList tours;
    public bool locationsDisplayed;
    public string Tourname;
    

    void Start()
    {
        tours = new ArrayList();

        dbDetails = new DB_Details();

        //Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        singleton = Singleton.Instance();
        Tourname = singleton.GetDeptTourName();
        Name.text = Tourname;
        
        
        getScheduleData();
        schedulesDisplayed = false;
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(true);
    }

    public void OnCancel()
    {
        deletePanel.SetActive(false);
    }
    public void Delete()

    {
        Destroy(gameObject);
    }

    public void Edit()
    {
    
        
    }

    public void NextScene()
    {
        // write next scene logic here
    }
    void Update()
    {
        if (!schedulesDisplayed && tours.Count > 0)
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
                DataSnapshot snapshot = task.Result.Child(dbDetails.getDeptTourDBName()).Child(Tourname);

                Dictionary<string, object> scheduleData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                foreach (string schedule in scheduleData.Keys)
                {
                    this.tours.Add(new Schedule(schedule));
                }
            }
        });
    }
    void createScheduleList()
    {
        foreach (Schedule s in tours)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        schedulesDisplayed = true;
    }
}
