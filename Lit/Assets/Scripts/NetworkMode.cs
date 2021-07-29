using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMode : Singleton<NetworkMode>
{
    public enum NetworkModeSetting
    {
        Network,
        NonNetwork,
    }

    public NetworkModeSetting currentNetworkMode;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        switch (currentNetworkMode)
        {
            case NetworkModeSetting.Network:
                break;
            case NetworkModeSetting.NonNetwork:
                break;
            default:
                break;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
