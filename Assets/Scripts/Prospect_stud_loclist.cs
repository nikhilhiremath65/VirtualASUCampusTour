using UnityEngine;
using UnityEngine.UI;

public class Prospect_stud_loclist : MonoBehaviour
{
    public Text Location;
    public GameObject item;
    public GameObject deletePanel;
    public string TourName = "Business";

    Singleton singleton;
    public void ConfirmDelete()
    {
        singleton = Singleton.Instance();
        TourName = singleton.GetTourName();
        //deletePanel.transform.SetAsLastSibling();
        deletePanel.SetActive(true);
        //Debug.Log(Name.text);
    }



    public void OnCancel()
    {
        deletePanel.SetActive(false);
    }

    public void OnDelete()
    {
        // write delete logic here
        item.Destroy();
        deletePanel.SetActive(false);
    }


    public void OnCreateTourLocationDelete()
    {
        item.Destroy();
        deletePanel.SetActive(false);
    }


    public void NextScene()
    {
        // write next scene logic here
    }
}
