using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the powerup mechanic; keeps track of active powerups, their duration and reduces their duration with time.
/// </summary>
public class PowerupController : MonoBehaviour
{
    public Racer racer;

    public List<Opponent> opponents = new List<Opponent>();

    public List<Powerup> powerups = new List<Powerup>();



    //  public Dictionary<PowerupInformation, float> activePowerups = new Dictionary<PowerupInformation, float>();
    public List<PowerupInformation> activePowerups = new List<PowerupInformation>();
    private Dictionary<PowerupInformation, Powerup> keys;
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
            var originalPowerupBehaviour = powerup.prefab.GetComponent<PowerupBehaviour>();
            powerup.name = originalPowerupBehaviour.powerup.name;
            powerup.isSelectable = originalPowerupBehaviour.powerup.isSelectable;
            powerup.duration = originalPowerupBehaviour.powerup.duration;
            powerup.canBeMany = originalPowerupBehaviour.powerup.canBeMany;
            powerup.powerupID = originalPowerupBehaviour.powerup.powerupID;
            powerup.powerupType = originalPowerupBehaviour.powerup.powerupType;
            powerup.powerupActiveType = originalPowerupBehaviour.powerup.powerupActiveType;
        }
    }
    private void SetRunnerPowerupManager()
    {
        if (racer != null)
        {
            racer.powerupManager = gameObject;
        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// Regulates the powerup timer for each active powerup.
    /// </summary>
    public void HandleActivePowerups()
    {
        bool changed = false;
        // run the 'for' loop if one or more powerups exist
        if (activePowerups.Count > 0)
        {
            foreach (var pair in keys)
            {
                var index = this.activePowerups.IndexOf(pair.Key);
                if (this.activePowerups[index].duration > 0)
                {
                    pair.Key.powerup.Active(racer);
                    this.activePowerups[index].duration -= Time.deltaTime;
                }
                else
                {
                    changed = true;

                    pair.Key.powerup.End(racer);
                    pair.Key.powerup.isActive = false;
                    this.activePowerups.Remove(pair.Key);
                }
            }
        }

        if (changed)
        {
            keys = new Dictionary<PowerupInformation, Powerup>();
            for (int i = 0; i < activePowerups.Count; i++)
            {
                keys.Add(activePowerups[i], activePowerups[i].powerup);
            }
        }

        if (selectedPowerups.Count > 0)
        {
            foreach (var powerup in selectedPowerups)
            {
                powerup.SelectedActive(racer);
            }
        }
    }

    public void ActivatePowerup(PowerupInformation powerupInformation)
    {
        // if the powerup doesn't previously exist, add it and activate
        Debug.Log("Activate powerup called!");
        if (GetPowerupInformationByPowerupType(powerupInformation.powerup) == null)
        {
            powerupInformation.powerup.Start(racer);
            powerupInformation.powerup.isActive = true;
            activePowerups.Add(powerupInformation);
        }
        else
        {
            switch (powerupInformation.powerup.powerupActiveType)
            {
                case Powerup.PowerupActiveType.Additive:
                    var samePowerupInformation = GetPowerupInformationByPowerupType(powerupInformation.powerup);
                    var index = activePowerups.IndexOf(samePowerupInformation);
                    samePowerupInformation.powerup.Active(racer);
                    activePowerups[index].duration = samePowerupInformation.powerup.duration;
                    break;
                case Powerup.PowerupActiveType.Instance:
                    powerupInformation.powerup.Start(racer);
                    powerupInformation.powerup.isActive = true;
                    activePowerups.Add(powerupInformation);
                    break;
                default:
                    break;
            }

        }

        keys = new Dictionary<PowerupInformation, Powerup>();
        for (int i = 0; i < activePowerups.Count; i++)
        {
            keys.Add(activePowerups[i], activePowerups[i].powerup);
        }
        Debug.Log("Activate powerup ended!");
    }
    private PowerupInformation GetPowerupInformationByPowerupType(Powerup powerup)
    {
        foreach (var powerInformation in activePowerups)
        {
            if (powerInformation.powerup.powerupID == powerup.powerupID)
                return powerInformation;
        }
        return null;
    }
    public void RemovePowerup(PowerupInformation powerup)
    {
        keys.Remove(powerup);
        powerup.powerup.End(racer);
        powerup.powerup.isActive = false;
        activePowerups.Remove(powerup);

        keys = new Dictionary<PowerupInformation, Powerup>();
        for (int i = 0; i < activePowerups.Count; i++)
        {
            keys.Add(activePowerups[i], activePowerups[i].powerup);
        }
    }
    public void TurnDurationTo0(PowerupInformation powerupInformation)
    {
        powerupInformation.duration = 0;
    }
}
