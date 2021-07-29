using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_ContentSizeFitter : Features_Object<ContentSizeFitter>
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
        component.horizontalFit = uiFeatures.horizontalFit;
        component.verticalFit = uiFeatures.verticalFit;
    }
}
