using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerDamages : MonoBehaviour, IEffectable
{
    public Racer racer;
    public RunnerDamagesOperator myDamages;

    public float damageResistance;
    public float blastRadiusMultiplier;

    public bool contactDamage;
    public bool canIncreaseStrength;
    // Start is called before the first frame update
    void Start()
    {
        damageResistance = racer.racerData.damageResistance;
        blastRadiusMultiplier = racer.racerData.blastRadiusMultiplier;

        myDamages.InitDamages();
    }
    private void FixedUpdate()
    {
        CheckIncreaseStrength();
    }
    // Update is called once per frame
    void Update()
    {
        if (myDamages.IsDamaged())
        {
            foreach (var damage in myDamages.DamageList())
            {
                Debug.Log("Damage type is: " + damage.damageName);
            }
        }

        if (myDamages.activeDamageCount == 1)
            DamageEffects();

        DoContactDamage();
    }

    #region Damage Functions
    /// <summary>
    /// The function that gets called when damage occurs. The runner damages operator struct holds the neccessary information to deal appropriate damage.
    /// </summary>
    /// <param name="runnerDamages">A struct that holds all neccessary damage information.</param>
    public virtual void DamageRunner(RunnerDamagesOperator runnerDamages)
    {
        var newDamageList = runnerDamages.DamageList();

        // completely ignore damaging if is invulnerable or damage type is identical to our color
        if (racer.isInvulnerable || newDamageList[0].damageInt == racer.runner.stickmanNet.currentColor.colorID) { return; }

        foreach (var damage in myDamages.DamageList())
        {
            if (damage.damageInt == newDamageList[0].damageInt) { return; }
        }

        // if we have been damaged, remove current powerup ability or disable a selected powerup.
        RemovePowerups();

        //check if the runner is currently damaged, and if so do damage again, this time instantly depleting the runner's stamina and knocking him out
        if (myDamages.IsDamaged())
        {
            foreach (var damage in newDamageList)
            {
                DealDoubleDamage(damage);
            }

            Debug.Log("Do Dynamic Damage has run!");
        }
        // if is not previously damaged, this is new damage
        else
        {
            foreach (var damage in newDamageList)
            {
                // change to the appropirate damage state
                ChangeToDamageState(runnerDamages, damage, racer.damageStates[racer.currentRacerType][runnerDamages.Damages.IndexOf(damage)]);
            }
        }
    }
    /// <summary>
    /// Recovers the runner from damage effects, replenishes strength and puts them in a revived state.
    /// </summary>
    public virtual void Recover()
    {
        RestoreStrength();
        RemoveDamage();
    }
    /// <summary>
    /// Replenishes runner's strength.
    /// </summary>
    public virtual void RestoreStrength()
    {
        racer.strength = racer.racerData.maxStrength;
    }
    public virtual void CheckIfStrengthIsDepleted(float finalStrength)
    {
        if (racer.strength <= 0)
        {
            racer.strength = Mathf.Max(racer.strength, 0);
        }
        else
            racer.strength = Mathf.Max(racer.strength, finalStrength);
    }
    public virtual void HandleDamageFinished(float finalStrength, bool wasAwakened)
    {
        racer.normalizedStrength = racer.strength / racer.racerData.maxStrength;

        if (racer.strength <= 0)
        {
            racer.movementVelocity = 0;
            RemoveDamage();
            StrengthBreak();
        }
        else if (racer.strength <= finalStrength || wasAwakened)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.StateMachine.ChangeDamagedState(racer.playerRevivedState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.StateMachine.ChangeDamagedState(racer.opponentRevivedState);
                    break;
                default:
                    break;
            }
            Invoke("SetCanIncreaseStrength", 3f);
            RemoveDamage();
        }
    }
    /// <summary>
    /// Function responsible for dealing the appropriate effects on the strength of a racer.
    /// </summary>
    /// <returns></returns>

    public virtual void DamageEffects()
    {
        float finalStrength = 0;

        foreach (var damage in myDamages.DamageList())
        {
            if (racer.isAwakened)
            {
                var total = Utils.PercentageValue(racer.racerData.awakenTime, damage.damagePercentage * (1 - damageResistance));

                racer.racerAwakening.awakenTimer.ReduceTime(total);

                CheckIfStrengthIsDepleted(finalStrength);
                HandleDamageFinished(finalStrength, true); 
            }
            else if (racer.strength > finalStrength)
            {
                float totalDamage;
                if(damage.canResistDamage)
                    totalDamage = Utils.PercentageValue(racer.racerData.maxStrength, damage.damagePercentage * (1 - damageResistance));
                else
                    totalDamage = Utils.PercentageValue(racer.racerData.maxStrength, damage.damagePercentage);

                finalStrength = racer.recentStrength - totalDamage;
                var rate = Utils.PercentageValue(totalDamage, damage.damageRate);

                racer.strength -= rate * (Time.deltaTime /* / 5*/ );
                Debug.Log($"Rate of Damage on {name} is: {rate}");

                CheckIfStrengthIsDepleted(finalStrength);
                HandleDamageFinished(finalStrength, false);
            }
        }

       
    }
    public virtual void ChangeToDamageState(RunnerDamagesOperator runnerDamages, DamageForm damage, State stateToChangeTo)
    {
        myDamages.AddDamage(damage.damageInt, damage);

        racer.recentStrength = racer.strength;
        racer.previousStrengthNormalized = racer.strength / racer.racerData.maxStrength;
        racer.StateMachine.ChangeDamagedState(stateToChangeTo);
    }
    /// <summary>
    /// Resets damage data in the "myDamages" struct.
    /// </summary>
    public virtual void RemoveDamage()
    {
        if (myDamages.IsDamaged())
        {
            myDamages.RemoveAllDamages();
            racer.recentStrength = racer.strength;
        }
    }
    public virtual void DealDoubleDamage(DamageForm damage)
    {
        // play animation or mm feedback to show that the strength guage is broken and runner has lost conciousness. Maybe add a time modifier feedback
        // then take the play to a damage knockdown state

        var damageLeft = myDamages.DamageList()[0].damagePercentage - (racer.previousStrengthNormalized - racer.normalizedStrength);
        var totalDamagePercentage = damage.damagePercentage + damageLeft;
        Debug.Log($"Total Double Damage Percentage: {totalDamagePercentage}");

        myDamages.AddDamage(damage.damageInt, damage);

        var totalDamage = Utils.PercentageValue(racer.racerData.maxStrength, totalDamagePercentage);
        Debug.Log($"Total Double Damage: {totalDamage}");
        racer.strength -= totalDamage;
        racer.strength = Mathf.Round(racer.strength);

        CheckIfStrengthIsDepleted(racer.strength);
        HandleDamageFinished(racer.strength, false);
    }
    public virtual void StrengthBreak()
    {
        racer.strength = 0f;
        // play guage break animation
        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                if(racer.StateMachine.DamagedState != racer.playerDeadState)
                    racer.StateMachine.ChangeState(racer.playerDamageKnockDownState);
                break;
            case Racer.RacerType.Opponent:
                if (racer.StateMachine.DamagedState != racer.opponentDeadState)
                    racer.StateMachine.ChangeState(racer.opponentDamageKnockDownState);
                break;
            default:
                break;
        }
    }
    public void CheckIncreaseStrength()
    {
        if (canIncreaseStrength)
        {
            if(!myDamages.IsDamaged() && !(racer.normalizedStrength >= 1))
            {
                var rate = Utils.PercentageValue(racer.racerData.maxStrength, racer.racerData.strengthIncreaseRate);

                if (!(racer.strength > racer.racerData.maxStrength))
                {
                    racer.strength += rate * (Time.deltaTime /* / 5*/ );
                    racer.strength = Mathf.Min(racer.strength, racer.racerData.maxStrength);
                }
            }
            else if(racer.normalizedStrength >= 1)
            {
                canIncreaseStrength = false;
            }
        }
    }
    public void SetCanIncreaseStrength() => canIncreaseStrength = true;
    /// <summary>
    /// Removes current powerup ability or disables a selected powerup.
    /// </summary>
    public void RemovePowerups()
    {
        // if we have been damaged, remove current powerup ability or disable a selected powerup.
        for (int i = 0; i < racer.powerupController.activePowerups.Count; i++)
        {
            switch (racer.powerupController.activePowerups[i].powerup.powerupID)
            {
                case Powerup.PowerupID.SpeedUp:
                    racer.powerupController.TurnDurationTo0(racer.powerupController.activePowerups[i]);
                    continue;
                case Powerup.PowerupID.ElementField:
                    racer.powerupController.TurnDurationTo0(racer.powerupController.activePowerups[i]);
                    racer.powerupController.TurnDurationTo0(racer.powerupController.activePowerups[i]);
                    continue;
                case Powerup.PowerupID.Projectile:
                    // TODO: change to disable powerup selected nature
                    continue;
                case Powerup.PowerupID.Bomb:
                    // TODO: change to disable powerup selected nature
                    continue;
            }
        }
    }
    #endregion

    public void DoContactDamage()
    {
        if(!contactDamage) { return; }

        var racerData = racer.racerData;
        var hitObjects = Physics2D.OverlapCircleAll(racer.playerCenter.position, racerData.powerupSelfRadius, racerData.whatToDamage);

        RunnerDamagesOperator runnerDamages = new RunnerDamagesOperator();
        runnerDamages.InitDamages();

        foreach (var hitRacer in hitObjects)
        {
            var objectRB = hitRacer.GetComponent<Rigidbody2D>();
            var damageType = racer.runner.stickmanNet.currentColor.colorID;

            // instantiate appropriate element effect on hit player
            if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
            {
                objectRB.AddExplosionForce(racer.powerupData.fieldExplosiveForce, transform.position, racerData.powerupSelfRadius, 0f, racer.powerupData.fieldForceMode);
                Utils.SetDamageVariables(runnerDamages, racer, damageType, racer.powerupData.fieldDamagePercentage, racer.powerupData.fieldDamageRate, hitRacer.gameObject);
            }
        }
    }
    public void ApplyPerk()
    {
        throw new System.NotImplementedException();
    }

    public void HandleEffect()
    {
        throw new System.NotImplementedException();
    }

    public void RemovePerk()
    {
        throw new System.NotImplementedException();
    }
}
