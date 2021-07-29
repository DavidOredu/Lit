using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_GridLayoutGroup : Features_Object<GridLayoutGroup>
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
        component.padding.left = uiFeatures.leftPadding;
        component.padding.right = uiFeatures.rightPadding;
        component.padding.top = uiFeatures.topPadding;
        component.padding.bottom = uiFeatures.bottomPadding;
        component.cellSize = uiFeatures.cellSize;
        component.spacing = uiFeatures.spacing;
        component.startCorner = uiFeatures.startCorner;
        component.startAxis = uiFeatures.startAxis;
        component.childAlignment = uiFeatures.childAlignment;
        component.constraint = uiFeatures.constraint;
    }
}
