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
