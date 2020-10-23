using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeptTourListitem : MonoBehaviour
{

    public Text Name;
    public GameObject item;
    public GameObject deletePanel;

    Singleton singleton;

    private void Start()
    {
        singleton = Singleton.Instance();
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(true);
    }

    public void OnCancel()
    {
        deletePanel.SetActive(false);
    }

    public void OnDelete()
    {
        // write delete logic here
        //DB_Details dbDetails = new DB_Details();

        //// Set up the Editor before calling into the realtime database.
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbDetails.getDBUrl());

        //// Get the root reference location of the database.
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        //reference.Child(dbDetails.getDeptTourDBName()).Child(Name.text).RemoveValueAsync().ContinueWith(task =>
        //{
        //    if (task.IsFaulted)
        //    {
        //        Debug.Log("ERROR: when accessing Data from Database");
        //    }
        //    else if (task.IsCompleted)
        //    {

        //        Debug.Log("SUCCESS: DATA Deleted IN DATABASE");
        //    }
        //});

        item.Destroy();
        deletePanel.SetActive(false);
    }

    public void Edit(string scenename)
    {
        // write edit logic here
        //singleton.setTourName(Name.text);
        //SceneManager.LoadScene(scenename);
    }

    public void NextScene(string scenename)
    {
        // write next scene logic here
        singleton.SetTourName(Name.text);
        SceneManager.LoadScene(scenename);
    }
}
