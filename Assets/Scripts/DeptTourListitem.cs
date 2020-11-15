using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;
using UnityEngine.SceneManagement;

public class DeptTourListitem : MonoBehaviour
{

    public Text Name;
    Singleton singleton;
    public GameObject item;
    public GameObject deletePanel;
    public void confirmDelete()
    {
        deletePanel.SetActive(true);

    }

    public void onCancel()
    {
        deletePanel.SetActive(false);
    }
    public void onDelete()

    {
        Destroy(gameObject);
        deletePanel.SetActive(false);

      
        deleteFromArrayList(Name.text);


    }

    public void deleteFromArrayList(string deleteLocation)
    {
        singleton = Singleton.Instance();
        string tourName = singleton.getTourName();

        PSLocationArraySingleton psObject = PSLocationArraySingleton.Instance();
        Dictionary<string, ArrayList> toursLocations = psObject.getToursLocationDictionary();

        print("Deleting location in tour: " + tourName);
        ArrayList locations = toursLocations[tourName];
        print("Deleting locations: " + deleteLocation);
        locations.RemoveAt(locations.IndexOf(deleteLocation));

        toursLocations[tourName] = locations;
        psObject.setUpdateDeleteStatus(1);
        Dictionary<string, int> toursLocationsStatusUpdate = psObject.getToursLocationsUpdateStatusDictionary();
        toursLocationsStatusUpdate[tourName] = 1;
    }


    public void Edit(string scenename)
    {
        // write edit logic here
        singleton = Singleton.Instance();
        singleton.setScheduleName(Name.text);
        SceneManager.LoadScene(scenename);
    }

    public void PSEdit(string scenename)
    {
        singleton = Singleton.Instance();
        singleton.setPSTourNameEdit(Name.text);
        SceneManager.LoadScene(scenename);

    }
    public void nextScene(string scenename)
    {
        // write next scene logic here
        singleton = Singleton.Instance();
        singleton.setScheduleName(Name.text);
        SceneManager.LoadScene(scenename);
    }
}
