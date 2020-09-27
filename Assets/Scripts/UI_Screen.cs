using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Tours.UI
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CanvasGroup))]
	public class UI_Screen : MonoBehaviour
	{

		#region  Variables
		[Header("Main Properties")]
		public Selectable m_StartSelectable;

		[Header("Screen Events")]
		public UnityEvent onScreenStart = new UnityEvent();
		public UnityEvent onScreenClose = new UnityEvent();

		private Animator animator;

		#endregion


		#region Main Methods

    	// Start is called before the first frame update
    	void Start()
    	{
        	animator = GetComponent<Animator>();

        	if(m_StartSelectable)
        	{
        		EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
        	}
    	}

    	// Update is called once per frame
    	void Update()
    	{
        	
   	 	}
    	#endregion



    	#region Helper Methods
    	public virtual void StartScreen()
    	{
    		if(onScreenStart != null)
    		{
    			onScreenStart.Invoke();
    		}

    	}

    	public virtual void CloseScreen()
    	{
    		if(onScreenClose != null)
    		{
    			onScreenClose.Invoke();
    		}
    		HandleAnimator("show");

    	}


    	void HandleAnimator(string aTigger)
    	{
    		if(animator)
    		{
    			animator.SetTrigger(aTigger);
    		}
    		HandleAnimator("hide");
    	}


   		 #endregion
}
}