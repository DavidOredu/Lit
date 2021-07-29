using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Features_ScrollRect : Features_Object<ScrollRect>
{
    public UiFeatures.ScrollRectType scrollRectType;

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
        switch (scrollRectType)
        {
            case UiFeatures.ScrollRectType.PlayerLobby:
                SetScrollRectFeatures(uiFeatures.playerLobby);
                break;
            case UiFeatures.ScrollRectType.BarPanel:
                SetScrollRectFeatures(uiFeatures.barPanel);
                break;
            case UiFeatures.ScrollRectType.LevelSelect:
                SetScrollRectFeatures(uiFeatures.levelSelect);
                break;
            default:
                break;
        }
    }
    private void SetScrollRectFeatures(ScrollRect scrollRect)
    {
        component.elasticity = scrollRect.elasticity;
        component.decelerationRate = scrollRect.decelerationRate;
        component.scrollSensitivity = scrollRect.scrollSensitivity;
        component.vertical = scrollRect.vertical;
        component.horizontal = scrollRect.horizontal;
    }
}
