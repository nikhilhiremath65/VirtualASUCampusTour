using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorClickOk : MonoBehaviour
{
    public GameObject ErrorPanel;

    public void onClickOk()
    {
        ErrorPanel.SetActive(false);
    }
}
