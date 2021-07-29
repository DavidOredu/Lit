using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Features_Text : Features_Object<TextMeshProUGUI>
{
    public UiFeatures.textType textType;
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
        switch (textType)
        {
            case UiFeatures.textType.titleText:
                ChangeTextSettings(uiFeatures.titleText);
                component.enableVertexGradient = true;
                component.colorGradient = uiFeatures.titleText.colorGradient;
                break;
            case UiFeatures.textType.gameText:
                ChangeTextSettings(uiFeatures.gameText);
                break;
            case UiFeatures.textType.headerText:
                ChangeTextSettings(uiFeatures.headerText);
                component.enableVertexGradient = true;
                component.colorGradient = uiFeatures.headerText.colorGradient;
                break;
        }
    }
    void ChangeTextSettings(TextMeshProUGUI text)
    {
        component.material = text.material;
        component.fontStyle = text.fontStyle;
        component.font = text.font;
        component.characterSpacing = text.characterSpacing;
    }
}
