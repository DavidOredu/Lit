using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RacerAwakening : MonoBehaviour
{
    public Racer racer;

    public int awakenCount;
    public bool canAwaken { get; set; }


    public RunnerEffects runnerEffects;
    public VFXConnector VFXConnector;
    // Start is called before the first frame update
    void Start()
    {
        awakenCount = racer.racerData.awakenCount;

        var runnerEffectComp = transform.Find("RunnerEffects").GetComponent<VisualEffect>();
        runnerEffects = new RunnerEffects(runnerEffectComp);

        runnerEffects.runnerVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAwakenStateRequirements();

        if (Input.GetKeyDown(KeyCode.P))
        {
            VFXConnector.ChangeVFXProperties(racer.runner.stickmanNet.code);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            runnerEffects.runnerVFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            runnerEffects.runnerVFX.Stop();
        }
    }

    #region Awaken Functions
    /// <summary>
    /// Puts the runner in an awakened state, where the runner is allowed to use his base power more freely.
    /// </summary>
    /// <param name="colorCode">The color signature of the runner.</param>
    public void Awaken(int colorCode)
    {
        // if the runner is previously damaged, remove the damage
        if (racer.racerDamages.myDamages.IsDamaged())
        {
            racer.racerDamages.Recover();
        }

        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                racer.StateMachine.ChangeAwakenedState(racer.playerAwakenedState);
                break;
            case Racer.RacerType.Opponent:
                racer.StateMachine.ChangeAwakenedState(racer.opponentAwakenedState);
                break;
        }
        awakenCount--;
        canAwaken = false;
    }
    public void ActivateAwakenEffects()
    {
        // vfx open
        VFXConnector.ChangeVFXProperties(racer.runner.stickmanNet.code);
        runnerEffects.runnerVFX.Play();

        //instantiate damage prefab
        var damageEffectPrefab = Resources.Load<GameObject>($"{racer.runner.stickmanNet.currentColor.colorID}/DamageEffect");
        var damageEffect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity, transform);

        //instantiate explosion effect
        var explosionEffectPrefab = Resources.Load<GameObject>($"{racer.runner.stickmanNet.currentColor.colorID}/Explosion");
        var explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity, transform);

        switch (racer.runner.stickmanNet.currentColor.colorID)
        {
            case 1:

                break;
            default:
                break;
        }
        var explosionScript = explosionEffect.GetComponent<ElementExplosionScript>();


        // assign the variables
        Utils.SetExplosionVariables(racer, explosionScript, racer.runner.stickmanNet.currentColor.colorID, racer.powerupData, racer.awakenedData);

        // initialize and do damage
        explosionScript.runnerDamages.InitDamages();
        explosionScript.Explode(true);
    }
    public void SetAbilityActive()
    {
        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                racer.playerAwakenedState.canUseAbility = true;
                break;
            case Racer.RacerType.Opponent:
                racer.opponentAwakenedState.canUseAbility = true;
                break;
            default:
                break;
        }
    }

    // the players abilities are kept here but these are called in update
    public void AwakenedGimmick(int colorCode)
    {
        switch (colorCode)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            default:
                break;
        }
    }

    // the players abilities are here but these are called once, when the players enters the awakened state
    public void AwakenedAbility(int colorCode)
    {
        switch (colorCode)
        {
            case 0:
                break;
            case 1:
                var speedBurstGO = Resources.Load<GameObject>($"{colorCode}/SpeedBurst");
                var speedBurstInGame = Instantiate(speedBurstGO, transform.position, speedBurstGO.transform.rotation);

                racer.RB.AddForce(new Vector2(20f, 0f), ForceMode2D.Impulse);
                racer.moveVelocityResource += Utils.PercentageValue(racer.racerData.topSpeed, racer.awakenedData.redSpeedIncreasePercentage);
                racer.movementVelocity = racer.moveVelocityResource;
                break;
            case 2:
                switch (racer.currentRacerType)
                {
                    case Racer.RacerType.Player:
                        racer.StateMachine.ChangeAwakenedState(racer.playerSlideGlideState);
                        break;
                    case Racer.RacerType.Opponent:
                        racer.StateMachine.ChangeAwakenedState(racer.opponentSlideGlideState);
                        break;
                    default:
                        break;
                }
                var iceBoardGO = Resources.Load<GameObject>($"{colorCode}/IceBoard");
                GameObject iceBoardInGame = null;
                //      if(!transform.Find("IceBoard(Clone)"))
                iceBoardInGame = Instantiate(iceBoardGO, racer.GroundCheck.position, Quaternion.identity, transform);

                racer.moveVelocityResource += Utils.PercentageValue(racer.racerData.topSpeed, racer.awakenedData.blueSpeedIncreasePercentage);

                break;
            case 3:
                break;
            case 4:
                var trailGO = Resources.Load<GameObject>("TrailEffect");
                var trailEffectGO = Instantiate(trailGO, transform.position, Quaternion.identity);
                var trailEffect = trailEffectGO.GetComponent<TrailRenderer>();

                Transform oldPosition = transform;
                transform.position = new Vector3(transform.position.x + Utils.PercentageValue(transform.position.x, racer.awakenedData.yellowTeleportationPositionXPercentage), transform.position.y, transform.position.z);
                Transform newPosition = transform;



                //  trailEffect.AddPosition(oldPosition.position);
                trailEffect.AddPosition(newPosition.position);
                break;
            case 5:
                var orbGO = Resources.Load<GameObject>($"{colorCode}/Orb");
                var orbInGame = Instantiate(orbGO, transform.position, Quaternion.identity, transform);
                var orbScript = orbInGame.GetComponent<OrbController>();

                orbScript.orbLifetime = racer.awakenedData.purpleOrbLifeTime;
                orbScript.ownerRacer = racer;
                orbScript.orbSpeed = racer.awakenedData.purpleOrbSpeed;
                orbScript.shockRadius = racer.awakenedData.purpleOrbRadius;
                orbScript.damageType = colorCode;
                orbScript.whatToDamage = racer.awakenedData.purpleWhatToHit;
                orbScript.beamProjectile = Resources.Load<GameObject>($"{colorCode}/Beam");

                // spawn electric orb
                // set its variables

                // give it active time
                break;
            case 6:
                var effect = Resources.Load<GameObject>($"{racer.runner.stickmanNet.currentColor.colorID}/ElementField");
                var effectInGame = Instantiate(effect, transform.Find("PlayerCenter").position, Quaternion.identity, transform);
                break;
            case 7:
                break;
            default:
                break;
        }
    }
    public virtual void CheckAwakenStateRequirements()
    {
        if (racer.litPlatforms.Count >= racer.racerData.requiredLitPlatformsToAwaken && awakenCount > 0)
        {
            canAwaken = true;
        }

    }
    #endregion
}
