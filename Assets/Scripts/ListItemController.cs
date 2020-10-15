using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;

public class ListItemController : MonoBehaviour
{

    public Text Name;

    public void delete()
    {
        CrudOperations crud = new CrudOperations();
        crud.deleteSchedule("scheduleDataBase", "nhiremat", "MondaySchedule");
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
