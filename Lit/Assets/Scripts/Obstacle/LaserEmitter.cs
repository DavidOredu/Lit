using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public float timeToSpawn = 1f;
    public float projectionForce;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPeriodically());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private IEnumerator SpawnPeriodically()
    {
        yield return new WaitForSeconds(timeToSpawn);
        SpawnOrb();

        StartCoroutine(SpawnPeriodically());
    }
    void SpawnOrb()
    {
        var orb = ObjectPooler.instance.SpwanFromPool("ElementOrb", transform.position, Quaternion.identity);
        var orbRB = orb.GetComponent<Rigidbody2D>();
        orbRB.AddForce(new Vector2(0f, projectionForce), ForceMode2D.Impulse);
    }
}
