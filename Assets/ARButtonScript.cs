using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARButtonScript : MonoBehaviour
{

    Singleton singleton;
    private string role;
    public void onARClick()
    {
        singleton = Singleton.Instance();
        singleton.setARType(null);
        SceneManager.LoadScene("AR");
    }

    public void onstartClick()
    {
        singleton = Singleton.Instance();
        role = singleton.getUserRole();
        Debug.Log("Role : " + role);
        if (role == "Student")
        {
            singleton.setARType("schedule");
        }

        else if (role == "Guest")
        {
            singleton.setARType("tour");
        }
        SceneManager.LoadScene("AR");
    }

}
