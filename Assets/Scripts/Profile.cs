

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



public class Profile : MonoBehaviour
{
    public Text UserName;
    public Text Email;
    public Text Role;
    private Singleton singleton;
   

    // Start is called before the first frame update
    void Start()
    {
        singleton = Singleton.Instance();
        UserName.text= "User Name: "+ singleton.getUserName();
        Email.text = "Email: "+singleton.getUserEmail();
        Role.text = "Role: "+singleton.getUserRole();
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

    public void onQRScan()
    { 
        //redirect to login
        SceneManager.LoadScene("QRCodeReader");

    }


}
