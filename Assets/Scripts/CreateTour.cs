using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CreateTour : MonoBehaviour
{
    bool toursDisplayed;

    ArrayList tours;

    DB_Details dbDetails;
    DatabaseReference reference;

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject ErrorPanel;

    public Text ErrorMessage;

    public InputField TourNameText;
    public InputField AddLocationText;
    Singleton singleton;

    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        dbDetails = new DB_Details();



        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());
        

    
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        toursDisplayed = false;
    }
   
    public void onAddLocation()
    {
        try
        {
            if (AddLocationText.text == "")
            {
                throw new Exception("Please enter location!");
            }
            this.tours.Add(AddLocationText.text);
            updateTourListOnAdd(AddLocationText.text);
            AddLocationText.text = null;
        }
        catch (Exception e)
        {
            // Perform some action here, and then throw a new exception.
            ErrorMessage.text = e.Message;
            ErrorPanel.SetActive(true);
        }
    }

    void updateTourListOnAdd(string name)
    {
        GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;
        LocationListItem controller = newSchedule.GetComponent<LocationListItem>();
        controller.Name.text = name;
        newSchedule.transform.parent = ContentPanel.transform;
        newSchedule.transform.localScale = Vector3.one;
    }

    public void onSave()
    {
        StartCoroutine(save());
    }

    private IEnumerator save()
    {
        string dummyString = "dummy";

        //Creating JSON
        JObject locations = new JObject();

        foreach (string s in tours)
        {
            locations[s] = dummyString;
        }

        string jsonData = locations.ToString();


        
        if (TourNameText.text == "")
        {
             ErrorMessage.text = "Please enter Tour Name!";
            ErrorPanel.SetActive(true);
        }

           

        var deleteTask = reference.Child(dbDetails.getTourDBName()).Child(TourNameText.text).SetValueAsync(null);
        yield return new WaitUntil(predicate: () => deleteTask.IsCompleted);
        if (deleteTask.Exception != null)
        {
            throw new Exception("ERROR while deleting values from database.");
        }
        else
        {
            var appendTask = reference.Child(dbDetails.getTourDBName()).Child(TourNameText.text).SetRawJsonValueAsync(jsonData);
            yield return new WaitUntil(predicate: () => appendTask.IsCompleted);
            if (appendTask.Exception != null)
            {
                throw new Exception("ERROR while appending values from database.");
            }
            else
            {
                Debug.Log("SUCCESS: DATA ADDED TO DATABASE");
            }
        };

        SceneManager.LoadScene("ManagerTourView");
    }


    public void onDelete(Text name)
    {
        for (int i = 0; i < tours.Count; i++)
        {
            if (tours[i].Equals(name.text))
            {
                tours.RemoveAt(i);
                break;
            }
        }

    }


}
