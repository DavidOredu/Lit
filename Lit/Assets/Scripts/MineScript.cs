using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public Racer ownerRacer;
    public int damageType;
    public float damagePercentage;
    public float damageRate;
    public float explosiveRadiusDecreasePercentage;
    public float explosiveForce;
    public float explosiveRadius;
    public float upwardsModifier;
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    private GameObject explosionGO = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //--------------FOR PLAYER OBJECTS-----------------//
        if (other.CompareTag("Player") || other.CompareTag("Opponent"))
        {
            if(ownerRacer == other.GetComponent<Racer>()) { return; }

            explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
            var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
            var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

            Utils.SetMineToBombVariables(explosionComp, this);

            explosionComp.runnerDamages.InitDamages();
            explosionComp.Explode(true);
            Destroy(gameObject);
        }
        //------------------------------------------------//

        //---------------FOR OBSTACLE OBJECTS--------------//
        else if (other.CompareTag("Obstacle"))
        {
            explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
            var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
            var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

            Utils.SetMineToBombVariables(explosionComp, this);
            var obstacleScript = other.GetComponent<Obstacle>();

            if(obstacleScript.currentObstacleType == Obstacle.ObstacleType.LaserOrb)
            {
                obstacleScript.ExplodeLaserOrb();
                explosionComp.Explode(false);
                Destroy(gameObject);
            }
            else if(obstacleScript.currentObstacleType == Obstacle.ObstacleType.Breakable)
            {
                obstacleScript.BreakObstacle();
                explosionComp.Explode(false);
                Destroy(gameObject);
            }
        }
        //----------------------------------------------------//

        //-------------------FOR PROJECTILE OBJECTS----------------------//
        else if (other.CompareTag("Projectile"))
        {
            explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
            var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
            var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

            Utils.SetMineToBombVariables(explosionComp, this);

            if (other.GetComponent<MagicProjectileScript>())
            {
                var projectileScript = other.GetComponent<MagicProjectileScript>();
                projectileScript.Collide(false);
                explosionComp.Explode(false);
                Destroy(gameObject);
            }
            else if (other.GetComponent<BombScript>())
            {
                var bombScript = other.GetComponent<BombScript>();
                bombScript.Explode(false);
                explosionComp.Explode(false);
                Destroy(gameObject);
            }
        }
        //------------------------------------------------------//
    }
}
