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
using System;

public class Switch_button : MonoBehaviour
{
    public Singleton q;
    public GameObject MapCamera;
    public GameObject MainCamera;
    public Text ViewText;
    private string view;
    

    // Start is called before the first frame update
    void Start()
    {

        q = Singleton.Instance();

        view = q.getScheduleName().ToString();

        print(view);
        print(ViewText.text);

        //onButtonClick(view);
        //onButtonClick(ViewText.text);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClick(string view1)
    {
        
        if (view1 == "Map")
        {
            MapCamera.SetActive(true);
            MainCamera.SetActive(false);
            ViewText.text = "AR";
        }
        else if (view1 == "AR")
        {
            MainCamera.SetActive(true);
            MapCamera.SetActive(false);
            ViewText.text = "Map";
        }
        else
        {
            Debug.Log("no view selected");
        }

    }
}
