using DapperDino.Mirror.Tutorials.Lobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour, IDamageable
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
    public MineType mineType = MineType.Mine;

    private GameObject explosionGO = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mineType == MineType.Orb && ownerRacer == null)
        {
            foreach (var racer in GameManager.instance.allRacers)
            {
                if(racer.isInNativeMap)
                {
                    ownerRacer = racer;
                    return; 
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //--------------FOR PLAYER OBJECTS-----------------//
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        {
            if (mineType == MineType.Mine)
            {
                if (ownerRacer == other.collider.GetComponent<Racer>()) { return; }

                Damage();
            }
            else if(mineType == MineType.Orb)
            {
                Damage();
            }
        }
        //------------------------------------------------//

        //---------------FOR OTHER DAMAGEABLE OBJECTS--------------//
        else 
        {
            if(other.collider.GetComponent<IDamageable>() == null) { return; }

            Damage();
        }
    }
    public void Damage()
    {
        explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
        var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
        var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

        Utils.SetMineToExplosionVariables(explosionComp, this);
        explosionComp.runnerDamages.InitDamages();
        explosionComp.Explode(false);
        Destroy(gameObject);
    }
    public enum MineType
    {
        Mine,
        Orb
    }
}
