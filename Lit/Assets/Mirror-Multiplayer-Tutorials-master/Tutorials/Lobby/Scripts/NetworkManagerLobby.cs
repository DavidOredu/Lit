using DapperDino.Tutorials.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Maps")]
        [SerializeField] private int numberOfRounds = 1;
        [SerializeField] private MapSet mapSet = null;

        [Header("Room")]
        [SerializeField] private RoomPlayerLobby networkRoomPlayerPrefab = null;
        [SerializeField] private RoomPlayerLobby nonNetworkRoomPlayerPrefab = null;
        [SerializeField] private RoomPlayerLobby opponentRoomPlayerPrefab = null;


        [Header("Game")]
        [SerializeField] private GamePlayerLobby networkGamePlayerPrefab = null;
        [SerializeField] private GamePlayerLobby nonNetworkGamePlayerPrefab = null;
        [SerializeField] private GamePlayerLobby opponentGamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;
        [SerializeField] private GameObject objectSpawnSystem = null;
        [SerializeField] private GameObject roundSystem = null;

        private MapHandler mapHandler;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<RoomPlayerLobby> RoomPlayers { get; } = new List<RoomPlayerLobby>();
        
        public List<GamePlayerLobby> GamePlayers { get; } = new List<GamePlayerLobby>();
        public List<StickmanNet> players { get; } = new List<StickmanNet>();
        public List<int> freeColors = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public readonly Dictionary<GamePlayerLobby, GameObject> gamePlayerConnect = new Dictionary<GamePlayerLobby, GameObject>();

        public int numberOfRoomPlayers;

        public override void Awake()
        {
            base.Awake();

            SetNetworkMode.OnModeSelected += SetNetworkMode_OnModeSelected;
        }
        
        private void SetNetworkMode_OnModeSelected()
        {
            StartServer();
            Debug.Log("The host has been started by the event action 'OnModeSelected'!");
        }
        private void Update()
        {
            Debug.Log($"{RoomPlayers.Count} room players exist in the list!");
        }
        [Server]
        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

            //for (int i = 0; i < 9; i++)
            //{
            //    colorCodes[i + 1] = false;
            //}
        }

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }

            //for (int i = 0; i < 9; i++)
            //{
            //    colorCodes[i + 1] = false;
            //}

        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            switch (NetworkState.instance.currentNetworkState)
            {
                case NetworkState.State.None:
                    break;
                case NetworkState.State.Network:
                    if (SceneManager.GetActiveScene().path == menuScene)
                    {
                        bool isLeader = RoomPlayers.Count == 0;

                        RoomPlayerLobby roomPlayerInstance = Instantiate(networkRoomPlayerPrefab);

                        roomPlayerInstance.IsLeader = isLeader;

                        NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

                    }
                    break;
                case NetworkState.State.NonNetwork:
                    if (SceneManager.GetActiveScene().path == menuScene)
                    {
                        bool isLeader = RoomPlayers.Count == 0;

                        RoomPlayerLobby roomPlayerInstance = Instantiate(nonNetworkRoomPlayerPrefab);

                        roomPlayerInstance.IsLeader = isLeader;

                        NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
                        var nonNetworkRoomPlayer = roomPlayerInstance.GetComponent<NonNetworkRoomPlayerLobby>();

                        // Instantiate opponent runners for the level
                        for (int i = 0; i < nonNetworkRoomPlayer.currentLevel.level.numberOfOpponentsInLevel; i++)
                        {
                            var opponentRoomPlayer = Instantiate(opponentRoomPlayerPrefab);
                            opponentRoomPlayer.IsLeader = false;
                        }
                    }
                    break;
                default:
                    break;
            }
            
        }
        //public void InstanDisplayPlayer()
        //{

        //    foreach (var roomPlayer in RoomPlayers)
        //    {
        //        if (roomPlayer == this)
        //        {
        //            var displayPlayerInstance = Instantiate(displayPlayer, displayPrefabs[RoomPlayers.IndexOf(roomPlayer)]);
        //            NetworkServer.Spawn(displayPlayerInstance);
        //            displayPlayerInstance.GetComponent<Stickman>().playerData = roomPlayer.playerData;
        //        }
        //    }


        //    foreach (var player in Room.RoomPlayers)
        //    {
        //        if (player.DisplayName == DisplayName)
        //        {

        //        }
        //    }
        //}

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            OnServerStopped?.Invoke();

            RoomPlayers.Clear();
            GamePlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) { return false; }

            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }

            return true;
        }

        public void StartGame()
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if (!IsReadyToStart()) { return; }

                mapHandler = new MapHandler(mapSet, numberOfRounds);

                ServerChangeScene(mapHandler.NextMap);
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            switch (NetworkState.instance.currentNetworkState)
            {
                case NetworkState.State.None:
                    break;
                case NetworkState.State.Network:
                    if (SceneManager.GetActiveScene().path == menuScene && newSceneName.Contains("Scene_Game"))
                    {
                        numberOfRoomPlayers = RoomPlayers.Count;
                        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                        {
                            var conn = RoomPlayers[i].connectionToClient;
                            var gameplayerInstance = Instantiate(networkGamePlayerPrefab);
                            gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                            //TODO: change back to normal
                            gameplayerInstance.SetColor(RoomPlayers[i].currentColorCode);

                            NetworkServer.Destroy(conn.identity.gameObject);

                            NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                        }
                    }
                    break;
                case NetworkState.State.NonNetwork:
                    if (SceneManager.GetActiveScene().path == menuScene && newSceneName.Contains("Scene_Game"))
                    {
                        numberOfRoomPlayers = RoomPlayers.Count;
                        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                        {
                            switch (RoomPlayers[i].roomPlayerType)
                            {
                                case Racer.RacerType.Player:
                                    var conn = RoomPlayers[i].connectionToClient;
                                    var gameplayerInstance = Instantiate(nonNetworkGamePlayerPrefab);
                                    gameplayerInstance.gamePlayerType = RoomPlayers[i].roomPlayerType;
                                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                                    //TODO: change back to normal
                                    gameplayerInstance.SetColor(RoomPlayers[i].currentColorCode);

                                    NetworkServer.Destroy(RoomPlayers[i].gameObject);

                                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                                    break;

                                case Racer.RacerType.Opponent:
                                    var conn2 = RoomPlayers[i].connectionToClient;
                                    var gameplayerInstance2 = Instantiate(opponentGamePlayerPrefab);
                                    gameplayerInstance2.gamePlayerType = RoomPlayers[i].roomPlayerType;
                                    gameplayerInstance2.SetDisplayName(RoomPlayers[i].DisplayName);
                                    //TODO: change back to normal
                                    gameplayerInstance2.SetColor(RoomPlayers[i].currentColorCode);

                                    NetworkServer.Destroy(RoomPlayers[i].gameObject);
                                    NetworkServer.Spawn(gameplayerInstance2.gameObject);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            // From menu to game
            

            base.ServerChangeScene(newSceneName);
          
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.Contains("Scene_Game"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);

                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);

                //GameObject objectSpawnSystemInstance = Instantiate(objectSpawnSystem);
                //NetworkServer.Spawn(objectSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }
    }
}
