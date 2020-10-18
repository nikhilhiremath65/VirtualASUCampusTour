using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    static ScheduleNameTransfer s;
    public void SceneLoader(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        Text[] texts = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>();
        
            print("Clicked on : " + texts[0].text);
        s = ScheduleNameTransfer.Instance;
        s.setScheduleName(texts[0].text);
        
    }
}
