using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    static ScheduleNameTransfer s;
    static Singleton q;
    public void SceneLoader(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.SetAsLastSibling();
    }

    public void OnButtonClick()
    {
        Text[] texts = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>();
        
            print("Clicked on : " + texts[0].text);
        s = ScheduleNameTransfer.Instance;
        s.setScheduleName(texts[0].text);
        q = Singleton.Instance();
        q.setScheduleName(texts[0].text);
    }

    public void OnDeptButtonClick()
    {
        Text[] texts = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>();

        print("Clicked on : " + texts[0].text);
        q = Singleton.Instance();
        q.setTourName(texts[0].text);

    }


}
