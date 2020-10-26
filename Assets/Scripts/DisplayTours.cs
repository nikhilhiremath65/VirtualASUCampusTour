using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayTours : MonoBehaviour
{
    ArrayList tours;
    bool toursDisplayed;

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject ErrorPanel;

    public Text ErrorMessage;

    private DateTime startTime;

    DB_Details dbDetails;
    DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();
        Debug.Log(tours.Count);

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getTourData();

        toursDisplayed = false;

        startTime = DateTime.Now;
    }

    // Update is called once per frame
        void Update()
    {
        if(!toursDisplayed && tours.Count > 0)
        {
            createTourList();
        }

        if (!toursDisplayed)
        {
            if ((DateTime.Now - startTime).Seconds > 2)
            {
                SceneManager.LoadScene("ManagerTourView");
            }
        }
    }

    public void getTourData()
    {
        try
        {
            reference.GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    throw new Exception("ERROR while fetching data from database!!! Please refresh scene(Click Tours)");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result.Child(dbDetails.getTourDBName());

                    Dictionary<string, object> tourData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                    foreach (string tour in tourData.Keys)
                    {
                        this.tours.Add(new Tour(tour));
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

    public void createTourList()
    {
        foreach (Tour s in tours)
        {
            ListItemPrefab.SetActive(true);
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            TourListItem controller = newSchedule.GetComponent<TourListItem>();
            controller.Name.text = s.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        toursDisplayed = true;
    }

   
}
