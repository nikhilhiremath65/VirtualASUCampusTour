using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;
using UnityEngine.SceneManagement;

public class DeptTourListitem : MonoBehaviour
{

    public Text Location;
    Singleton singleton;
    public GameObject deletePanel;
    public void confirmDelete()
    {
        deletePanel.SetActive(true);
    }

    public void onCancel()
    {
        deletePanel.SetActive(false);
    }
    public void delete()

    {
    
        Destroy(gameObject);
    }

    public void Edit(string scenename)
    {
        // write edit logic here
    }

    public void nextScene()
    {
        // write next scene logic here
    }
}
