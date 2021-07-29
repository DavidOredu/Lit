using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PowerupEvent : UnityEvent<Racer>
{

}
[Serializable]
public class Powerup
{
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public string name;

    [SerializeField]
    public float duration;

    
    public bool isActive { get; set; }
    
    
    public bool isSelected { get; set; }

    [SerializeField]
    public bool isSelectable;

    [SerializeField]
    public PowerupEvent startAction;

    [SerializeField]
    public PowerupEvent activeAction;

    [SerializeField]
    public PowerupEvent endAction;
    
    [SerializeField]
    public PowerupEvent selectedAction;

    public PowerupID powerup;
    public PowerupType powerupType;

    public PowerupActions powerupActions { get; private set; }

    public void ReassignOwner(GameObject gameObject)
    {
        powerupActions = gameObject.GetComponent<PowerupActions>();
        switch (powerup)
        {
            case PowerupID.SpeedUp:
                startAction.AddListener(powerupActions.SpeedUpStartAction);
                activeAction.AddListener(powerupActions.SpeedUpActiveAction);
                endAction.AddListener(powerupActions.SpeedUpEndAction);
                break;
            case PowerupID.Shield:
                startAction.AddListener(powerupActions.ShieldStartAction);
                endAction.AddListener(powerupActions.ShieldEndAction);
                break;
            case PowerupID.ElementField:
                startAction.AddListener(powerupActions.ElementFieldStartAction);
                activeAction.AddListener(powerupActions.ElementFieldActiveAction);
                endAction.AddListener(powerupActions.ElementFieldEndAction);
                break;
            case PowerupID.Mine:
                startAction.AddListener(powerupActions.MineStartAction);
                activeAction.AddListener(powerupActions.MineActiveAction);
                endAction.AddListener(powerupActions.MineEndAction);
                break;
            case PowerupID.Projectile:
                startAction.AddListener(powerupActions.ProjectileStartAction);
                selectedAction.AddListener(powerupActions.ProjectileSelectedAction);
                activeAction.AddListener(powerupActions.ProjectileActiveAction);
                endAction.AddListener(powerupActions.ProjectileEndAction);
                break;
            case PowerupID.Beam:
                startAction.AddListener(powerupActions.BeamStartAction);
                endAction.AddListener(powerupActions.BeamEndAction);
                break;
            default:
                break;
        }
    }
    public void Start(Racer racer)
    {
        if (startAction != null)
            startAction.Invoke(racer);
    }
    public void Active(Racer racer)
    {
        if (activeAction != null)
           if(isActive)
                activeAction.Invoke(racer);
    }
    public void Selected(Racer racer)
    {
        if (selectedAction != null)
        {
            if (isSelected)
            {
                selectedAction.Invoke(racer);
            }
        }
    }
    public void End(Racer racer)
    {
        if(endAction != null)
            endAction.Invoke(racer);
        
    }

    public enum PowerupType
    {
        offensive,
        selfAiding
    }

    public enum PowerupID
    {
        SpeedUp,
        Shield,
        ElementField,
        Mine,
        Projectile,
        Beam,
    }
}
