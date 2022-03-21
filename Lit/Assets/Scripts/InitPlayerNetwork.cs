using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using UnityEngine;

/// <summary>
/// To initialize the "runner" player gameobject in the network manager since network manager does not view it as a local player.
/// </summary>
public class InitPlayerNetwork : NetworkBehaviour
{
    // get the stickman component attached to the gameobject
    [SerializeField] private Racer racer;
    [SerializeField] private StickmanNet stickmanNet;

    private GameManager gameManager;
    private GameManager GameManager
    {
        get
        {
            if(gameManager != null) { return gameManager; }
            return gameManager = GameManager.instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void OnStartClient()
    {
        if (!GameManager.allRacers.Contains(racer))
            GameManager.allRacers.Add(racer);
        if (!GameManager.allStickmenColors.Contains(stickmanNet)) // if the stickman component attached to this gameobject is not found in the stickman player list...
            GameManager.allStickmenColors.Add(stickmanNet);  // add said stickman to the list
    }

    // Update is called once per frame
    void Update()
    {

    }
}
