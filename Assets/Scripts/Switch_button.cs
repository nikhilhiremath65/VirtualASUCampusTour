using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch_button : MonoBehaviour
{
    public GameObject MapCamera;
    public GameObject ARCamera;
    public Text ViewText;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        
        if (ARCamera.active)
        {
            MapCamera.SetActive(false);
            ViewText.text = "Map View";
        }
        else if(MapCamera.active)
        {
            ARCamera.SetActive(false);
            ViewText.text = "AR View";
        }
        else
        {
            Debug.Log("No View selected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
