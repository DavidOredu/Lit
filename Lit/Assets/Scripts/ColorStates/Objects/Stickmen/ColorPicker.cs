using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using DapperDino.Mirror.Tutorials.Lobby;

public class ColorPicker : MonoBehaviour
{
    
    public static ColorPicker instance;
    public static ColorStateCode blackColor;
    public static ColorStateCode redColor;
    public static ColorStateCode blueColor;
    public static ColorStateCode greenColor;
    public static ColorStateCode yellowColor;
    public static ColorStateCode purpleColor;
    public static ColorStateCode cyanColor;
    public static ColorStateCode magentaColor;
    public static ColorStateCode orangeColor;
    public static ColorStateCode yellowGreenColor;
    public static ColorStateCode whiteColor;

  
    public bool isPicked;

    

    public List<ColorStateCode> colorStateCodes = new List<ColorStateCode>();
    public List<ColorButtonNetwork> colorButtonNetworks = new List<ColorButtonNetwork>();
    //public List<Button> colorButtons = new List<Button>();
    public List<ColorButtons> colorButtonss = new List<ColorButtons>();
    private void Awake()
    {
        instance = this;

        blackColor = new ColorStateCode(0, Color.black, false);
        redColor = new ColorStateCode(1, Color.red, false);
        blueColor = new ColorStateCode(2, Color.blue, false);
        greenColor = new ColorStateCode(3, Color.green, false);
        yellowColor = new ColorStateCode(4, Color.yellow, false);
        purpleColor = new ColorStateCode(5, Color.gray, false);
        cyanColor = new ColorStateCode(6, Color.cyan, false);
        magentaColor = new ColorStateCode(7, Color.magenta, false);
        orangeColor = new ColorStateCode(8, Color.white, false);
        yellowGreenColor = new ColorStateCode(9, Color.white, false);
        whiteColor = new ColorStateCode(10, Color.white, false);

        

        colorStateCodes.Add(blackColor);
        colorStateCodes.Add(redColor);
        colorStateCodes.Add(blueColor);
        colorStateCodes.Add(greenColor);
        colorStateCodes.Add(yellowColor);
        colorStateCodes.Add(purpleColor);
        colorStateCodes.Add(cyanColor);
        colorStateCodes.Add(magentaColor);
        colorStateCodes.Add(orangeColor);
        colorStateCodes.Add(yellowGreenColor);
        colorStateCodes.Add(whiteColor);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    
   
   
}
