using UnityEngine;

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
        component.renderMode = RenderMode.ScreenSpaceCamera;
        if (component.worldCamera == null)
        {
            if (Camera.main)
                component.worldCamera = Camera.main;
        }
        // in case we want to split the camera functions
        //{
        //    if (GameObject.FindGameObjectWithTag("UICamera"))
        //    {
        //        uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        //        component.worldCamera = uiCamera;
        //    }
        //}

    }
}
