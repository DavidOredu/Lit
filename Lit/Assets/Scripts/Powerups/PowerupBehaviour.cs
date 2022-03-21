using Mirror;
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
        ammoProbability = new Probability<int>(powerupData.ammoCurve, ammoSizes);
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
                var racer = other.gameObject.GetComponent<Racer>();
                PowerupBehaviour oldPowerupBehaviour;
                switch (other.GetComponent<Racer>().currentRacerType)
                {
                    case Racer.RacerType.Player:
                        if (other.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
                        {
                            if (powerup.canBeMany)
                            {
                                powerupAmmo = ammoProbability.ProbabilityGenerator();
                            }
                            PlayerEvents.instance.GottenPowerup();
                            var powerupImg = racer.GamePlayer.powerupButton.image;
                            oldPowerupBehaviour = racer.GamePlayer.powerupButton.powerupBehaviour;
                            powerupImg.sprite = powerup.prefab.GetComponent<SpriteRenderer>().sprite;
                            if(oldPowerupBehaviour != null)
                                Destroy(racer.GamePlayer.powerupButton.powerupBehaviour.gameObject);
                            racer.GamePlayer.powerup = powerup;
                            racer.GamePlayer.powerupButton.powerupBehaviour = this;
                        }
                        break;
                    case Racer.RacerType.Opponent:
                        if (powerup.canBeMany)
                        {
                            powerupAmmo = ammoProbability.ProbabilityGenerator();
                        }

                        PlayerEvents.instance.GottenPowerup();
                        oldPowerupBehaviour = racer.GamePlayer.enemyPowerup.powerupBehaviour;
                        if (oldPowerupBehaviour != null)
                            Destroy(racer.GamePlayer.enemyPowerup.powerupBehaviour.gameObject);
                        racer.GamePlayer.powerup = powerup;
                        racer.GamePlayer.enemyPowerup.powerupBehaviour = this;
                        break;
                    default:
                        break;
                }
                GameObject powerupM = null;

                foreach (var powerupManager in GameManager.instance.powerupManagers)
                {
                    if (powerupManager.GetComponent<PowerupController>().racer == racer)
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
                image.enabled = false;
                image.gameObject.GetComponent<Collider2D>().enabled = false;
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
