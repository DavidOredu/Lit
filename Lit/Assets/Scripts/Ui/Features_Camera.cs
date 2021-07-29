using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Features_Camera : Features_Object<Canvas>
{
    private Camera uiCamera;

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
        if (component.worldCamera == null)
            if(Camera.main)
            component.worldCamera = Camera.main;
            else
            {
               uiCamera = GameObject.FindGameObjectWithTag("MainCamera1").GetComponent<Camera>();
                component.worldCamera = uiCamera;
            }
        
    }
}
