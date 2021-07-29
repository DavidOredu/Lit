using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[System.Serializable]
public class ColorButtons 
{
    public ColorStateCode color;
    public Button button;


    public bool isPicked;

   
    public void SetInteractable()
    {
        if (isPicked)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
