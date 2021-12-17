using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetBeamVariables(Racer racer, BeamProjectileScript beamScript, int damageType, PowerupData powerupData = null, AwakenedData awakenedData = null)
    {
        if (powerupData != null)
        {
            beamScript.ownerRacer = racer;
            beamScript.damageType = damageType;
            beamScript.damageStrength = powerupData.beamDamageStrength;
        }
        else if (awakenedData != null)
        {
            beamScript.ownerRacer = racer;
            beamScript.damageType = damageType;
            beamScript.damageStrength = awakenedData.purpleBeamDamageStrength;
        }
    }
    public static void SetBombVariables(Racer racer, BombScript bombScript, int damageType, PowerupData powerupData)
    {
        bombScript.OwnerRacer = racer;
        bombScript.damageType = damageType;
        bombScript.damageStrength = powerupData.bombDamageStrength;
        bombScript.explosiveForce = powerupData.bombExplosiveForce;
        bombScript.explosiveRadius = powerupData.bombExplosiveRadius;
        bombScript.upwardsModifier = powerupData.bombUpwardsModifier;
        bombScript.forceMode = powerupData.bombForceMode;
    }
    public static void SetMineVariables(Racer racer, MineScript mineComp, int damageType, PowerupData powerupData)
    {
        mineComp.ownerRacer = racer;
        mineComp.damageType = damageType;
        mineComp.damageStrength = powerupData.mineDamageStrength;
        mineComp.explosiveForce = powerupData.mineExplosiveForce;
        mineComp.explosiveRadius = powerupData.mineExplosiveRadius;
        mineComp.upwardsModifier = powerupData.mineUpwardsModifier;
        mineComp.forceMode = powerupData.mineForceMode;
        mineComp.explosiveRadiusDecreasePercentage = powerupData.mineExplosiveRadiusDecreasePercentage;
        mineComp.explosiveRadius -= PercentageValue(mineComp.explosiveRadius, mineComp.explosiveRadiusDecreasePercentage);
    }
    public static void SetExplosionVariables(Racer racer, ElementExplosionScript explosionScript, int damageType, PowerupData powerupData = null, AwakenedData awakenedData = null)
    {
        if (racer.isAwakened)
        {
            switch (damageType)
            {
                case 1:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageInt = damageType;
                    explosionScript.damagePercentage = awakenedData.redDamageStrength;
                    explosionScript.explosiveRadius = PercentageIncreaseOrDecrease(powerupData.bombExplosiveRadius, awakenedData.redExplosiveRadiusPercentageIncrease, AlterType.Increase);
                    ParticleSystemAction(explosionScript.gameObject, ParticleSystemActions.IncreaseStartSize, awakenedData.redExplosiveRadiusPercentageIncrease);
                    explosionScript.explosiveForce = powerupData.bombExplosiveForce;
                    explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
                    explosionScript.forceMode = powerupData.bombForceMode;
                    break;
                case 6:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageInt = damageType;
                    explosionScript.damagePercentage = powerupData.bombDamageStrength;
                    explosionScript.explosiveForce = PercentageIncreaseOrDecrease(powerupData.bombExplosiveForce, awakenedData.cyanExplosiveForcePercentageIncrease, AlterType.Increase);
                    explosionScript.explosiveRadius = PercentageIncreaseOrDecrease(powerupData.bombExplosiveRadius, awakenedData.cyanExplosiveRadiusPercentageDecrease, AlterType.Decrease);
                    explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
                    explosionScript.forceMode = powerupData.bombForceMode;
                    break;
                default:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageInt = damageType;
                    explosionScript.damagePercentage = powerupData.bombDamageStrength;
                    explosionScript.explosiveForce = powerupData.bombExplosiveForce;
                    explosionScript.explosiveRadius = powerupData.bombExplosiveRadius;
                    explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
                    explosionScript.forceMode = powerupData.bombForceMode;
                    break;
            }

        }
        else if (powerupData != null)
        {
            explosionScript.ownerRacer = racer;
            explosionScript.damageInt = damageType;
            explosionScript.damagePercentage = powerupData.bombDamageStrength;
            explosionScript.explosiveForce = powerupData.bombExplosiveForce;
            explosionScript.explosiveRadius = powerupData.bombExplosiveRadius;
            explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
            explosionScript.forceMode = powerupData.bombForceMode;
        }
    }
    public static void SetMineToBombVariables(ElementExplosionScript explosionComp, MineScript mineScript)
    {
        explosionComp.ownerRacer = mineScript.ownerRacer;
        explosionComp.damageInt = mineScript.damageType;
        explosionComp.damagePercentage = mineScript.damageStrength;
        explosionComp.explosiveForce = mineScript.explosiveForce;
        explosionComp.explosiveRadius = mineScript.explosiveRadius;
        explosionComp.upwardsModifier = mineScript.upwardsModifier;
        explosionComp.forceMode = mineScript.forceMode;

        ParticleSystemAction(explosionComp.gameObject, ParticleSystemActions.DecreaseStartSize, mineScript.explosiveRadiusDecreasePercentage);
    }
    public static List<ParticleSystem> GetParticlesInGameObject(GameObject particleObject)
    {
        List<ParticleSystem> particleList = new List<ParticleSystem>();

        var particle = particleObject.GetComponent<ParticleSystem>();
        particleList.Add(particle);

        var childParticles = particleObject.GetComponentsInChildren<ParticleSystem>();

        foreach (var childParticle in childParticles)
        {
            if(!particleList.Contains(childParticle))
                particleList.Add(childParticle);
        }
        Debug.Log(particleList.Count);
        return particleList;
        
    }
    /// <summary>
    /// Neccessary actions needed to be taken on particle systems during runtime.
    /// </summary>
    /// <param name="particleObject">The main object.</param>
    /// <param name="actions">The type of action to be carried out.</param>
    /// <param name="alterPercentage">by how much do you want to change something. Usually used for Increase Particle Size, Decrease Particle Size actions.</param>
    public static void ParticleSystemAction(GameObject particleObject, ParticleSystemActions actions, float alterPercentage = 0f)
    {
        var particle = particleObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule particleMain = new ParticleSystem.MainModule();
        if(particle != null)
           particleMain = particle.main;
        var childParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
        switch (actions)
        {
            case ParticleSystemActions.TurnOffLooping:
                if (particle)
                {
                    particleMain.loop = false;
                    particleMain.stopAction = ParticleSystemStopAction.Destroy;
                }
                
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;
                    childParticleMain.loop = false;
                    childParticleMain.stopAction = ParticleSystemStopAction.Destroy;
                }
                break;
            case ParticleSystemActions.IncreaseStartSize:
                if (particle)
                {
                    switch (particleMain.startSize.mode)
                    {
                        case ParticleSystemCurveMode.Constant:
                            var newSize = particleMain.startSize.constant + PercentageValue(particleMain.startSize.constant, alterPercentage);
                            particleMain.startSize = new ParticleSystem.MinMaxCurve(newSize);
                            break;
                        case ParticleSystemCurveMode.TwoConstants:
                            var newMinSize = particleMain.startSize.constantMin + PercentageValue(particleMain.startSize.constantMin, alterPercentage);
                            var newMaxSize = particleMain.startSize.constantMax + PercentageValue(particleMain.startSize.constantMax, alterPercentage);
                            particleMain.startSize = new ParticleSystem.MinMaxCurve(newMinSize, newMaxSize);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;

                    switch (particleMain.startSize.mode)
                    {
                        case ParticleSystemCurveMode.Constant:
                            var newSize = childParticleMain.startSize.constant + PercentageValue(childParticleMain.startSize.constant, alterPercentage);
                            childParticleMain.startSize = new ParticleSystem.MinMaxCurve(newSize);
                            break;
                        case ParticleSystemCurveMode.TwoConstants:
                            var newMinSize = childParticleMain.startSize.constantMin + PercentageValue(childParticleMain.startSize.constantMin, alterPercentage);
                            var newMaxSize = childParticleMain.startSize.constantMax + PercentageValue(childParticleMain.startSize.constantMax, alterPercentage);
                            childParticleMain.startSize = new ParticleSystem.MinMaxCurve(newMinSize, newMaxSize);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case ParticleSystemActions.DecreaseStartSize:
                if (particle)
                {
                    switch (particleMain.startSize.mode)
                    {
                        case ParticleSystemCurveMode.Constant:
                            var newSize = particleMain.startSize.constant - PercentageValue(particleMain.startSize.constant, alterPercentage);
                            particleMain.startSize = new ParticleSystem.MinMaxCurve(newSize);
                            break;
                        case ParticleSystemCurveMode.TwoConstants:
                            var newMinSize = particleMain.startSize.constantMin - PercentageValue(particleMain.startSize.constantMin, alterPercentage);
                            var newMaxSize = particleMain.startSize.constantMax - PercentageValue(particleMain.startSize.constantMax, alterPercentage);
                            particleMain.startSize = new ParticleSystem.MinMaxCurve(newMinSize, newMaxSize);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;

                    switch (particleMain.startSize.mode)
                    {
                        case ParticleSystemCurveMode.Constant:
                            var newSize = childParticleMain.startSize.constant - PercentageValue(childParticleMain.startSize.constant, alterPercentage);
                            childParticleMain.startSize = new ParticleSystem.MinMaxCurve(newSize);
                            break;
                        case ParticleSystemCurveMode.TwoConstants:
                            var newMinSize = childParticleMain.startSize.constantMin - PercentageValue(childParticleMain.startSize.constantMin, alterPercentage);
                            var newMaxSize = childParticleMain.startSize.constantMax - PercentageValue(childParticleMain.startSize.constantMax, alterPercentage);
                            childParticleMain.startSize = new ParticleSystem.MinMaxCurve(newMinSize, newMaxSize);
                            break;
                        default:
                            break;
                    }
                }
                break;
        }
    }
    public static float PercentageIncreaseOrDecrease(float originalValue, float percentage, AlterType alterType)
    {
        float value = 0;
        switch (alterType)
        {
            case AlterType.Increase:
                 value = originalValue + (originalValue * percentage);
                break;
            case AlterType.Decrease:
                 value =  originalValue - (originalValue * percentage);
                break;
            default:
                break;
        }
        return value;
    }
    /// <summary>
    /// Returns a fraction of a certain value depending on the percentage parced to it.
    /// </summary>
    /// <param name="value">The value where the fraction comes from.</param>
    /// <param name="percentage">The percentage of "value" needed.</param>
    /// <returns>Float: value * percentage</returns>
    public static float PercentageValue(float value, float percentage)
    {
        return value * percentage;
    }
    /// <summary>
    /// Sets the neccessary data needed for proper damaging. And sends the damage information to the damaged racer.
    /// </summary>
    /// <param name="runnerDamages">The damage manager. Where the damage comes from.</param>
    /// <param name="ownerRacer">The racer doing damage. Use 'null' if a racer didn't do the damage.</param>
    /// <param name="damageInt">The color code of the damage done. Use '8' if is laser damage.</param>
    /// <param name="damagePercentage">The percentage of damage done, relating to the damaged runner's max strength.</param>
    /// <param name="damageRate">The rate at which the damage reduces strength.</param>
    /// <param name="hitObject">The object to be damaged. This object should have the 'racer' component on it.</param>
    public static void SetDamageVariables(RunnerDamagesOperator runnerDamages,Racer ownerRacer, int damageInt, float damagePercentage, float damageRate, GameObject hitObject)
    {
        runnerDamages.Damages[damageInt].damaged = true;
        runnerDamages.Damages[damageInt].damageInt = damageInt;
        runnerDamages.Damages[damageInt].damagePercentage = damagePercentage;
        runnerDamages.Damages[damageInt].damageRate = damageRate;
        runnerDamages.Damages[damageInt].racer = ownerRacer;
        hitObject.transform.SendMessage("DamageRunner", runnerDamages);
    }
    public enum ParticleSystemActions
    {
        TurnOffLooping,
        IncreaseStartSize,
        DecreaseStartSize,
    }
    public enum AlterType
    {
        Increase,
        Decrease,
    }
}
