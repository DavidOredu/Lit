using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class ObjectSpawnPoint : MonoBehaviour
    {
        
        private void Awake() => ObjectSpawnSystem.AddSpawnPoint(transform);
        private void OnDestroy() => ObjectSpawnSystem.RemoveSpawnPoint(transform);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
}

