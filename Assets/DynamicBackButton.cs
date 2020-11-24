using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicBackButton : MonoBehaviour
{

    Singleton singleton;
    string role;
    public void onBackClick()
    {
        singleton = Singleton.Instance();
        role = singleton.getUserRole();
        Debug.Log("RRRRRRRRRRRR : "+ role);
        if (role == "Student")
        {
            SceneManager.LoadScene("AR");

        }
        
        else if ( role == "Guest")
        {
            SceneManager.LoadScene("AR");

        }

       
    }
}
