using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LitPlatformSetter : MonoBehaviour
{
    private NetworkState networkState;
    private LitPlatform litPlatform;
    private LitPlatformNetwork litPlatformNetwork;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        networkState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<NetworkState>();
        litPlatform = GetComponent<LitPlatform>();
        litPlatformNetwork = GetComponent<LitPlatformNetwork>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (networkState.currentNetworkState)
        {
            case NetworkState.State.Network:

                if (litPlatform != null)
                {
                    Destroy(gameObject);
                }

                break;
            case NetworkState.State.NonNetwork:

                if (litPlatformNetwork != null)
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }
}
