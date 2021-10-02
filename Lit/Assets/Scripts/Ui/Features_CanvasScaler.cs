using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_CanvasScaler : Features_Object<CanvasScaler>
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
        component.uiScaleMode = uiFeatures.scaleMode;
        component.referenceResolution = uiFeatures.referenceResolution;
        component.screenMatchMode = uiFeatures.screenMatchMode;
        component.matchWidthOrHeight = uiFeatures.matchWidthOrHeight;
    }
}
