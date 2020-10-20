using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prospect_stud_loclist : MonoBehaviour
{
    public Text Location;
    public GameObject item;
    public GameObject deletePanel;
    public string TourName = "Business";

    public Dropdown search_loc_list;

    Singleton singleton;
    public void confirmDelete()
    {
        singleton = Singleton.Instance();
        TourName = singleton.getTourName();
        //deletePanel.transform.SetAsLastSibling();
        deletePanel.SetActive(true);
        //Debug.Log(Name.text);
    }



    public void onCancel()
    {
        deletePanel.SetActive(false);
    }

    public void onDelete()
    {
        // write delete logic here
        item.Destroy();
        deletePanel.SetActive(false);
    }


    public void onCreateTourLocationDelete()
    {
        item.Destroy();
        deletePanel.SetActive(false);
    }


    public void nextScene()
    {
        // write next scene logic here
    }
}
