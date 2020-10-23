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

    public Text Name;
    Singleton singleton;
    public GameObject deletePanel;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;


    DB_Details dbDetails;
    DatabaseReference reference;
    bool schedulesDisplayed;

    ArrayList schedules;

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

    public void Edit(string scenename)
    {
        // write edit logic here
        //singleton = Singleton.Instance();
        //singleton.SetScheduleName(Name.text);
        SceneManager.LoadScene(scenename);
    }

    public void NextScene()
    {
        // write next scene logic here
    }
}
