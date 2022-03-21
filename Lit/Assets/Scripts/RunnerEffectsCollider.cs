using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEffectsCollider : MonoBehaviour
{
    private ParticleSystem pSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    public float damagePercentage;
    public float damageRate;
    private RunnerDamagesOperator runnerDamages;

    Racer racer;
    // Start is called before the first frame update
    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        runnerDamages.InitDamages();
    }

    // Update is called once per frame
    void Update()
    {
        //ParticleSystem.TriggerModule triggerModule;
        //triggerModule.outside = ParticleSystemOverlapAction.Callback;

        if (racer == null)
            racer = transform.root.GetComponent<Racer>();
    }
    private void OnParticleCollision(GameObject other)
    { 
        if(racer == null)
            racer = transform.root.GetComponent<Racer>();
        var damageInt = racer.runner.stickmanNet.currentColor.colorID;

        if (racer.isAwakened)
        {
            if (other.CompareTag("Player") || other.CompareTag("Opponent"))
            {
                if (!other.GetComponent<RacerDamages>().myDamages.IsDamaged())
                {
                    Utils.SetDamageVariables(runnerDamages, racer, damageInt, damagePercentage, damageRate, other);
                }
            }
            else if (other.CompareTag("Obstacle"))
            {
                var obstacle = other.GetComponent<Obstacle>();
                switch (obstacle.currentObstacleType)
                {
                    case Obstacle.ObstacleType.Breakable:
                        obstacle.BreakObstacle();
                        break;
                    case Obstacle.ObstacleType.LaserOrb:
                        obstacle.ExplodeLaserOrb();
                        break;
                    case Obstacle.ObstacleType.ReleasedLaserBarricade:
                        obstacle.DestroyBarricade();
                        break;

                    default:
                        break;
                }
            }
            else if (other.CompareTag("Projectile"))
            {
                var projectile = other.GetComponent<MagicProjectileScript>();
                projectile.Collide(false);
            }
        }
    }
}
