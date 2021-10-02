using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupBehaviour : MonoBehaviour
{
    PowerupData powerupData;
    public PowerupController powerupController;
    SpriteRenderer image;

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
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
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
                            player.GamePlayer.powerupButton.powerupBehaviour = this;
                            var powerupImg = player.powerupSlot.GetComponent<Image>();
                            powerupImg.sprite = powerup.prefab.GetComponent<SpriteRenderer>().sprite;
                            player.GamePlayer.powerup = powerup;

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
                            powerup.ReassignOwner(powerupMM);
                            // Call this function insteaad when we click gthe powerup button
                            //  ActivatePowerup();
                            image.enabled = false;
                        }
                        break;
                    case Racer.RacerType.Opponent:
                        var opponent = other.gameObject.GetComponent<Opponent>();

                        PlayerEvents.instance.GottenPowerup();
                        opponent.enemyPowerup.powerupBehaviour = this;
                        opponent.GamePlayer.enemyPowerup.powerupBehaviour = this;
                        opponent.GamePlayer.powerup = powerup;
                        opponent.powerup = powerup;
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
                        break;
                    default:
                        break;
                }


            }

        }
    }

    public void ActivatePowerup()
    {
        powerupController.ActivatePowerup(powerup);
    }


    public void SetPowerup(Powerup powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }
}
