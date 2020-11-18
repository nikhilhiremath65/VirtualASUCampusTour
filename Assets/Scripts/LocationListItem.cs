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

    Singleton singleton;
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
        item.Destroy();
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
        Dictionary<string, int> toursLocationsStatusUpdate = psObject.getToursLocationsUpdateStatusDictionary();
        toursLocationsStatusUpdate[tourName] = 1;
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
