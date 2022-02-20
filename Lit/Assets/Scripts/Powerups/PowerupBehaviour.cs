﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls powerup in-game interaction and activation.
/// </summary>
public class PowerupBehaviour : MonoBehaviour
{
    public SpriteRenderer image { get; private set; }
    PowerupData powerupData;
    PowerupInformation powerupInformation;
    public PowerupController powerupController;

    public Probability<int> ammoProbability;
    List<int> ammoSizes = new List<int> { 1, 2, 3, 4, 5 };
    
    public Powerup powerup;

    public int powerupAmmo = 0;

    bool isTaken;
    private void Awake()
    {
        powerupData = Resources.Load<PowerupData>("PowerupData");
        image = GetComponent<SpriteRenderer>();
        ammoProbability = new Probability<int>(powerupData.ammoCurve);
        ammoProbability.InitDictionary(ammoSizes);
        isTaken = false;
    }
    private void Update()
    {
        if (powerup.isSelected && powerup.isSelectable && !powerupController.selectedPowerups.Contains(powerup))
            powerupController.selectedPowerups.Add(powerup);
        else if(!powerup.isSelected && powerup.isSelectable && powerupController.selectedPowerups.Contains(powerup))
            powerupController.selectedPowerups.Remove(powerup);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTaken)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Opponent"))
            {
                isTaken = true;
                switch (other.GetComponent<Racer>().currentRacerType)
                {
                    case Racer.RacerType.Player:
                        if (other.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
                        {
                            var player = other.gameObject.GetComponent<Player>();

                            if (powerup.canBeMany)
                            {
                                powerupAmmo = ammoProbability.ProbabilityGenerator();
                            }
                            PlayerEvents.instance.GottenPowerup();
                            var powerupImg = player.GamePlayer.powerupButton.image;
                            var oldPowerupBehaviour = player.GamePlayer.powerupButton.powerupBehaviour;
                            powerupImg.sprite = powerup.prefab.GetComponent<SpriteRenderer>().sprite;
                            if(oldPowerupBehaviour != null)
                                Destroy(player.GamePlayer.powerupButton.powerupBehaviour.gameObject);
                            player.GamePlayer.powerup = powerup;
                            player.GamePlayer.powerupButton.powerupBehaviour = this;

                            GameObject powerupMM = null;

                            foreach (var powerupManager in GameManager.instance.powerupManagers)
                            {
                                if (powerupManager.GetComponent<PowerupController>().racer == player)
                                {
                                    powerupMM = powerupManager;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            powerupController = powerupMM.GetComponent<PowerupController>();
                         //   if (player != powerup.PowerupActions.racer)
                            powerup.ReassignOwner(powerupMM);
                            // Call this function insteaad when we click gthe powerup button
                            //  ActivatePowerup();
                            image.enabled = false;
                            image.gameObject.GetComponent<Collider2D>().enabled = false;
                        }
                        break;
                    case Racer.RacerType.Opponent:
                        var opponent = other.gameObject.GetComponent<Opponent>();

                        PlayerEvents.instance.GottenPowerup();
                        opponent.GamePlayer.enemyPowerup.powerupBehaviour = this;
                        opponent.GamePlayer.powerup = powerup;
                        GameObject powerupM = null;

                        foreach (var powerupManager in GameManager.instance.powerupManagers)
                        {
                            if (powerupManager.GetComponent<PowerupController>().racer == opponent)
                            {
                                powerupM = powerupManager;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        powerupController = powerupM.GetComponent<PowerupController>();
                        powerup.ReassignOwner(powerupM);
                        // Call this function insteaad when we click the powerup button
                        //  ActivatePowerup();
                        image.enabled = false;
                        image.gameObject.GetComponent<Collider2D>().enabled = false;
                        break;
                    default:
                        break;
                }


            }

        }
    }

    public void ActivatePowerup()
    {
        powerupInformation = new PowerupInformation(powerup, powerup.duration);
        powerupController.ActivatePowerup(powerupInformation);
    }


    public void SetPowerup(Powerup powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }
}
