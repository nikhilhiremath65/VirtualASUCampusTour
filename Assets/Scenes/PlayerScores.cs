using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

public class PlayerScores : MonoBehaviour
{
    public Text scoreText; //Display
    public InputField nameText; //Input 
    public InputField timeText; //Input 
    public InputField locationText; //Input 


    private System.Random random = new System.Random();

    // add two variable 
    public static int playerScore;
    public static string playerName;
    public static string playerTime;
    public static string playerLocation;


    User user = new User();

    // Start is called before the first frame update
    private void Start()
    {
        playerScore = random.Next(0, 101);
        scoreText.text = "Score: " + playerScore;
    }

    public void OnSubmit()
    {
        playerName = nameText.text;
        playerTime = timeText.text;
        playerLocation = locationText.text;
        PostToDatabase();
    }

    public void OnGetScore()
    {

        RetriveFromDatabase();
    }

    public void DeleteScore()
    {
        playerName = nameText.text;
        DeleteFromDatabase();
    }


    private void UpdateScore()
    {
        scoreText.text = "Score: " + user.userScore;
    }


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
       
        RestClient.Get<User>("https://fir-f7893.firebaseio.com/" + nameText.text + ".json").Then(response=>
        {
            user= response;
            UpdateScore();
        });
        
    }

    private void DeleteFromDatabase()
    {
        RestClient.Delete("https://fir-f7893.firebaseio.com/" + nameText.text + ".json");
    }



    private void UpdateDatabase()
    {
        RestClient.Put("https://fir-f7893.firebaseio.com/" + nameText.text + ".json", user );
    }

}
