using UnityEngine;

public class MagicProjectileScript : MonoBehaviour
{
    public Racer ownerRacer { get; set; }
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    public bool hasFired = false;
    public bool canControl = true;
    private bool hasCollided = false;

    public float speed = 0f;
    public Vector2 direction = new Vector2(0f, 0f);
    public float explosiveFactor = 10f;
    public float upwardsModifier = 2f;
    public float damageRadius = 6f;
    public ForceMode2D forceMode = ForceMode2D.Force;

    private RunnerDamagesOperator runnerDamages;

    public int damageInt;
    public float damagePercentage;
    public float damageRate;
    public Rigidbody2D rb { get; private set; }

    Timer lifetimeTimer;
    public float lifetime = 5f;

    Collision2D defaultCollision;

    #region Homing Variables
    public Transform target;
    public bool isHomingMissile = false;
    #endregion

    void Start()
    {
        lifetimeTimer = new Timer(lifetime);
        lifetimeTimer.SetTimer();
        defaultCollision = new Collision2D();
        rb = GetComponent<Rigidbody2D>();
        runnerDamages.InitDamages();
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }
    private void FixedUpdate()
    {
        if (isHomingMissile)
        {
            Vector2 direction = transform.position - target.position;
            direction.Normalize();

            float cross = Vector3.Cross(direction, transform.right).z;

            rb.angularVelocity = 360 * cross;
        }

        if (!hasCollided)
        {
            lifetimeTimer.UpdateTimer();
        }
        if (lifetimeTimer.isTimeUp && !hasCollided)
        {
            Collide(true);
        }
    }
    private void Update()
    {
        var thisCollider = GetComponent<CircleCollider2D>();
        var runnerCollider = ownerRacer.GetComponent<CapsuleCollider2D>();
   //     Physics2D.IgnoreCollision(thisCollider, runnerCollider, true);
        if (!hasCollided)
        {
            //   rb.AddForce(direction * speed);
            rb.velocity = direction * speed;
        }
    }
    public void Collide(bool collideWithDamage, Collision2D hit = null)
    {
        hasCollided = true;
        if(hit == null)
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z)));
        else
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(hit.transform.rotation.x, hit.transform.rotation.y, hit.transform.rotation.z)));

        var hitObjects = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach (var hitObject in hitObjects)
        {
            // an if statement can be placed here if we only need an explosive force on only players. the same condition in the damaging part of the code below can be used
            // set explosive force
            var objectRB = hitObject.GetComponent<Rigidbody2D>();
            if(objectRB != null)
                objectRB.AddExplosionForce(explosiveFactor * speed, transform.position, damageRadius, upwardsModifier, forceMode);
            if (collideWithDamage)
            {
                // take the player to the appropriate reaction 
                if (hitObject.gameObject.tag == "Player" || hitObject.gameObject.tag == "Opponent")
                {
                    // damage hit object
                    Utils.SetDamageVariables(runnerDamages, ownerRacer, damageInt, damagePercentage, damageRate, hitObject.gameObject);
                }
            }
        }

        if (canControl)
        {
            if (ownerRacer.GamePlayer.enemyPowerup == null)
            {
                ownerRacer.GamePlayer.powerupButton.TurnSelectableState(false);
                ownerRacer.GamePlayer.powerupButton.UsePowerup(true);
            }
            else if(ownerRacer.GamePlayer.powerupButton == null)
            {
                ownerRacer.GamePlayer.enemyPowerup.UsePowerup();
            }
        }

        //yield WaitForSeconds (0.05);
        foreach (GameObject trail in trailParticles)
        {
            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            curTrail.transform.parent = null;
            Destroy(curTrail, 3f);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);
        //projectileParticle.Stop();

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (!trail.gameObject.name.Contains("Trail"))
                continue;

            trail.transform.SetParent(null);
            Destroy(trail.gameObject, 2);
        }

    }
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (!hasCollided)
        {
            if(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Opponent"))
            {
                if (hit.collider.gameObject.GetComponent<Racer>().runner.stickmanNet.code != damageInt)
                {
                    Collide(true, hit);
                }
                else
                {
                    Physics2D.IgnoreCollision(hit.collider, hit.otherCollider, true);
                }
            }
            //else if(hit.collider.CompareTag("Projectile"))
            //{
            //    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(hit.transform.rotation.x, hit.transform.rotation.y, hit.transform.rotation.z)));

            //    Collide(false);
            //}
            else if (hit.collider.CompareTag("Obstacle"))
            {
                var obstacle = hit.collider.GetComponent<Obstacle>();

                Collide(false, hit);
                if (obstacle.currentObstacleType == Obstacle.ObstacleType.Breakable)
                {
                    obstacle.BreakObstacle();
                }
                else if (obstacle.currentObstacleType == Obstacle.ObstacleType.LaserOrb)
                {
                    obstacle.ExplodeLaserOrb();
                }
                else if (obstacle.currentObstacleType == Obstacle.ObstacleType.ReleasedLaserBarricade)
                {
                    obstacle.DestroyBarricade();
                }
            }
            else
            {
                Collide(false, hit);
            }
        }
    }
}