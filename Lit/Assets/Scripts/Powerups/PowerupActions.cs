using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PowerupActions : MonoBehaviour
{
    public Racer racer;

    private PlayerData playerData;
    private D_DifficultyData opponentData;
    private PowerupData powerupData;
    int colorCode;
    Camera cam;

    private float playerMovementVelocityTemp;
    private float playerMovementVelocityResourceTemp;

    public InputManager InputManager;

    private BombScript bombScript;
    private MagicProjectileScript projectileScript;

    private RunnerDamagesOperator runnerDamages;

    private class GraphicsEffects
    {
        public GameObject effectGO;
        public string effectName;
    }

    #region Graphics Effects
    private GraphicsEffects speedBurstEffect;
    private GraphicsEffects shieldEffect;
    private GraphicsEffects elementFieldEffect;
    private GraphicsEffects mineEffect;
    private GraphicsEffects bombEffect;
    private GraphicsEffects projectileEffect;
    private GraphicsEffects beamEffect;
    #endregion
    private void Awake()
    {

    }
    private void Start()
    {
        shieldEffect = new GraphicsEffects();
        speedBurstEffect = new GraphicsEffects();
        elementFieldEffect = new GraphicsEffects();
        mineEffect = new GraphicsEffects();
        beamEffect = new GraphicsEffects();
        bombEffect = new GraphicsEffects();
        projectileEffect = new GraphicsEffects();
        runnerDamages.InitDamages();
    }
    private void OnEnable()
    {
        cam = Camera.main;
        playerData = Resources.Load<PlayerData>("PlayerData");
        InputManager = InputManager.instance;

        InputManager.OnStartTouch += StartDragging;
        InputManager.OnMoveTouch += UpdateDragging;
        ///    InputManager.OnStationaryTouch += UpdateDragging;
        InputManager.OnEndTouch += EndDragging;

        InputManager.OnStartTouch += StartAiming;
        InputManager.OnMoveTouch += UpdateAiming;
        InputManager.OnEndTouch += EndAiming;
    }
    private void OnDisable()
    {
        InputManager.OnStartTouch -= StartDragging;
        InputManager.OnMoveTouch -= UpdateDragging;
        //    InputManager.OnStationaryTouch -= UpdateDragging;
        InputManager.OnEndTouch -= EndDragging;

        InputManager.OnStartTouch -= StartAiming;
        InputManager.OnMoveTouch -= UpdateAiming;
        InputManager.OnEndTouch -= EndAiming;
    }
    private void LateUpdate()
    {

    }
    #region Shield Powerup
    public void ShieldStartAction(Racer racer)
    {
        if (racer != null)
        {
            shieldEffect.effectGO = Resources.Load<GameObject>("Shield");
            racer.isInvulnerable = true;
            var playerCenter = racer.transform.Find("PlayerCenter");
            var effectInGame = Instantiate(shieldEffect.effectGO, playerCenter.transform.position, Quaternion.identity, racer.transform);
            shieldEffect.effectName = effectInGame.name;
        }
    }
    public void ShieldActiveAction(Racer racer)
    {
        if (racer != null)
        {
            var playerCenter = racer.transform.Find("PlayerCenter");

            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var hitObjectsPlayer = Physics2D.OverlapCircleAll(playerCenter.position, playerData.powerupRadius, playerData.whatToDamage);
                    foreach (var hitRacer in hitObjectsPlayer)
                    {
                        var objectRB = hitRacer.GetComponent<Rigidbody2D>();
                        var damageType = racer.runner.stickmanNet.currentColor.colorID;
                        powerupData = Resources.Load<PowerupData>($"{damageType}/PowerupData");
                        // instantiate appropriate element effect on hit player
                        if (objectRB.CompareTag("Obstacle"))
                        {
                            var obstacle = objectRB.GetComponent<Obstacle>();
                            if(obstacle.currentObstacleType == Obstacle.ObstacleType.LaserOrb)
                            {
                                obstacle.ExplodeLaserOrb();
                            }
                        }
                        // take to appropriate paralyzed state version
                        // determine length of duration of effect or consider making a variable on racers to govern stun time
                    }
                    break;
                case Racer.RacerType.Opponent:
                    var opponentData = racer.difficultyData;
                    var hitObjectsOpponent = Physics2D.OverlapCircleAll(playerCenter.position, opponentData.powerupRadius, opponentData.whatToDamage);
                    foreach (var hitRacer in hitObjectsOpponent)
                    {

                        var objectRB = hitRacer.GetComponent<Rigidbody2D>();
                        var damageType = racer.runner.stickmanNet.currentColor.colorID;
                        powerupData = Resources.Load<PowerupData>($"{damageType}/PowerupData");
                        // instantiate appropriate element effect on hit player
                        if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
                        {
                            foreach (var damage in hitRacer.GetComponent<Racer>().myDamages.DamageList())
                            {
                                if (damage.damageInt == damageType)
                                    return;
                                else
                                    continue;
                            }
                            if (objectRB.GetComponent<StickmanNet>().currentColor.colorID != damageType)
                            {
                                objectRB.AddExplosionForce(powerupData.fieldExplosiveForce, transform.position, playerData.powerupRadius, 0f, powerupData.fieldForceMode);
                                Debug.Log($"Damage Type at point of contact is: {damageType}");
                                runnerDamages.Damages[damageType].damaged = true;
                                runnerDamages.Damages[damageType].damageInt = damageType;
                                runnerDamages.Damages[damageType].damagePercentage = powerupData.fieldDamageStrength;
                                runnerDamages.Damages[damageType].racer = racer;
                                objectRB.transform.SendMessage("DamageRunner", runnerDamages);
                            }


                            // instantiate appropriate element effect on hit player
                            // take to appropriate paralyzed state version
                            // determine length of duration of effect or consider making a variable on racers to govern stun time
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void ShieldEndAction(Racer racer)
    {
        if (racer != null)
        {
            if (racer.transform.Find(shieldEffect.effectName))
            {
                var particle = racer.transform.Find("Shield(Clone)").gameObject;
                Utils.ParticleSystemAction(particle, Utils.ParticleSystemActions.TurnOffLooping);
                racer.isInvulnerable = false;
            }
        }
    }
    #endregion

    #region SpeedUp Powerup
    public void SpeedUpStartAction(Racer racer)
    {
        if (racer != null)
        {
            var stickman = racer.GetComponent<StickmanNet>();
            speedBurstEffect.effectGO = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/SpeedBurst");
            var effectInGame = Instantiate(speedBurstEffect.effectGO, racer.transform.position, speedBurstEffect.effectGO.transform.rotation);
            speedBurstEffect.effectName = effectInGame.name;
            
            racer.RB.AddForce(new Vector2(20f, 0f), ForceMode2D.Impulse);
            racer.moveVelocityResource += Utils.PercentageValue(racer.playerData.topSpeed, racer.powerupData.speedUpPercentageIncrease);
            racer.movementVelocity = racer.moveVelocityResource;
        }
    }
    public void SpeedUpActiveAction(Racer racer)
    {
        //if (racer.isOnLit)
        //{
        //    var stickmanColorCode = racer.GetComponent<StickmanNet>().code;
        //    PowerupData powerupData = Resources.Load<PowerupData>($"{stickmanColorCode}/PowerupData");

        //    racer.movementVelocity += powerupData.speedUpPercentageIncrease;
        //    racer.moveVelocityResource += powerupData.speedUpPercentageIncrease;
        //}
        Debug.Log("Is constantly running 'SpeedUpActiveAction' function!");
    }
    public void SpeedUpEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.moveVelocityResource -= Utils.PercentageValue(racer.playerData.topSpeed, racer.powerupData.speedUpPercentageIncrease);
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

            elementFieldEffect.effectGO = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/ElementField");
            var effectInGame = Instantiate(elementFieldEffect.effectGO, racer.transform.Find("PlayerCenter").position, Quaternion.identity, racer.transform);
            elementFieldEffect.effectName = effectInGame.name;
            //effectInGame.GetComponent<ParticleSystem>().Play();
            //var objectsInChildren = effectInGame.GetComponentsInChildren<ParticleSystem>();
            //foreach (var child in objectsInChildren)
            //{
            //    child.Play();
            //}

        }
    }
    public void ElementFieldActiveAction(Racer racer)
    {
        if (racer != null)
        {
            {
                var playerCenter = racer.transform.Find("PlayerCenter");
                switch (racer.currentRacerType)
                {
                    case Racer.RacerType.Player:
                        var hitObjectsPlayer = Physics2D.OverlapCircleAll(playerCenter.position, playerData.powerupRadius, playerData.whatToDamage);
                        foreach (var hitRacer in hitObjectsPlayer)
                        {
                            var objectRB = hitRacer.GetComponent<Rigidbody2D>();
                            var damageType = racer.runner.stickmanNet.currentColor.colorID;
                            powerupData = Resources.Load<PowerupData>($"{damageType}/PowerupData");
                            // instantiate appropriate element effect on hit player
                            if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
                            {
                                foreach (var damage in hitRacer.GetComponent<Racer>().myDamages.DamageList())
                                {
                                    if (damage.damageInt == damageType)
                                        return;
                                    else
                                        continue;
                                }
                                if (objectRB.GetComponent<StickmanNet>().currentColor.colorID != damageType)
                                {
                                    objectRB.AddExplosionForce(powerupData.fieldExplosiveForce, transform.position, playerData.powerupRadius, 0f, powerupData.fieldForceMode);
                                    Debug.Log($"Damage Type at point of contact is: {damageType}");
                                    runnerDamages.Damages[damageType].damaged = true;
                                    runnerDamages.Damages[damageType].damageInt = damageType;
                                    runnerDamages.Damages[damageType].damagePercentage = powerupData.fieldDamageStrength;
                                    runnerDamages.Damages[damageType].racer = racer;
                                    objectRB.transform.SendMessage("DamageRunner", runnerDamages);
                                }

                            }
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
            }
            // run code on all hit objects
        }
    }
    public void ElementFieldEndAction(Racer racer)
    {
        if (racer != null)
        {
            if (racer.transform.Find(elementFieldEffect.effectName))
            {
                var field = racer.transform.Find("ElementField(Clone)").gameObject;
                Utils.ParticleSystemAction(field, Utils.ParticleSystemActions.TurnOffLooping);
                if (racer.GamePlayer.powerup != null)
                {
                    racer.canUsePowerup = true;
                }
            }
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
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");

            mineEffect.effectGO = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Mine");
            var effectInGame = Instantiate(mineEffect.effectGO, racer.transform.Find("PlayerCenter").position, Quaternion.identity);
            var mineComp = effectInGame.GetComponent<MineScript>();
            mineEffect.effectName = mineComp.name;

            Utils.SetMineVariables(racer, mineComp, stickman.currentColor.colorID, powerupData);

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
    public void ProjectileSelectedStartAction(Racer racer)
    {
        if (racer != null)
        {
            this.racer = racer;
            racer.Anim.SetBool("shoot", true);
        }
    }
    public void ProjectileSelectedActiveAction(Racer racer)
    {
        Debug.Log("Projectile selected function is running!");
        if (racer != null)
        {
            if (projectileScript == null) { return; }
            if (projectileScript.hasFired)
            {

                projectileScript.canControl = racer.StateMachine.AwakenedState == racer.playerAwakenedState || racer.StateMachine.AwakenedState == racer.opponentAwakenedState || GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID;
                if (projectileScript.canControl)
                {

                }
                else
                {
                    if (racer.GamePlayer.enemyPowerup == null)
                        racer.GamePlayer.powerupButton.UsePowerup(true);
                    else if (racer.GamePlayer.powerupButton == null)
                        racer.GamePlayer.enemyPowerup.UsePowerup();
                    else
                        return;
                    racer.GamePlayer.powerup.isSelected = false;
                    racer.GamePlayer.powerupButton.isSelected = false;
                }
            }

        }
    }
    public void ProjectileActiveAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    public void ProjectileSelectedEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.Anim.SetBool("shoot", false);
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
    void StartAiming(Vector2 touchPosition, float time, Finger finger)
    {
        if (racer == null) { return; }
    }
    void UpdateAiming(Vector2 touchPosition, float time, Finger finger)
    {
        #region Checks
        if (racer == null) { return; }
        if (racer.GamePlayer.powerup != null)
            if (racer.GamePlayer.powerup.powerup != Powerup.PowerupID.Projectile) { return; }
        if (!racer.GamePlayer.powerup.isSelected) { return; }
        #endregion

        if (projectileScript != null)
        {
            if (projectileScript.canControl)
            {
                AimProjectile(touchPosition);
            }
        }
        else
        {
            AimProjectile(touchPosition);
        }


    }
    void EndAiming(Vector2 touchPosition, float time, Finger finger)
    {
        #region Checks
        if (racer == null) { return; }
        if (racer.GamePlayer.powerup != null)
            if (racer.GamePlayer.powerup.powerup != Powerup.PowerupID.Projectile) { return; }
        if (!racer.GamePlayer.powerup.isSelected) { return; }
        #endregion

        Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0f;
        var mousePos = worldCoordinates;
        var offset = (mousePos - racer.projectileArm.position);
        var rotationZ = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                {
                    var distance = offset.magnitude;
                    Vector2 direction = offset / distance;
                    direction.Normalize();
                    FireProjectile(racer, direction, rotationZ);
                }

                break;
            case Racer.RacerType.Opponent:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Beam Powerup
    public void BeamStartAction(Racer racer)
    {
        if (racer != null)
        {
            var stickman = racer.GetComponent<StickmanNet>();
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");
            var firstRunner = GameManager.instance.GetRunnerAtPosition(1);
            if (firstRunner == null)
            {
                Debug.Log("first runner is null");
            }
            else
            {
                Debug.Log("first runner's name is" + firstRunner.name);
            }

            beamEffect.effectGO = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Beam");
            var effectInGame = Instantiate(beamEffect.effectGO, new Vector3(firstRunner.transform.Find("PlayerCenter").position.x, firstRunner.transform.Find("PlayerCenter").position.y + 40), Quaternion.identity);
            var beamScript = effectInGame.GetComponent<BeamProjectileScript>();
            beamEffect.effectName = effectInGame.name;

            Utils.SetBeamVariables(racer, beamScript, stickman.code, powerupData);
            
        }
    }
    public void BeamEndAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    #endregion

    #region Bomb Powerup

    public void BombStartAction(Racer racer)
    {
        if (racer != null)
        {

        }
    }
    public void BombSelectedStartAction(Racer racer)
    {
        if (racer != null)
        {
            GameObject bomb = null;
            colorCode = racer.GetComponent<StickmanNet>().currentColor.colorID;
            bombEffect.effectGO = Resources.Load<GameObject>($"{colorCode}/Bomb");
            racer.Anim.SetBool("throw", true);
            if (!GameObject.Find("Bomb(Clone)"))
            {
                bomb = Instantiate(bombEffect.effectGO, racer.bombArm.position, Quaternion.identity);
            }
            else
            {
                if (GameObject.Find("Bomb(Clone)").GetComponent<BombScript>().isDiscarded)
                    bomb = Instantiate(bombEffect.effectGO, racer.bombArm.position, Quaternion.identity);
                else
                    bomb = GameObject.Find("Bomb(Clone)");
            }


            if (bomb == null) { return; }
            powerupData = Resources.Load<PowerupData>($"{colorCode}/PowerupData");
            bombEffect.effectName = bomb.name;
            bombScript = bomb.GetComponent<BombScript>();

            Utils.SetBombVariables(racer, bombScript, colorCode, powerupData);
        }
    }
    public void BombSelectedActiveAction(Racer racer)
    {
        if (racer != null)
        {
            if (bombScript == null) { return; }
            bombScript.FollowPlayer(racer.bombArm);
            if (bombScript.hasFired)
            {

                bombScript.canControl = racer.StateMachine.AwakenedState == racer.playerAwakenedState || racer.StateMachine.AwakenedState == racer.opponentAwakenedState || GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID;
                if (bombScript.canControl)
                {

                }
                else
                {
                    if (racer.GamePlayer.enemyPowerup == null)
                    {
                        racer.GamePlayer.powerup.isSelected = false;
                        racer.GamePlayer.powerupButton.isSelected = false;
                        racer.GamePlayer.powerupButton.UsePowerup(true);
                    }
                    else if (racer.GamePlayer.powerupButton == null)
                        racer.GamePlayer.enemyPowerup.UsePowerup();
                    else
                        return;

                }
            }
        }
    }
    public void BombSelectedEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.Anim.SetBool("throw", false);

            GameObject bomb = null;
            if (GameObject.Find("Bomb(Clone)"))
                bomb = GameObject.Find("Bomb(Clone)").gameObject;
            if (bomb != null)
            {
                var bombScript = bomb.GetComponent<BombScript>();
                bombScript.canExplode = false;
                bombScript.canControl = false;
                bombScript.isDiscarded = true;
                Utils.ParticleSystemAction(bomb, Utils.ParticleSystemActions.TurnOffLooping);
            }
            else { return; }
        }
    }
    public void BombEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.Anim.SetBool("throw", false);
        }
    }
    void StartDragging(Vector2 touchPosition, float time, Finger finger)
    {
        if (bombScript == null) { return; }
        //  if (!bombScript.hasFired || bombScript.canControl)
        {
            Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
            Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
            worldCoordinates.z = 0f;
            bombScript.throwForce = powerupData.bombThrowForce;
            bombScript.startScreenCoordinates = screenCoordinates;
            bombScript.OnBombDragStart(worldCoordinates);
        }
    }
    void UpdateDragging(Vector2 touchPosition, float time, Finger finger)
    {
        if (bombScript == null) { return; }
        //  if (!bombScript.hasFired || bombScript.canControl)
        {
            Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
            Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
            worldCoordinates.z = 0f;
            bombScript.OnDrag(worldCoordinates);
        }
    }
    void EndDragging(Vector2 touchPosition, float time, Finger finger)
    {
        if (bombScript == null) { return; }
        if (!bombScript.hasFired || bombScript.canControl)
        {
            Utils.SetBombVariables(racer, bombScript, colorCode, powerupData);
            bombScript.OnBombDragEnd();
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
    private void AimProjectile(Vector2 touchPosition)
    {
        Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0f;
        var powerArm = racer.transform.Find("LeftArmProjectile");
        var mousePos = worldCoordinates;

        powerArm.transform.position = mousePos;
    }
    private void FireProjectile(Racer racer, Vector2 direction, float rotationZ)
    {
        if (projectileScript == null)
        {
            var stickman = racer.GetComponent<StickmanNet>();
            PowerupData powerupData = Resources.Load<PowerupData>($"{stickman.code}/PowerupData");
            projectileEffect.effectGO = Resources.Load<GameObject>($"{stickman.currentColor.colorID}/Projectile");
            var projectile = Instantiate(projectileEffect.effectGO);

            projectileEffect.effectName = projectile.name;
            var projectileScript = projectile.GetComponent<MagicProjectileScript>();
            this.projectileScript = projectileScript;
            projectileScript.ownerRacer = racer;
            projectileScript.hasFired = true;

            projectileScript.canControl = (racer.runner.stickmanNet.currentColor.colorID == 1) && (racer.isAwakened || GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID);
            projectile.transform.position = racer.projectileArm.position;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            projectileScript.damageInt = racer.runner.stickmanNet.code;
            projectileScript.impactNormal = new Vector3(0, 0, rotationZ);
            projectileScript.damagePercentage = powerupData.projectileDamageStrength;
            projectileScript.speed = powerupData.projectileSpeed;
            projectileScript.direction = direction;
            if (!projectileScript.canControl)
            {
                racer.GamePlayer.powerupButton.UsePowerup(false);
                racer.GamePlayer.powerupButton.UsePowerup(true);
            }
            //      else
            {

            }
        }
        else
        {
            for (int i = 0; i < InputManager.activeTouches.Count; i++)
            {
                if (i == 0)
                {
                    var Pos = InputManager.activeTouches[i].screenPosition;
                    Vector3 screenCoordinates = new Vector3(Pos.x, Pos.y, cam.nearClipPlane);
                    Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
                    worldCoordinates.z = 0f;
                    var mousePos = worldCoordinates;
                    var offset = (mousePos - projectileScript.transform.position);
                    rotationZ = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                    var distance = offset.magnitude;
                    direction = offset / distance;
                    direction.Normalize();
                    projectileScript.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                    projectileScript.impactNormal = new Vector3(0, 0, rotationZ);
                    projectileScript.direction = direction;
                }
            }
        }
    }
    
    private void CheckColliders(Racer racer)
    {

    }
    #endregion
}
