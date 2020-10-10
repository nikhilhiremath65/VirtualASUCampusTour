using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;


[Serializable]
public class Building { 
    // Start is called before the first frame update
    public string bName;

    public Building()
    {
        bName = BuildingName.buildingName;
       
    }

}
