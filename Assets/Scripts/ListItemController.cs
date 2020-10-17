using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;
using UnityEngine.SceneManagement;

public class ListItemController : MonoBehaviour
{

    public Text Name;
    Singleton singleton;
    public void delete()
    {
        CrudOperations crud = new CrudOperations();
        crud.deleteSchedule("scheduleDataBase", "nhiremat", Name.text);
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
