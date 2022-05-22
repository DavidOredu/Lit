using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    public GameObject laserOrb;
    public Vector2 projectionForce;

    public Timer spawnTimer;
    public float timeToSpawn = .8f;
    private float yProjectionTemp;

    public bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = new Timer(timeToSpawn);
        spawnTimer.SetTimer();

        yProjectionTemp = projectionForce.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    private IEnumerator SpawnOrb()
    {
        yield return new WaitForSeconds(timeToSpawn);

        var spawnPositions = GetComponentsInChildren<Transform>();
        Debug.Log($"Spawn Position for laser orbs are {spawnPositions.Length}!");
        foreach (var position in spawnPositions)
        {
            var orb = ObjectPooler.instance.SpwanFromPool("ElementOrb", position.position, Quaternion.identity);
            var orbRB = orb.GetComponent<Rigidbody2D>();
            orbRB.velocity = projectionForce;
            projectionForce.y -= yProjectionTemp;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Opponent")
        {
            StartCoroutine(SpawnOrb());
        }
    }
}
