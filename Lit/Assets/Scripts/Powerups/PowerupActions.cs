using UnityEngine;


public class PowerupActions : MonoBehaviour
{
    private PlayerData playerData;
    private D_DifficultyData opponentData;

    private float playerMovementVelocityTemp;
    private float playerMovementVelocityResourceTemp;

   

    private void Start()
    {
        playerData = Resources.Load<PlayerData>("PlayerData");
    }
    private void LateUpdate()
    {
        
    }
    #region Shield Powerup
    public void ShieldStartAction(Racer racer)
    {
        if (racer != null)
        {
            var stickmanColorCode = racer.GetComponent<StickmanNet>().code;
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickmanColorCode}/PowerupData");

            var Shield = Resources.Load<GameObject>("Shield");
            var shieldParticle = Shield.GetComponent<ParticleSystem>(); 
            racer.isInvulnerable = true;
            var playerCenter = racer.transform.Find("PlayerCenter");
            Instantiate(Shield, playerCenter.transform.position, Quaternion.identity, racer.transform);
            var main = shieldParticle.main;
            main.duration = powerupData.shieldParticleDuration;
            main.startLifetime = powerupData.shieldStartLifetime;
            var main2 = shieldParticle.GetComponentInChildren<ParticleSystem>().main;
            main2.duration = powerupData.shieldParticleDuration;
            main2.startLifetime = powerupData.shieldStartLifetime;
        }



    }

    public void ShieldEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.isInvulnerable = false;
        }
    }
    #endregion

    #region SpeedUp Powerup
    public void SpeedUpStartAction(Racer racer)
    {
        if (racer != null)
        {
            var stickman = racer.GetComponent<StickmanNet>();
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");
            var effect = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/SpeedBurst");
            var effectInGame = Instantiate(effect, racer.transform.position, Quaternion.identity, racer.transform);
            effectInGame.GetComponent<ParticleSystem>().Play();
            var objectsInChildren = effectInGame.GetComponentsInChildren<ParticleSystem>();
            foreach (var child in objectsInChildren)
            {
                child.Play();
            }
            if (racer.isOnLit)
            {

            }
            
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var player = racer.gameObject.GetComponent<Player>();
                    playerMovementVelocityTemp = player.playerData.topSpeed;
                    playerMovementVelocityResourceTemp = player.playerData.topSpeed;
                    break;
                case Racer.RacerType.Opponent:
                    var opponent = racer.gameObject.GetComponent<Opponent>();
                    playerMovementVelocityTemp = opponent.difficultyData.topSpeed;
                    playerMovementVelocityResourceTemp = opponent.difficultyData.topSpeed;
                    break;
                default:
                    break;
            }
            racer.movementVelocity += powerupData.speedUpValue;
            racer.moveVelocityResource += powerupData.speedUpValue;
        }
    }
    public void SpeedUpActiveAction(Racer racer)
    {
        if (racer.isOnLit || racer.canSpeedUp)
        {
            var stickmanColorCode = racer.GetComponent<StickmanNet>().code;
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickmanColorCode}/PowerupData");

            racer.movementVelocity += powerupData.speedUpValue;
            racer.moveVelocityResource += powerupData.speedUpValue;
        }
        Debug.Log("Is constantly running 'SpeedUpActiveAction' function!");
    }
    public void SpeedUpEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.movementVelocity = playerMovementVelocityTemp;
            racer.moveVelocityResource = playerMovementVelocityResourceTemp;
        }
    }
    #endregion

    #region Element Field Powerup
    public void ElementFieldStartAction(Racer racer)
    {
        if (racer != null)
        {
            //instantiate collider field
            var stickman = racer.GetComponent<StickmanNet>();

            var effect = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/ElementField");
            var effectInGame = Instantiate(effect, racer.transform.Find("PlayerCenter").position, Quaternion.identity, racer.transform);
            effectInGame.GetComponent<ParticleSystem>().Play();
            var objectsInChildren = effectInGame.GetComponentsInChildren<ParticleSystem>();
            foreach (var child in objectsInChildren)
            {
                child.Play();
            }
        }
    }
    public void ElementFieldActiveAction(Racer racer)
    {
        if(racer != null)
        {
            var playerCenter = racer.transform.Find("PlayerCenter");
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var hitObjectsPlayer = Physics2D.OverlapCircleAll(playerCenter.position, playerData.powerupRadius, playerData.whatToDamage);
                    foreach (var hitRacer in hitObjectsPlayer)
                    {
                        // instantiate appropriate element effect on hit player
                        // take to appropriate paralyzed state version
                        // determine length of duration of effect or consider making a variable on racers to govern stun time
                    }
                    break;
                case Racer.RacerType.Opponent:
                    var opponentData = racer.difficultyData;
                    var hitObjectsOpponent = Physics2D.OverlapCircleAll(playerCenter.position, opponentData.powerupRadius, opponentData.whatToDamage);
                    foreach (var hitRacer in hitObjectsOpponent)
                    {
                        // instantiate appropriate element effect on hit player
                        // take to appropriate paralyzed state version
                        // determine length of duration of effect or consider making a variable on racers to govern stun time
                    }
                    break;
                default:
                    break;
            }
            
            // run code on all hit objects
        }
    }
    public void ElementFieldEndAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }

    #endregion

    #region Mine Powerup
    public void MineStartAction(Racer racer)
    {
        if (racer != null)
        {
            //instantiate mine
            var stickman = racer.GetComponent<StickmanNet>();

            var effect = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Mine");
            var effectInGame = Instantiate(effect, racer.transform.Find("PlayerCenter").position, Quaternion.identity);
            
            var objectsInChildren = effectInGame.GetComponentsInChildren<ParticleSystem>();
            foreach (var child in objectsInChildren)
            {
                child.Play();
            }
        }
    }
    public void MineActiveAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    public void MineEndAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    #endregion

    #region Projectile Powerup
    public void ProjectileStartAction(Racer racer)
    {
        if (racer != null)
        {
            //GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
            //GetComponent<MagicProjectileScript>().impactNormal = hit.normal;
        }
    }
    public void ProjectileSelectedAction(Racer racer)
    {
        Debug.Log("Projectile selected function is running!");
        if (racer != null)
        {
            //       var armSolver = racer.transform.Find("LeftArmSolver");
            //        armSolver.gameObject.SetActive(true);
            racer.Anim.SetBool("shoot", true);
            var powerArm = racer.transform.Find("LeftArmProjectile");
     //       var playerCenter = racer.transform.Find("PlayerCenter");
            var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
  //          Debug.Log(mousePos);
                  var offset = (mousePos - racer.projectileArm.position);
  //          Debug.Log(offset + "is the offset value");
                  var rotationZ = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            powerArm.transform.position = mousePos;
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var player = racer.GetComponent<Player>();
                    if (player.inputHandler.AttackInput)
                    {
                        player.inputHandler.UseAttackInput();
                        
                        var distance = offset.magnitude;
                        Vector2 direction = offset / distance;
                        direction.Normalize();
                        FireProjectile(racer, direction, rotationZ);
                        player.powerupButton.UsePowerup(true);
                    }
                    break;
                case Racer.RacerType.Opponent:
                    break;
                default:
                    break;
            }
        }
    }
    public void ProjectileActiveAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    public void ProjectileEndAction(Racer racer)
    {
        if (racer != null)
        {
      //      racer.transform.Find("LeftArmSolver").gameObject.SetActive(false);
            racer.Anim.SetBool("shoot", false);
        }
    }
    #endregion

    #region Beam Powerup
    public void BeamStartAction(Racer racer)
    {
        if(racer != null)
        {
            var stickman = racer.GetComponent<StickmanNet>();
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");
            var firstRunner = GameManager.instance.GetRunnerAtPosition(1);
            if(firstRunner == null)
            {
                Debug.Log("first runner is null");
            }
            else
            {
                Debug.Log("first runner's name is" + firstRunner.name);
            }

            var effect = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Beam");
            var effectInGame = Instantiate(effect, new Vector3(firstRunner.transform.Find("PlayerCenter").position.x, firstRunner.transform.Find("PlayerCenter").position.y + 40), Quaternion.identity);
            var beamScript = effectInGame.GetComponent<BeamProjectileScript>();

            beamScript.damageTime = powerupData.beamDamageTime;
        }
    }
    public void BeamEndAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    #endregion

    #region Coins
    public void CoinStartAction()
    {

    }

    public void CoinEndAction()
    {

    }
    #endregion


    #region Other Functions
    private void FireProjectile(Racer racer, Vector2 direction, float rotationZ)
    {
        var stickman = racer.GetComponent<StickmanNet>();
        PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");
        var projectilePrefab = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Projectile");
        var projectile = Instantiate(projectilePrefab);
        
        var projectileScript = projectile.GetComponent<MagicProjectileScript>();
        projectile.transform.position = racer.projectileArm.position;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        projectileScript.damageType = racer.runner.stickmanNet.code;
        projectileScript.impactNormal = new Vector3(0, 0, rotationZ);
        projectileScript.damageTime = powerupData.projectileDamageTime;
        projectileScript.speed = powerupData.projectileSpeed;
        projectileScript.direction = direction;
        
    }
    #endregion
}
