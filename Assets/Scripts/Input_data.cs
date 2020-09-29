using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Input_data : MonoBehaviour
{
    //region variables
    public static string location;
    public static string time;
    public static string userName;

    //reference variables
    public InputField locationText;
    public InputField timeText;
    public InputField userNameText;

    //User user = new User();
    //end region

    
    public void OnSubmit()
    {
        location = locationText.text;
        time = timeText.text;
        userName = userNameText.text;
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("username = " + userName);

        //testing
        //forTests();

        //posts the data onto the database
        PostToDatabase();
    }

    /*public void forTests()
    {
        location = "Hayden";
        time = "14:00";
        userName = "pzar97";
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("username = " + userName);
    }*/

    private void PostToDatabase()
    {
        //User user = new User();
        JObject time_jobj = new JObject();
        time_jobj["Time"] = time;
        JObject location_jobj = new JObject();
        location_jobj[location] = time_jobj;
        print(location_jobj.ToString());


        /*if(RestClient.Get("https://fir-f7893.firebaseio.com/" + userName  + ".json"))
        {

        }*/

        //connects to the firebase
        RestClient.Put("https://virtualasucampustour.firebaseio.com/" + userName + ".json", location_jobj.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
