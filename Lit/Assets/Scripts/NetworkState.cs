using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkState : SingletonDontDestroy<NetworkState>
{
    public State currentNetworkState { get; set; }
    public enum State
    {
        None,
        Network,
        NonNetwork
    }

    public override void Awake()
    {
        base.Awake();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        currentNetworkState = State.None;
    }

    // Update is called once per frame
    void Update()
    {

    }

  
}
