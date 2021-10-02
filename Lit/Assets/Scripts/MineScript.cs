using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public Racer ownerRacer;
    public int damageType;
    public float damageStrength;

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
        if (explosionGO == null)
            explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
        var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

        explosionComp.ownerRacer = ownerRacer;
        explosionComp.damageType = damageType;
        explosionComp.damageStrength = damageStrength;
        explosionComp.explosiveForce = explosiveForce;
        explosionComp.explosiveRadius = explosiveRadius;
        explosionComp.upwardsModifier = upwardsModifier;
        explosionComp.forceMode = forceMode;

        explosionComp.runnerDamages.InitDamages();
        explosionComp.Explode(true);
        Destroy(gameObject);
        //if(other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        //{
        //    // instantiate explosive damage
        //}
        //if (other.collider.CompareTag("Projectile"))
        //{

        //}
        //instantiate explosive effect
    }
}
