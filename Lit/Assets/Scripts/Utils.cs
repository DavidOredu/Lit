using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
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
    public static void SetExplosionVariables(Racer racer, ElementExplosionScript explosionScript, int damageType, PowerupData powerupData = null, AwakenedData awakenedData = null)
    {
        if(awakenedData != null && powerupData != null)
        {
            switch (damageType)
            {
                case 1:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageType = damageType;
                    explosionScript.damageStrength = awakenedData.redDamageStrength;
                    explosionScript.explosiveRadius = PercentageIncreaseOrDecrease(powerupData.bombExplosiveRadius, awakenedData.redExplosiveRadiusPercentageIncrease, AlterType.Increase);
                    Utils.ParticleSystemAction(explosionScript.gameObject, Utils.ParticleSystemActions.IncreaseStartSize, awakenedData.redExplosiveRadiusPercentageIncrease);
                    explosionScript.explosiveForce = powerupData.bombExplosiveForce;
                    explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
                    explosionScript.forceMode = powerupData.bombForceMode;
                    break;
                case 6:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageType = damageType;
                    explosionScript.damageStrength = powerupData.bombDamageStrength;
                    explosionScript.explosiveForce = PercentageIncreaseOrDecrease(powerupData.bombExplosiveForce, awakenedData.cyanExplosiveForcePercentageIncrease, AlterType.Increase);
                    explosionScript.explosiveRadius = PercentageIncreaseOrDecrease(powerupData.bombExplosiveRadius, awakenedData.cyanExplosiveRadiusPercentageDecrease, AlterType.Decrease);
                    explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
                    explosionScript.forceMode = powerupData.bombForceMode;
                    break;
                default:
                    explosionScript.ownerRacer = racer;
                    explosionScript.damageType = damageType;
                    explosionScript.damageStrength = powerupData.bombDamageStrength;
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
            explosionScript.damageType = damageType;
            explosionScript.damageStrength = powerupData.bombDamageStrength;
            explosionScript.explosiveForce = powerupData.bombExplosiveForce;
            explosionScript.explosiveRadius = powerupData.bombExplosiveRadius;
            explosionScript.upwardsModifier = powerupData.bombUpwardsModifier;
            explosionScript.forceMode = powerupData.bombForceMode;
        }
    }
    public static void ParticleSystemAction(GameObject particleObject, ParticleSystemActions actions, float increasePercentage = 0f)
    {
        var particle = particleObject.GetComponent<ParticleSystem>();
        var particleMain = particle.main;
        var startSize = particle.main.startSize;
        var childParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
        switch (actions)
        {
            case ParticleSystemActions.TurnOffLooping:
                particleMain.loop = false;
                particleMain.stopAction = ParticleSystemStopAction.Destroy;
                
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;
                    childParticleMain.loop = false;
                    childParticleMain.stopAction = ParticleSystemStopAction.Destroy;
                }
                break;
            case ParticleSystemActions.IncreaseStartSize:
                startSize.constant =6;
            //    startSize.constantMax = PercentageIncreaseOrDecrease(startSize.constantMax, increasePercentage, AlterType.Increase);
             //   particleMain.stopAction = ParticleSystemStopAction.Disable;
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;
                    var childStartSize = childParticleMain.startSize;
                    childStartSize.constantMin = PercentageIncreaseOrDecrease(childStartSize.constantMin, increasePercentage, AlterType.Increase);
                    childStartSize.constantMax = PercentageIncreaseOrDecrease(childStartSize.constantMax, increasePercentage, AlterType.Increase);
                }
                
                
                break;
            case ParticleSystemActions.DecreaseStartSize:
            startSize.constant = PercentageIncreaseOrDecrease(startSize.constant, increasePercentage, AlterType.Decrease);
                //    startSize.constantMax = PercentageIncreaseOrDecrease(startSize.constantMax, increasePercentage, AlterType.Increase);
                //   particleMain.stopAction = ParticleSystemStopAction.Disable;
                foreach (var childParticle in childParticles)
                {
                    var childParticleMain = childParticle.main;
                    var childStartSize = childParticleMain.startSize;
                    childStartSize.constantMin = PercentageIncreaseOrDecrease(childStartSize.constantMin, increasePercentage, AlterType.Decrease);
                    childStartSize.constantMax = PercentageIncreaseOrDecrease(childStartSize.constantMax, increasePercentage, AlterType.Decrease);
                }
                break;
            default:
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
    public static float PercentageValue(float value, float percentage)
    {
        return value * percentage;
    }
    public static void SetDamageVariables(RunnerDamagesOperator runnerDamages,Racer ownerRacer, int damageType, float damageStrength, GameObject hitObject)
    {
        runnerDamages.Damages[damageType].damaged = true;
        runnerDamages.Damages[damageType].damageInt = damageType;
        runnerDamages.Damages[damageType].damageStrength = damageStrength;
        runnerDamages.Damages[damageType].racer = ownerRacer;
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
