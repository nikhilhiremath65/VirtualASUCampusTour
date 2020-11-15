

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Firebase;
using Mapbox.Json.Bson;



public class Dynamic_navigation : MonoBehaviour
{
    public Text Role;
    private Singleton singleton;
    public GameObject nav_Manager_Panel;
    public GameObject nav_Student_Panel;
    public GameObject nav_Prospective_Panel;

    // Start is called before the first frame update
    void Start()
    {
        singleton = Singleton.Instance();
        Role.text = "Role: " + singleton.getUserRole();
        on_Navigation(Role.text);
    }

    public void onLogout()
    {
        // logout from firebase
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }

        //reset the singleton instances
        singleton.setUserRole(null);

        //redirect to login
        SceneManager.LoadScene("Login");

    }
    public void on_Navigation(string role)
    {
        if(role == "Manager")
        {
            nav_Manager_Panel.SetActive(true);
        }
        else if(role == "Student")
        {
            nav_Student_Panel.SetActive(true);
        }
        else if(role == "Guest")
        {
            nav_Prospective_Panel.SetActive(true);
        }
        else
        {
            Debug.Log("invalid role");
        }
    }

}
