using UnityEngine;

public class MagicProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    private bool hasCollided = false;

    public float speed = 0f;
    public Vector2 direction = new Vector2(0f, 0f);
    public float explosiveFactor = 10f;
    public float upwardsModifier = 2f;
    public float damageRadius = 6f;
    public ForceMode2D forceMode = ForceMode2D.Force;

    private RunnerDamagesOperator runnerDamages;

    public int damageType;
    public float damageTime;
    public Rigidbody2D rb { get; private set; }

    Timer lifetimeTimer;
    public float lifetime = 5f;

    Collision2D defaultCollision;
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
        if (!hasCollided)
        {
            lifetimeTimer.UpdateTimer();
        }
        if (lifetimeTimer.isTimeUp && !hasCollided)
        {
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z)));

            Collide(true);
        }
    }
    private void Update()
    {
        if (!hasCollided)
        {
            //   rb.AddForce(direction * speed);
            rb.velocity = direction * speed;
        }
    }
    void Collide(bool collideWithDamage)
    {


        hasCollided = true;
        //  transform.DetachChildren();
        //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);
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
                if ((hitObject.gameObject.tag == "Player" || hitObject.gameObject.tag == "Opponent") && hitObject.gameObject.GetComponent<Racer>().runner.stickmanNet.code != damageType)
                {
                    // damage hit object
                    runnerDamages.Damages[damageType].damaged = true;
                    runnerDamages.Damages[damageType].damageTime = damageTime;
                    hitObject.transform.SendMessage("DamageRunner", runnerDamages);
                }
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
                if (hit.collider.gameObject.GetComponent<Racer>().runner.stickmanNet.code != damageType)
                {
                    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(hit.transform.rotation.x, hit.transform.rotation.y, hit.transform.rotation.z)));

                    Collide(true);
                }
                else
                {
                    Physics2D.IgnoreCollision(hit.collider, hit.otherCollider);
                }
            }
            else if(hit.collider.CompareTag("Projectile"))
            {
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(hit.transform.rotation.x, hit.transform.rotation.y, hit.transform.rotation.z)));

                Collide(false);
            }    
            else
            {
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, new Vector3(hit.transform.rotation.x, hit.transform.rotation.y, hit.transform.rotation.z)));

                Collide(true);
            }
        }
    }
}