using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch_button : MonoBehaviour
{
    public GameObject MapCamera;
    public GameObject MainCamera;
    public Text ViewText;
    private string view_selected;
    private Singleton singleton;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        singleton = Singleton.Instance();

        view_selected = singleton.getViewName();

        if (MapCamera.active)
        {
            Debug.Log("Map Camera");
            if (view_selected == "AR View")
            {
                Debug.Log("AR Camera");
                MapCamera.SetActive(false);
                MainCamera.SetActive(true);
                ViewText.text = "Map View";
            }
            if (view_selected == "Map View")
            {
                Debug.Log("map Camera");
                MainCamera.SetActive(false);
                MapCamera.SetActive(true);
                ViewText.text = "AR View";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
