using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkLobbyHook : NetworkBehaviour
{
    [SyncVar]
    public int code;

    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        code = playerData.colorCode;
    }
}
