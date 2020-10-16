using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class User
{
    // Start is called before the first frame update
    public string uName;
    public string uLocation;
    public string uTime;


    public User()
    {
        uName = Input_data.userName;
        uLocation = Input_data.location;
        uTime = Input_data.time;
    }
}