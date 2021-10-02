﻿using System;
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

    public bool canBeMany;
    public bool isSelectable;

    public PowerupID powerup;
    public PowerupType powerupType;

    public bool isActive { get; set; }
    public bool isSelected { get; set; }

    [SerializeField]
    public PowerupEvent startAction;

    [SerializeField]
    public PowerupEvent activeAction;

    [SerializeField]
    public PowerupEvent endAction;

    [SerializeField]
    public PowerupEvent selectedStartAction;
    [SerializeField]
    public PowerupEvent selectedActiveAction;
    [SerializeField]
    public PowerupEvent selectedEndAction;



    public PowerupActions PowerupActions { get; private set; }


    public void ReassignOwner(GameObject gameObject)
    {
        PowerupActions = gameObject.GetComponent<PowerupActions>();
        switch (powerup)
        {
            case PowerupID.SpeedUp:
                startAction.AddListener(PowerupActions.SpeedUpStartAction);
                activeAction.AddListener(PowerupActions.SpeedUpActiveAction);
                endAction.AddListener(PowerupActions.SpeedUpEndAction);
                break;
            case PowerupID.Shield:
                startAction.AddListener(PowerupActions.ShieldStartAction);
                activeAction.AddListener(PowerupActions.ShieldActiveAction);
                endAction.AddListener(PowerupActions.ShieldEndAction);
                break;
            case PowerupID.ElementField:
                startAction.AddListener(PowerupActions.ElementFieldStartAction);
                activeAction.AddListener(PowerupActions.ElementFieldActiveAction);
                endAction.AddListener(PowerupActions.ElementFieldEndAction);
                break;
            case PowerupID.Mine:
                startAction.AddListener(PowerupActions.MineStartAction);
                activeAction.AddListener(PowerupActions.MineActiveAction);
                endAction.AddListener(PowerupActions.MineEndAction);
                break;
            case PowerupID.Projectile:
                startAction.AddListener(PowerupActions.ProjectileStartAction);
                activeAction.AddListener(PowerupActions.ProjectileActiveAction);
                endAction.AddListener(PowerupActions.ProjectileEndAction);
                selectedStartAction.AddListener(PowerupActions.ProjectileSelectedStartAction);
                selectedActiveAction.AddListener(PowerupActions.ProjectileSelectedActiveAction);
                selectedEndAction.AddListener(PowerupActions.ProjectileSelectedEndAction);
                break;
            case PowerupID.Beam:
                startAction.AddListener(PowerupActions.BeamStartAction);
                endAction.AddListener(PowerupActions.BeamEndAction);
                break;
            case PowerupID.Bomb:
                startAction.AddListener(PowerupActions.BombStartAction);
                endAction.AddListener(PowerupActions.BombEndAction);
                selectedStartAction.AddListener(PowerupActions.BombSelectedStartAction);
                selectedActiveAction.AddListener(PowerupActions.BombSelectedActiveAction);
                selectedEndAction.AddListener(PowerupActions.BombSelectedEndAction);
                break;
            default:
                break;
        }
    }
    public void Start(Racer racer)
    {
            startAction?.Invoke(racer);
    }
    public void Active(Racer racer)
    {
            if (isActive)
                activeAction?.Invoke(racer);
    }
    public void End(Racer racer)
    {
            endAction?.Invoke(racer);
    }
    public void SelectedStart(Racer racer)
    {
            if (isSelected)
                selectedStartAction?.Invoke(racer);
    }
    public void SelectedActive(Racer racer)
    {
            if (isSelected)
                selectedActiveAction?.Invoke(racer);
    }
    public void SelectedEnd(Racer racer)
    {
            if (!isSelected)
                selectedEndAction?.Invoke(racer);
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
        Bomb,
    }
}
