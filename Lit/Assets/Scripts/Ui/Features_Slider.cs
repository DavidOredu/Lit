using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_Slider : Features_Object<Slider>
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
        component.handleRect.gameObject.GetComponent<Image>().color = uiFeatures.sliderButtonColor;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
