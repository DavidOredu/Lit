using DapperDino.Tutorials.Lobby;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject aiRunnerPrefab = null;

        private static List<Transform> spawnPoints = new List<Transform>();
        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        private int nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);

            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }
        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

        public override void OnStartClient()
        {
            DapperDino.Tutorials.Lobby.InputManager.Add(ActionMapNames.Player);
            DapperDino.Tutorials.Lobby.InputManager.Controls.Player.Look.Enable();
        }

        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            switch (NetworkState.instance.currentNetworkState)
            {
                case NetworkState.State.None:
                    break;
                case NetworkState.State.Network:
                    Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

                    if (spawnPoint == null)
                    {
                        Debug.LogError($"Missing spawn point for player {nextIndex}");
                        return;
                    }

                    GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);

                    NetworkServer.Spawn(playerInstance, conn);

                    nextIndex++;
                    break;
                case NetworkState.State.NonNetwork:
                    
                    Transform spawnPoint2 = spawnPoints.ElementAtOrDefault(nextIndex);

                    if (spawnPoint2 == null)
                    {
                        Debug.LogError($"Missing spawn point for player {nextIndex}");
                        return;
                    }

                    for(int i = Room.GamePlayers.Count - 1; i >= 0; i--)
                    {
                        switch (Room.GamePlayers[i].gamePlayerType)
                        {
                            case Racer.RacerType.Player:
                                GameObject playerInstance2 = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
                                NetworkServer.Spawn(playerInstance2, conn);
                                break;
                            case Racer.RacerType.Opponent:
                                GameObject opponentInstance = Instantiate(aiRunnerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
                                NetworkServer.Spawn(opponentInstance);
                                break;
                            default:
                                break;
                        }
                        nextIndex++;
                    }

                    break;
                default:
                    break;
            }
            
        }
    }
}
