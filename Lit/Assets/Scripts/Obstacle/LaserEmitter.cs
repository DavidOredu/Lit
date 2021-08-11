using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public Timer spawnTimer;
    public float timeToSpawn = 1f;
    public float projectionForce;
    public GameObject laserOrb;

    private bool canSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = new Timer(timeToSpawn);
        spawnTimer.SetTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn)
        {
            SpawnOrb();
            canSpawn = false;
        }
        else
        {
            if (!spawnTimer.isTimeUp)
            {
                spawnTimer.UpdateTimer();
            }
            else
            {
                canSpawn = true;
                spawnTimer.ResetTimer();
            }
        }
    }
    void SpawnOrb()
    {
        var orb = Instantiate(laserOrb, transform.position, Quaternion.identity);
        var orbRB = orb.GetComponent<Rigidbody2D>();
        orbRB.AddForce(new Vector2(0f, projectionForce), ForceMode2D.Impulse);
    }
}
