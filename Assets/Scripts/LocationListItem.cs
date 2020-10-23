using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LocationListItem : MonoBehaviour
{

    public Text Name;
    public GameObject item;
    public GameObject deletePanel;
    public string TourName = "PolyCampusTour";

    Singleton singleton;
    public void confirmDelete()
    {
        singleton = Singleton.Instance();
        TourName = singleton.GetTourName();
        //deletePanel.transform.SetAsLastSibling();
        deletePanel.SetActive(true);
        //Debug.Log(Name.text);
    }

    public void onCancel()
    {
        deletePanel.SetActive(false);
    }

    public void onDelete()
    {
        // write delete logic here
        DB_Details dbDetails = new DB_Details();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child(dbDetails.getTourDBName()).Child(TourName).Child(Name.text).RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("ERROR: when accessing Data from Database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("SUCCESS: DATA Deleted IN DATABASE");
            }
        });
        // Debug.Log("heloo");
        item.Destroy();
        deletePanel.SetActive(false);
    }


    public void onCreateTourLocationDelete()
    {
        item.Destroy();
        deletePanel.SetActive(false);
    }



    public void Edit()
    {
        // write edit logic here
    }

    public void nextScene()
    {
        // write next scene logic here
    }




}
