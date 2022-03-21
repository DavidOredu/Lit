using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Features_MainCamera : Features_Object<Camera>
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

        var cameraData = component.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = uiFeatures.renderPostProcessing;
        cameraData.volumeLayerMask = uiFeatures.volumeLayerMask;
        
    }
}
