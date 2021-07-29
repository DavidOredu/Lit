using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerGameModes : MonoBehaviour
{
    public GameModes.Modes gameMode;
    public NetworkState.State networkState;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => SetTheNetworkMode());
        button.onClick.AddListener(() => SetGameMode());
    }
    void SetTheNetworkMode()
    {
        NetworkState.instance.currentNetworkState = networkState;
    }
    void SetGameMode()
    {
        GameManager.instance.currentGameMode = gameMode;
    }
    // Update is called once per frame
    
}
