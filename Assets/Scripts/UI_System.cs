using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Tours.UI
{
public class UI_System : MonoBehaviour
{


	#region Variables
	[Header("system Events")]
	public UnityEvent onSwitchedScreen = new UnityEvent();


	private UI_Screen previousScreen;
	public UI_Screen PreviousScreen{get{return previousScreen;}}
	private Component[] screens  = new Component[0];
	private UI_Screen currentScreen;
	public UI_Screen CurrentScreen {get{ return currentScreen;}}
	#endregion

	#region Main Methods

    // Start is called before the first frame update
    void Start()
    {
        screens  = GetComponentsInChildren<UI_Screen>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion


    #region Helper Methods
    // to switch screen
    public void SwitchScreens(UI_Screen aScreen)
    {
    	if(aScreen)
    	{
    		if(currentScreen)
    		{
    			// currentScreen.Close();
    			previousScreen = currentScreen;
    		}

    		currentScreen = aScreen;
    		currentScreen.gameObject.SetActive(true);
    		// currentScreen.StartScreen();

    		if(onSwitchedScreen != null)
    		{
    			onSwitchedScreen.Invoke();
    		}

    	}
    }

    public void GoToPreviousScreen()
    {
    	if(previousScreen)
    	{
    		SwitchScreens(previousScreen);
    	}
    }

    public void LoadScene(int sceneIndex)
    {
    	StartCoroutine(WaitToLoadScene(sceneIndex));
    }

    IEnumerator WaitToLoadScene(int sceneIndex)
    {
    	yield return null;
    }
    #endregion
}
}