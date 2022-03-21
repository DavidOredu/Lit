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

        protected GameManager gameManager;
        protected GameManager GameManager
        {
            get
            {
                if (gameManager != null) { return gameManager; }
                return gameManager = GameManager.instance;
            }
        }

        private MapHandler mapHandler;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<RoomPlayerLobby> RoomPlayers { get; private set; } = new List<RoomPlayerLobby>();

        public List<GamePlayerLobby> GamePlayers { get; private set; } = new List<GamePlayerLobby>();

        public List<int> freeColors { get; private set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

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

        [Server]
        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
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

                        // Instantiate opponent runners for the level
                        for (int i = 0; i < GameManager.currentLevel.numberOfOpponentsInLevel; i++)
                        {
                            var opponentRoomPlayer = Instantiate(opponentRoomPlayerPrefab);
                            opponentRoomPlayer.difficulty = GameManager.currentLevel.difficulties[i];
                            opponentRoomPlayer.IsLeader = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

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
            // From menu to game
            switch (NetworkState.instance.currentNetworkState)
            {
                case NetworkState.State.Network:
                    if (SceneManager.GetActiveScene().path == menuScene && newSceneName.Contains("Scene_Game"))
                    {
                        // A reverse loop is used here because the first room player at index 0 is our main or 'server' runner. We should replace his connection last since the network originates from him.
                        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                        {
                            // Set the Game Player properties from the Room Player properties.
                            var conn = RoomPlayers[i].connectionToClient;
                            var gameplayerInstance = Instantiate(networkGamePlayerPrefab);
                            gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                            gameplayerInstance.SetColor(RoomPlayers[i].currentColorCode);

                            // Destroy the previous object with the connection "conn". That must be the Room Player.
                            NetworkServer.Destroy(conn.identity.gameObject);
                            // Replace the connection "conn" with the newly instantiated Game Player object.
                            NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                        }
                    }
                    break;

                case NetworkState.State.NonNetwork:
                    if (SceneManager.GetActiveScene().path == menuScene && newSceneName.Contains("Scene_Game"))
                    {
                        // A reverse loop is used here because the first room player at index 0 is our main or 'server' runner. We should replace his connection last since the network originates from him.
                        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                        {
                            // Instantiate variables.
                            var conn = RoomPlayers[i].connectionToClient;
                            GamePlayerLobby gamePlayerInstance;

                            // if we are a player type, instantiate a player type prefab. Else, instantiate an opponent type prefab.
                            if (RoomPlayers[i].roomPlayerType == Racer.RacerType.Player)
                                gamePlayerInstance = Instantiate(nonNetworkGamePlayerPrefab);
                            else
                                gamePlayerInstance = Instantiate(opponentGamePlayerPrefab);

                            // Set the Game Player properties from the Room Player properties.
                            gamePlayerInstance.gamePlayerType = RoomPlayers[i].roomPlayerType;
                            gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                            gamePlayerInstance.SetColor(RoomPlayers[i].currentColorCode);

                            // Destroy the previous object with the connection "conn". That must be the Room Player.
                            NetworkServer.Destroy(RoomPlayers[i].gameObject);
                            // Replace the connection "conn" with the newly instantiated Game Player object. Do this only for the main player object.
                            if(gamePlayerInstance.gamePlayerType == Racer.RacerType.Player)
                                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);

                            // If we are an opponent, do an extra property adjustment and Network Spawn the object intead of replace the connection since we are running them on the same computer.
                            // and we don't want to replace the main player's connection.
                            if (gamePlayerInstance.gamePlayerType == Racer.RacerType.Opponent)
                            {
                                gamePlayerInstance.difficulty = RoomPlayers[i].difficulty;
                                NetworkServer.Spawn(gamePlayerInstance.gameObject);
                            }
                        }
                    }
                    break;
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.Contains("Scene_Game"))
            {
                // Spawn the system that will spawn the network player object. The main racer objects.
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);

                // Spawn the system responsible for setting and changing rounds.
                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);

                // In case we want to use an object spawn system, uncomment this.
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
