using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Features_Panel : Features_Object<Image>
{
    public UiFeatures.panelPart panelType;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        switch (panelType)
        {
            case UiFeatures.panelPart.PanelBorder:
                component.color = uiFeatures.panelsColor;
                break;
            case UiFeatures.panelPart.LineSegment:
                component.color = uiFeatures.lineColor;
                break;
            case UiFeatures.panelPart.Button:
                component.color = uiFeatures.buttonsColor;
                break;
            case UiFeatures.panelPart.Background:
                component.sprite = uiFeatures.bgImage.sprite;
                break;
        }

    }

}
