using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dept_tourlistcontroller : MonoBehaviour
{

    public Text Name;
    Singleton singleton;
    public GameObject deletePanel;
    public void ConfirmDelete()
    {
        deletePanel.SetActive(true);
    }

    public void OnCancel()
    {
        deletePanel.SetActive(false);
    }
    public void Delete()

    {
        Destroy(gameObject);
    }

    public void Edit(string scenename)
    {
        // write edit logic here
        singleton = Singleton.Instance();
        singleton.SetScheduleName(Name.text);
        SceneManager.LoadScene(scenename);
    }

    public void NextScene()
    {
        // write next scene logic here
    }
}
