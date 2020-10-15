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

public class DisplayLocations : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public Text TourNameText;
    public string TourName;
    DB_Details dbDetails;
    DatabaseReference reference;

    ArrayList tours;
    bool toursDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();
        TourName = "PolyCampusTour";

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getTourData();
        toursDisplayed = false;
        Debug.Log("hello");
        TourNameText.text = TourName;

    }

    // Update is called once per frame
    void Update()
    {
        if (!toursDisplayed && tours.Count > 0)
        {
            createTourList();
        }
    }

    void getTourData()
    {

        reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error fetching data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result.Child(dbDetails.getTourDBName()).Child(TourName);
                
                Dictionary<string, object> tourData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot.GetRawJsonValue());

                foreach (string tour in tourData.Keys)
                {
                    this.tours.Add(new Tour(tour));
                }
                Debug.Log(tours.Count);
            }
        });
        Debug.Log(tours);
    }

    void createTourList()
    {
        // Debug.Log(tours.Count);
        foreach (Tour s in tours)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
            controller.Name.text = s.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;
        }
        toursDisplayed = true;
    }

}
