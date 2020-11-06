using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeptTourListitem : MonoBehaviour
{

    public Text Name;
    Singleton singleton;
    public GameObject item;
    public GameObject deletePanel;
    PSLocationArraySingleton singletonObject = PSLocationArraySingleton.Instance();
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
        GameObject ChildGameObject1 = item.transform.GetChild(0).gameObject;
        GameObject ChildGameObject2 = ChildGameObject1.transform.GetChild(0).gameObject;
        deleteFromArrayList(ChildGameObject2.GetComponent<UnityEngine.UI.Text>().text);

        //print(item.GetComponent<UnityEngine.UI.Text>().text);
        //deleteFromArrayList();
    }

    public void deleteFromArrayList(string deleteLocation)
    {
        print("deleting" + deleteLocation);
        ArrayList tourLocationsList = singletonObject.getLocations();
        if (tourLocationsList.Contains(deleteLocation))
        {
            print("found " + deleteLocation);
            tourLocationsList.RemoveAt(tourLocationsList.IndexOf(deleteLocation));
        }
        else
            print("Not found " + deleteLocation);
        singletonObject.setLocations(tourLocationsList);

        ArrayList allLocations = singletonObject.getLocations();
        print("No of elements remaining" + allLocations.Count);
        foreach (string location in allLocations)
        {
            print(location + "\n");
        }
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
