using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;

public class LevelButton : MonoBehaviour
{
    public NetworkState.State networkState;

    public Button button;
    public TextMeshProUGUI levelText;
    public Image gameModeImage;
    public Level level;

    private NetworkManagerLobby networkManager;
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
    private void Awake()
    {
        //levelButton = GetComponent<Button>();
        //levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {

    }
    void SetTheNetworkMode()
    {
        NetworkState.instance.currentNetworkState = networkState;
    }
    // Update is called once per frame
    void Update()
    {
        if(networkManager == null)
        {
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerLobby>();
        }
        button.interactable = level.isUnlocked;
        if(networkManager == null) { return; }

        button.onClick.AddListener(() => OnLevelSelected());
        button.onClick.AddListener(() => SetTheNetworkMode());
        button.onClick.AddListener(() => networkManager.StartHost());
     //   levelButton.onClick.AddListener(() => networkManager.StartGame());
    }
    // Tells the level manager that this level is the current or most recent level selected
    private void OnLevelSelected()
    {
        LevelManager.instance.selectedLevel = this;
    }
    private void OnEnable()
    {
        
    }
}
