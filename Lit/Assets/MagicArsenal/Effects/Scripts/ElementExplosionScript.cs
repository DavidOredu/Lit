using UnityEngine;

public class ElementExplosionScript : MonoBehaviour
{
    public Racer ownerRacer;
    public int damageInt;
    public float damagePercentage;
    public float damageRate;

    public float explosiveForce;
    public float explosiveRadius;
    public float upwardsModifier;
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    public RunnerDamagesOperator runnerDamages;

    public BombScript bombScript;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void Explode(bool explodeWithDamage)
    {
        if (bombScript)
        {
            bombScript.EndBomb();
            Destroy(bombScript.gameObject);
        }

        var objectsToBlow = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
        for (int i = 0; i < objectsToBlow.Length; i++)
        {
            var objectRB = objectsToBlow[i].GetComponent<Rigidbody2D>();
            
            if (objectRB != null)
            {
                objectRB.AddExplosionForce(explosiveForce, transform.position, explosiveRadius, upwardsModifier, forceMode);
            }

            if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
            {
                if (explodeWithDamage)
                {
                    Debug.Log($"Has exploded! BOOM!: {objectsToBlow[i].name}");
                    Debug.Log($"Damage Type at point of explosion is: {damageInt}");
                    Utils.SetDamageVariables(runnerDamages, ownerRacer, damageInt, damagePercentage, damageRate, objectsToBlow[i].gameObject);

                }
            }
            if (GetComponent<IDamageable>() != null)
            {
                var damageable = GetComponent<IDamageable>();
                damageable.Damage();
            }
        }
    }
    [ContextMenu("Explode")]
    public void DebugExplode()
    {
        Explode(true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}
