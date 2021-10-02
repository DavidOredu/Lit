using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementExplosionScript : MonoBehaviour
{
    public Racer ownerRacer;
    public int damageType;
    public float damageStrength;

    public float explosiveForce;
    public float explosiveRadius;
    public float upwardsModifier;
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    public RunnerDamagesOperator runnerDamages;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Explode(bool explodeWithDamage)
    {
        var objectsToBlow = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);

        foreach (var objectToBlow in objectsToBlow)
        {
            var objectRB = objectToBlow.GetComponent<Rigidbody2D>();
            if (objectRB != null)
            {
                if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
                {
                    if (explodeWithDamage)
                    {
                        if (objectRB.GetComponent<StickmanNet>().currentColor.colorID != damageType)
                        {
                            objectRB.AddExplosionForce(explosiveForce, transform.position, explosiveRadius, upwardsModifier, forceMode);
                            Debug.Log($"Has exploded! BOOM!: {objectToBlow.name}");
                            Debug.Log($"Damage Type at point of explosion is: {damageType}");
                            runnerDamages.Damages[damageType].damaged = true;
                            runnerDamages.Damages[damageType].damageInt = damageType;
                            runnerDamages.Damages[damageType].damageStrength = damageStrength;
                            runnerDamages.Damages[damageType].racer = ownerRacer;
                            objectRB.transform.SendMessage("DamageRunner", runnerDamages);
                        }
                    }
                }
                if (objectRB.CompareTag("Obstacle"))
                {
                    var obstacle = objectRB.GetComponent<Obstacle>();
                    if(obstacle.currentObstacleType == Obstacle.ObstacleType.Breakable)
                    {
                        obstacle.BreakObstacle();
                    }
                    else if(obstacle.currentObstacleType == Obstacle.ObstacleType.LaserOrb)
                    {
                        obstacle.ExplodeLaserOrb();
                    }
                    else if(obstacle.currentObstacleType == Obstacle.ObstacleType.ReleasedLaserBarricade)
                    {
                        obstacle.DestroyBarricade();
                    }
                }
                if (objectRB.CompareTag("Projectile"))
                {
                    var projectileScript = objectRB.GetComponent<MagicProjectileScript>();
                    projectileScript.Collide(false);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}
