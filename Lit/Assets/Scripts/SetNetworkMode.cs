using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SetNetworkMode : MonoBehaviour
{
    public NetworkState.State networkState;
    private Button button;

    public static event Action OnModeSelected;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
   //     button.onClick.AddListener(() => SetTheNetworkMode());
    //    button.onClick.AddListener(() => ModeSelected());
    }

    void SetTheNetworkMode()
    {
        NetworkState.instance.currentNetworkState = networkState;
    }
    public void ModeSelected()
    {
        OnModeSelected?.Invoke();
    }
}
