using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;
using UnityEngine.SceneManagement;
using System;

public class ListItemController : MonoBehaviour
{

    public Text Name;
    public Text Time;
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
        Singleton singleton = Singleton.Instance();
        String user = singleton.getUserName();

        CrudOperations crud = new CrudOperations();
        crud.deleteSchedule("scheduleDataBase", user, Name.text);
        Destroy(gameObject);
    }

    public void Edit(string scenename)
    {
        // write edit logic here
        singleton = Singleton.Instance();
        singleton.setScheduleName(Name.text);
        SceneManager.LoadScene(scenename);
    }

    public void nextScene()
    {
        // write next scene logic here
    }
}
