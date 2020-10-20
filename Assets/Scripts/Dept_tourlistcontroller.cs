using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dept_tourlistcontroller : MonoBehaviour
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
