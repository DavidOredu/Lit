using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using UnityEngine;

//ROLE: To initialize the "runner" player gameobject in the network manager since network manager does not view it as a local player
public class InitPlayerNetwork : NetworkBehaviour
{
    // get the stickman component attached to the gameobject
    [SerializeField] private StickmanNet stickmanNet;

    //Singleton instance of the network manager lobby 
    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void OnStartClient()
    {
        if (!Room.players.Contains(stickmanNet)) // if the stickman component attached to this gameobject is not found in the stickman player list...
            Room.players.Add(stickmanNet);  // add said stickman to the list
    }

    // Update is called once per frame
    void Update()
    {

    }
}
