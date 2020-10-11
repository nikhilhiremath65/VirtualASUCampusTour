using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dynamic_Input : MonoBehaviour
{
    //region variables
    public static string location;
    public static string time;
    public static string time_h;
    public static string time_m;
    public static string scheduleName;

    //reference variables
    public InputField locationText;
    public Dropdown TiMe_hours;
    public Dropdown TiMe_mins;
    public InputField scheduleNameText;
    public Text Schedule_name;

    //dropdown index change function for hours
    public void Dropdown_IndexChanged_hours(int index)
    {
        time_h = time_hours[index];

    }

    //dropdown index change function for minutes
    public void Dropdown_IndexChanged_mins(int index)
    {
        time_m = time_mins[index];
        //concatenate both the results
        time = time_h + time_m;
        //time = Selectedname.text;
    }

    //User user = new User();
    //end region

    List<string> time_hours = new List<string>() { "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
    List<string> time_mins = new List<string>() { "00", "15", "30", "45" };

    // Start is called before the first frame update
    void Start()
    {
        PopulateList();

    }

    void PopulateList()
    {
        TiMe_hours.AddOptions(time_hours);
        TiMe_mins.AddOptions(time_mins);
    }

    public void OnSubmit()
    {
        location = locationText.text;
        scheduleName = scheduleNameText.text;
        Schedule_name.text = scheduleName;
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("schedule = " + scheduleName);

        //testing
        //forTests();

        //posts the data onto the database
        PostToDatabase();
    }

    /*public void forTests()
    {
        location = "Hayden";
        time = "14:00";
        scheduleName = "sch1";
        //prints the data onto console
        Debug.Log("location = " + location);
        Debug.Log("time = " + time);
        Debug.Log("schedule = " + scheduleName);
    }*/

    private void PostToDatabase()
    {
        //User user = new User();
        JObject time_jobj = new JObject();
        time_jobj["Time"] = time;
        JObject location_jobj = new JObject();
        location_jobj[location] = time_jobj;
        print(location_jobj.ToString());


        /*if(RestClient.Get("https://fir-f7893.firebaseio.com/" + scheduleName  + ".json"))
        {

        }*/

        //connects to the firebase
        RestClient.Put("https://virtualasucampustour.firebaseio.com/" + scheduleName + ".json", location_jobj.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
