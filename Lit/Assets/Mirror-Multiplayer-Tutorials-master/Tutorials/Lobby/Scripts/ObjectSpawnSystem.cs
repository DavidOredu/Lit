using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DapperDino.Mirror.Tutorials.Lobby
{
    //public class ObjectSpawnSystem : NetworkBehaviour
    //{
    //    [SerializeField] private GameObject objectPrefab = null;

    //    private static List<Transform> spawnPoints = new List<Transform>();

    //    private int nextIndex = 0;

    //    public static void AddSpawnPoint(Transform transform)
    //    {
    //        spawnPoints.Add(transform);

    //        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    //    }
    //    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    //    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnObject;

    //    public override void OnStartClient()
    //    {
    //        InputManager.Add(ActionMapNames.Player);
    //        InputManager.Controls.Player.Look.Enable();
    //    }

    //    [ServerCallback]
    //    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnObject;

    //    [Server]
    //    public void SpawnObject(NetworkConnection conn)
    //    {
    //        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

    //        if (spawnPoint == null)
    //        {
    //            Debug.LogError($"Missing spawn point for object {nextIndex}");
    //            return;
    //        }

    //        GameObject objectInstance = Instantiate(objectPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
    //        NetworkServer.Spawn(objectInstance, conn);

    //        nextIndex++;
    //    }
    //}

    public class ObjectSpawnSystem : NetworkBehaviour
    {
        public GameObject prefab;
        private int nextIndex = 0;
      //  public int numberOfObjects;
      
      private static List<Transform> spawnPoints = new List<Transform>();

        public static void AddSpawnPoint(Transform transform)
        {
           spawnPoints.Add(transform);
         
           spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
         }
        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);
        
        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnObject;

      
        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnObject;

        [Server]
        public void SpawnObject(NetworkConnection conn)
        {
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

                    if (spawnPoint == null)
                    {
                        Debug.LogError($"Missing spawn point for object {nextIndex}");
                        return;
                    }

                    GameObject objectInstance = Instantiate(prefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
                    NetworkServer.Spawn(objectInstance, conn);

                    nextIndex++;
        }
    }
}


