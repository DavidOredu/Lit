using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    public int damageType;
    public float orbSpeed;
    public float shockRadius;
    public float orbLifetime;
    public Racer ownerRacer;
    public LayerMask whatToDamage;

    public GameObject beamProjectile;

    private Timer orbTimer;
    private float initialYPosition;

    public List<Racer> struckRacers = new List<Racer>();

    // Start is called before the first frame update
    void Start()
    {
        orbTimer = new Timer(orbLifetime);
        orbTimer.SetTimer();

        initialYPosition = transform.position.y;
        SetOwner();

        GoToStartPosition();
    }

    private void OnEnable()
    {
        
    }
    private void SetOwner()
    {
        ownerRacer = transform.parent.GetComponent<Racer>();
        transform.parent = null;
    }
    private void GoToStartPosition()
    {
        transform.LeanMove(new Vector3(ownerRacer.transform.position.x, initialYPosition + 25, transform.position.z), .5f);
    }
    private void MoveOrb()
    {
        transform.LeanMove(new Vector3(transform.position.x + orbSpeed, transform.position.y, transform.position.z), .5f);
    }
    private void LookForPlayers()
    {
        var hitObjects = Physics2D.OverlapCircleAll(transform.position, shockRadius, whatToDamage);

        foreach (var hitObject in hitObjects)
        {
            var racer = hitObject.GetComponent<Racer>();

            if (struckRacers.Contains(racer)) { return; }

            if (racer.racerDamages.myDamages.IsDamaged())
            {
                foreach (var damage in racer.racerDamages.myDamages.DamageList())
                {
                    if(damage.damageInt == damageType)
                    {
                        return;
                    }
                    else { continue; }
                }
            }
            
            else
            {
                if (ownerRacer == hitObject.GetComponent<Racer>()) { return; }
                {
                    struckRacers.Add(racer);

                    var beamInGame = Instantiate(beamProjectile, transform);
                    var beamScript = beamInGame.GetComponent<BeamProjectileScript>();
                    Utils.SetBeamVariables(ownerRacer, beamScript, damageType, null, ownerRacer.awakenedData);
                    beamScript.beamType = BeamProjectileScript.BeamType.ElectricOrb;
                    beamScript.whatToHit = whatToDamage;
                    beamScript.directionToHit = hitObject.transform.position - transform.position;
                }
            }

            
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveOrb();
        LookForPlayers();
    }
    private void FixedUpdate()
    {
        if (orbTimer.isTimeUp)
        {
            var explosionGO = Resources.Load<GameObject>($"{damageType}/Explosion");
            var explosionInGame = Instantiate(explosionGO, transform.position, Quaternion.identity);
            var explosionComp = explosionInGame.GetComponent<ElementExplosionScript>();

            explosionComp.Explode(false);
            Destroy(gameObject);
        }
        else
        {
            orbTimer.UpdateTimer();
        }
    }
}
