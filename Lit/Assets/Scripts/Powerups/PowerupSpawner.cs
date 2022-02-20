using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private Transform spawnPoint;
    private float startTime;
    [SerializeField]
    private float spawnTime;

    private float _spawnTime;

    private PowerupController controller;
    private PowerupBehaviour currentPowerup;

    public bool hasPowerup;
    // Start is called before the first frame update
    void Start()
    {
        hasPowerup = false;
        spawnPoint = transform;
        _spawnTime = spawnTime;
        startTime = Time.time;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(controller == null)
            controller = GameObject.FindGameObjectWithTag("PowerupHolder").GetComponent<PowerupController>();
        if (!hasPowerup)
            _spawnTime -= Time.deltaTime;
        if (_spawnTime <= 0)
        {
            currentPowerup = SpawnRandomPowerup(Random.Range(0, controller.powerups.Count));
            _spawnTime = spawnTime;       
            hasPowerup = true;
        }
        if (currentPowerup != null)
        {
            if (!currentPowerup.image.enabled)
                hasPowerup = false;
        }
        else
        {
            hasPowerup = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Opponent"))
        {
            //    if(Time.time >= startTime + spawnTime)
          //  hasPowerup = false;
            //StopCoroutine(SpawnRandomPowerup());
            //StartCoroutine(SpawnRandomPowerup());
        }
    }
    public GameObject spawnPowerup(Powerup powerup, Vector3 position)
    {
        GameObject powerupGameObject = Instantiate(powerup.prefab);


          var powerupBehaviour = powerupGameObject.GetComponent<PowerupBehaviour>();

        powerupBehaviour.powerupController = controller;

            powerupBehaviour.SetPowerup(powerup);

            powerupGameObject.transform.position = position;
        
        return powerupGameObject;
    }

   PowerupBehaviour SpawnRandomPowerup(int index)
    {
        GameObject powerupGO;
        powerupGO = spawnPowerup(controller.powerups[index], spawnPoint.position);
        return powerupGO.GetComponent<PowerupBehaviour>();
    }
}
