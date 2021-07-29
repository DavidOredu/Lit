using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorStateCode
{
    public int colorID;
    public Color color;
    public bool isPicked;

    public ColorStateCode(int colorID, Color color, bool isPicked)
    {
        this.colorID = colorID;
        this.color = color;
        this.isPicked = isPicked;
    }
}
