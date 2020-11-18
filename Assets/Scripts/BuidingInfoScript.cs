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


public class BuidingInfoScript : MonoBehaviour
{
    public Text BuildingInfoText;
    private Singleton singleton;
    
    // Start is called before the first frame update
    void Start()
    {
        singleton = Singleton.Instance();
        Debug.Log("Get values: " + singleton.getBuildingInfo());
        BuildingInfoText.text = "Building Info: " + singleton.getBuildingInfo();
    }

    public void onLogout()
    {
        // logout from firebase
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }

        //reset the singleton instances
        singleton.setUserName(null);
        singleton.setUserRole(null);
        singleton.setUserEmail(null);

        //redirect to login
        SceneManager.LoadScene("Login");

    }

}
