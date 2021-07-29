using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class RoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] protected GameObject lobbyUI = null;
    [SerializeField] protected TMP_Text[] playerNameTexts = new TMP_Text[9];
    [SerializeField] protected TMP_Text[] playerReadyTexts = new TMP_Text[9];
    [SerializeField] protected Button startGameButton = null;
    [SerializeField] protected Button readyButton = null;
    [SerializeField] protected List<Stickman> displayPrefabs = new List<Stickman>();

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar(hook = nameof(HandleChosenColor))]
    public bool hasChosenColor = false;
    [SyncVar(hook = nameof(ChangeStickmanColor))]
    public int currentColorCode;

    public Racer.RacerType roomPlayerType;
    protected PlayerData playerData;
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleChosenColor(bool oldValue, bool newValue) => UpdateDisplay();
    public void ChangeStickmanColor(int oldValue, int newValue) => UpdateDisplay();
    

    protected bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }
    private NetworkManagerLobby room;
    protected NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public bool foundColor { get; set; } = false;
    protected virtual void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
        for (int i = 0; i < displayPrefabs.Count; i++)
        {
            displayPrefabs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            displayPrefabs[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            displayPrefabs[i].code = Room.RoomPlayers[i].currentColorCode;
        }
        
    }
    [Command]
    protected void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }
    [Command]
    public void CmdStartGame()
    {
        //if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }
}
