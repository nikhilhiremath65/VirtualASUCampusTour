using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setARType : MonoBehaviour
{
    public void setType(string type)
    {
        Singleton singleton = Singleton.Instance();
        singleton.setARType(type);
    }
}
