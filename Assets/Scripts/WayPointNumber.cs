using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointNumber : MonoBehaviour
{

    public Text NumberLabel;

    // Update is called once per frame
    void Update()
    {
        Vector3 numberPos = Camera.main.WorldToScreenPoint(this.transform.position);
        NumberLabel.transform.position = numberPos;
    }
}
