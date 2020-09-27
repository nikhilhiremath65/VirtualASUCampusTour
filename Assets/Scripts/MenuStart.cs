using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuStart : MonoBehaviour
{

    public void SceneLoader(string scenename){
        SceneManager.LoadScene(scenename);
    }

	// public void showToursScene(string scenename){
	// 	Application.LoadLevel(scenename);
	// }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
