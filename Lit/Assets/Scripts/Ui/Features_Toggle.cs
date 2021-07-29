using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_Toggle : Features_Object<Toggle>
{
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        var block = component.colors;
        block.selectedColor = uiFeatures.buttonsColor;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
