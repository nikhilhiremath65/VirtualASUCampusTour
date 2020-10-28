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
        //CrudOperations crud = new CrudOperations();
        //crud.deleteSchedule("departmentTourDataBase", Name.text);
        //Destroy(gameObject);
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
