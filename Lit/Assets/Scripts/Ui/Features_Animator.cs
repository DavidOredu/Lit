using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Features_Animator : Features_Object<Animator>
{
    public UiFeatures.AnimatorType currentAnimatorType;
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
        switch (currentAnimatorType)
        {
            case UiFeatures.AnimatorType.BackgroundAnimator:
                SetAnimatorType(uiFeatures.bgAnimator);
                break;
            case UiFeatures.AnimatorType.UiPopUpAnimator:
                SetAnimatorType(uiFeatures.popUpAnimator);
                break;
            default:
                break;
        }
        
     //   component.playbackTime = Random.Range(0f, 80f);
    }
    void SetAnimatorType(Animator animator)
    {
        component.runtimeAnimatorController = animator.runtimeAnimatorController;
    }
}
