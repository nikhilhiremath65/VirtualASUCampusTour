using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMapAR : MonoBehaviour
{
    public GameObject mapCamera;
    public GameObject buttonToSwitchView;
    public GameObject scanButton;
    private Singleton singleton;

    public GameObject startLocationTextBox;
    public GameObject destinationLocaitonTextBox;
    // Start is called before the first frame update
    void Start()
    {
        singleton = Singleton.Instance();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        if(mapCamera.activeSelf)
        {
            mapCamera.SetActive(false);
            buttonToSwitchView.GetComponent<Button>().GetComponentInChildren<Text>().text = "Map";
            singleton.setMapMode(false);
            startLocationTextBox.SetActive(false);
            scanButton.SetActive(true);
            destinationLocaitonTextBox.SetActive(false);
        }
        else
        {
            mapCamera.SetActive(true);
            buttonToSwitchView.GetComponent<Button>().GetComponentInChildren<Text>().text = "AR";
            singleton.setMapMode(true);
            startLocationTextBox.SetActive(true);
            scanButton.SetActive(false);
            destinationLocaitonTextBox.SetActive(true);
        }

    }
}
