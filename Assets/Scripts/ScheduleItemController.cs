using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crud;

public class ScheduleItemController : MonoBehaviour
{

    public Text Name;

    public void delete()
    {
        CrudOperations crud = new CrudOperations();
        crud.deleteSchedule("scheduleDataBase", "nhiremat", Name.text);
        Destroy(gameObject);
    }


}
