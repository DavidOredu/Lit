using UnityEngine;

public class BombTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float explosiveForce;
    [SerializeField] private float explosiveRadius;
    [SerializeField] private ForceMode2D forceMode;
    [SerializeField] private float upwardsModifier;

    public Racer racer;
    void Explode()
    {
        var objectsToBlow = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
        Debug.Log($"{objectsToBlow[0]}, {objectsToBlow[1]}, {objectsToBlow[2]}");
        foreach (var objectToBlow in objectsToBlow)
        {
            var objectRB = objectToBlow.GetComponent<Rigidbody2D>();
            if (objectRB != null && objectRB.CompareTag("Opponent"))
            {
                objectRB.AddExplosionForce(explosiveForce, transform.position, explosiveRadius, upwardsModifier, forceMode);
                Debug.Log($"Has exploded! BOOM!: {objectToBlow.name}");
            }
        }
    }
    void BlowGamobject()
    {
        rb.AddExplosionForce(explosiveForce, transform.position, explosiveForce, upwardsModifier, forceMode);
        Debug.Log("Has exploded! BOOM!");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            BlowGamobject();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}
