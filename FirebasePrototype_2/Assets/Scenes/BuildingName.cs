using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Newtonsoft.Json.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;



public class BuildingName : MonoBehaviour
{
    public Text nameText; //Display
    public InputField buildingNameText; //Input 


    public static string buildingName;
    DatabaseReference reference;


    private void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://asu-ar-app.firebaseio.com/");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //addData();
        //updateData();
        deleteData();
    }


    // Sample Data
    String Latitude = "-111.935502";
    String Longitude = "33.4199292";
    String BuildingKey = "TESTNAME";
    String DatabaseName = "buildingDataBase";

    // Add 
    public void addData()
    {
        //Creating JSON - APPROACH 1
        JObject co_ord = new JObject();
        co_ord["Latitude"] = Latitude;
        co_ord["Longitude"] = Longitude;


        JObject buildingObj = new JObject();
        buildingObj["BuildingCode"] = "TEST_CODE";
        buildingObj["BuildingName"] = "TEST_BUILDING";
        buildingObj["Coordinates"] = co_ord;
        String jsonData = buildingObj.ToString();


        /*
       // Creating JSON APPROACH 2
        JObject n1 = new JObject{
          {         
                  playerLocation,new JObject{
                  {
                      "Time",playerTime
                  }}
          }};
        */


        /*
        // Creating JSON APPROACH 3
        //Building bd = new Building();
        //string jsonData = JsonUtility.ToJson(bd);
       */

        //Append Values to database
        print("Wrting to database values : " + jsonData);
        reference.Child(DatabaseName).Child(BuildingKey).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

                //TODO  
                // Decide with team
                print("ERROR: when accessing Data from Database");

            }
            else if (task.IsCompleted)
            {
                print("SUCCESS: DATA ADDED TO DATABASE");
            }
        });






        //Using REST API
        //RestClient.Put("https://asu-ar-app.firebaseio.com/"+ BuildingKey +".json", buildingObj.ToString());

    }
    public void updateData()
    {
        String NewBuildingCode = "NEWCODE";
        String UpdateBuildingKey = "TESTNAME";
        String DatabaseName = "buildingDataBase";
        reference.Child(DatabaseName).Child(UpdateBuildingKey).Child("BuildingCode").SetValueAsync(NewBuildingCode).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

                //TODO  
                // Decide with team
                print("ERROR: when accessing Data from Database");

            }
            else if (task.IsCompleted)
            {
                print("SUCCESS: DATA UPDATED IN DATABASE");
            }
        });

    }



    public void OnGetData()
    {
        // Get the conmplete snapshot of the database.
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //TODO  
                // Decide with team
                print("ERROR: when accessing Data from Database");
            }
            else if (task.IsCompleted)
            {
       
                DataSnapshot snapshot = task.Result;
                print("Data SnapShot = " + snapshot.GetRawJsonValue());
                print("A building SnapShot = " + snapshot.Child(DatabaseName).Child("ajmatthewscenter").GetRawJsonValue());
            }
           

           // FirebaseDatabase.DefaultInstance.GetReference("a j matthews center").ValueChanged += Script_ValueChanged;

        });
    }

    /*
    private void Script_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        nameText.text= e.Snapshot.Child("BuildingCode").GetValue(true).ToString();
        print("On value change = "+ e.Snapshot.Child("BuildingCode").GetValue(true).ToString());
    }
    */

    public void deleteData()
    {


        String DeleteBuildingKey = "TESTNAME";
        String DatabaseName = "buildingDataBase";
        reference.Child(DatabaseName).Child(DeleteBuildingKey).RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

                //TODO  
                // Decide with team
                print("ERROR: when accessing Data from Database");

            }
            else if (task.IsCompleted)
            {
                print("SUCCESS: DATA Deleted IN DATABASE");
            }
        });
    }

    }







/*   FIREBASE USING API
public class BuildingName : MonoBehaviour
{
    public Text nameText; //Display
    //public InputField nameText; //Input 
    public InputField buildingNameText; //Input 
    //public InputField locationText; //Input 


    //private System.Random random = new System.Random();

    // add two variable 
    //public static int playerScore;    
    public static string buildingName;
    //public static string playerTime;
    //public static string playerLocation;


    Building building = new Building();

    // Start is called before the first frame update
    private void Start()
    {
        //playerScore = random.Next(0, 101);
        //scoreText.text = "Score: " + playerScore;
    }

    public void OnSubmit()
    {
        //playerName = nameText.text;
        //playerTime = timeText.text;
        //playerLocation = locationText.text;
    }

    public void OnGetData()
    {
        RetriveFromDatabase();
        UpdateScore();
    }


    /*
    public void DeleteScore()
    {
        playerName = nameText.text;
        DeleteFromDatabase();
    }

 
    private void UpdateScore()
    {
        nameText.text = "BuildingName: " + building.bName;
        print("BuildingName: " + building.bName);
    }

    /*
  private void PostToDatabase()
  {  
      JObject t = new JObject();
      t["Time"]= timeText.text;
      JObject l= new JObject();
      l[PlayerScores.playerLocation] = t;
      print(l.ToString());



      JObject n1 = new JObject{
          {
                  playerLocation,new JObject{
                  {
                      "Time",playerTime
                  }}
          }};

      print("N1 = " + n1.ToString());
      User user = new User();
      RestClient.Put("https://fir-f7893.firebaseio.com/" + playerName + ".json",n1.ToString());

  }
 
    private void RetriveFromDatabase()
    {
        print("here");
        print("https://asu-ar-app.firebaseio.com/" + buildingNameText.text + "/BuildingName.json");

        RestClient.Get<Building>("https://asu-ar-app.firebaseio.com/"+buildingNameText.text+"/BuildingName.json").Then(response =>
        {
            building = response;
            print("inside");
            print("response ="+response.ToString());
            UpdateScore();
        });


        print("bye");
  
    }

    /*  private void DeleteFromDatabase()
      {
          RestClient.Delete("https://asu-ar-app.firebaseio.com/" + nameText.text + ".json");
      }



      private void UpdateDatabase()
      {
          RestClient.Put("https://asu-ar-app.firebaseio.com/" + nameText.text + ".json", user );
      }
    

}


*/