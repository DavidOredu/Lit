﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public Racer racer;

    public List<Opponent> opponents = new List<Opponent>();

    public List<Powerup> powerups = new List<Powerup>();



    public Dictionary<Powerup, float> activePowerups = new Dictionary<Powerup, float>();

    private List<Powerup> keys = new List<Powerup>();
    public List<Powerup> selectedPowerups = new List<Powerup>();
    private void Awake()
    {
        SetRunnerPowerupManager();
        SetupPowerupList();
    }
    private void Update()
    {
        HandleActivePowerups();
        SetRunnerPowerupManager();
       
    }
    private void SetupPowerupList()
    {
        foreach (var powerup in powerups)
        {
            var originalPowerup = powerup.prefab.GetComponent<PowerupBehaviour>();
            powerup.name = originalPowerup.powerup.name;
            powerup.isSelectable = originalPowerup.powerup.isSelectable;
            powerup.duration = originalPowerup.powerup.duration;
            powerup.canBeMany = originalPowerup.powerup.canBeMany;
            powerup.powerup = originalPowerup.powerup.powerup;
            powerup.powerupType = originalPowerup.powerup.powerupType;
        }
    }
    private void SetRunnerPowerupManager()
    {
        if(racer != null)
        {
            racer.powerupManager = gameObject;
        }
        else
        {
            return;
        }
    }
    public void HandleActivePowerups()
    {
        bool changed = false;

        if(activePowerups.Count > 0)
        {
            foreach(Powerup powerup in keys)
            {
                if(activePowerups[powerup] > 0)
                {
                    powerup.Active(racer);
                    activePowerups[powerup] -= Time.deltaTime;
                }
                else
                {
                    changed = true;

                    activePowerups.Remove(powerup);
                    powerup.End(racer);
                    powerup.isActive = false;
                }
            }
        }

        if (changed)
        {
            keys = new List<Powerup>(activePowerups.Keys);
        }

        if(selectedPowerups.Count > 0)
        {
            foreach (var powerup in selectedPowerups)
            {
                powerup.SelectedActive(racer);
            }
        }
    }

    public void ActivatePowerup(Powerup powerup)
    {
        if (!activePowerups.ContainsKey(powerup))
        {
            powerup.Start(racer);
            powerup.isActive = true;
            powerup.Active(racer);
            activePowerups.Add(powerup, powerup.duration);
        }
        else
        {
            powerup.Start(racer);
            activePowerups[powerup] += powerup.duration;
        }

        keys = new List<Powerup>(activePowerups.Keys);
    }
}
