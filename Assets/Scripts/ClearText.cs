using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClearText : MonoBehaviour
{
    public Button clearButton;
    private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = clearButton.GetComponent<Button>();
        inputField = GetComponent<InputField>();
        btn.onClick.AddListener(ClearOnClick);

    }

    // Update is called once per frame
    void ClearOnClick()
    {
        inputField.text = "";
        // Debug.Log("You pressed clear button");
    }


}
