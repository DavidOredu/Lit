using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
/// <summary>
/// Contains logic for all powerup properties.
/// </summary>
public class PowerupActions : MonoBehaviour
{
    public Racer racer;

    int colorCode;
    Camera cam;

    public InputManager InputManager;

    private BombScript bombScript;
    private MagicProjectileScript projectileScript;

    private RunnerDamagesOperator runnerDamages;

    [System.Serializable]
    /// <summary>
    /// Class to hold related effect names to their gameobjects.
    /// </summary>
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
    private void Update()
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
            racer.moveVelocityResource += Utils.PercentageValue(racer.racerData.topSpeed, racer.powerupData.speedUpPercentageIncrease);
            racer.movementVelocity = racer.moveVelocityResource;

            Debug.Log("Has run 'SpeedUpStartAction' function!");
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
        //  Debug.Log("Is constantly running 'SpeedUpActiveAction' function!");
    }
    public void SpeedUpEndAction(Racer racer)
    {
        if (racer != null)
        {
            racer.moveVelocityResource -= Utils.PercentageValue(racer.racerData.topSpeed, racer.powerupData.speedUpPercentageIncrease);
            Debug.Log("Has ended speedup powerup!");
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
            var effectInGame = Instantiate(elementFieldEffect.effectGO, racer.playerCenter.position, Quaternion.identity, racer.transform);
            elementFieldEffect.effectName = effectInGame.name;
            Debug.Log("Has instantiated element field effect.");
        }
    }
    public void ElementFieldActiveAction(Racer racer)
    {
        if (racer != null)
        {
            {
                var racerData = racer.racerData;
                var hitObjects = Physics2D.OverlapCircleAll(racer.playerCenter.position, racerData.powerupRadius, racerData.whatToDamage);

                foreach (var hitRacer in hitObjects)
                {
                    var objectRB = hitRacer.GetComponent<Rigidbody2D>();
                    var damageType = racer.runner.stickmanNet.currentColor.colorID;

                    // instantiate appropriate element effect on hit player
                    if (objectRB.CompareTag("Opponent") || objectRB.CompareTag("Player"))
                    {
                        objectRB.AddExplosionForce(racer.powerupData.fieldExplosiveForce, transform.position, racerData.powerupRadius, 0f, racer.powerupData.fieldForceMode);
                        Utils.SetDamageVariables(runnerDamages, racer, damageType, racer.powerupData.fieldDamagePercentage, racer.powerupData.fieldDamageRate, hitRacer.gameObject); ;

                    }
                }

            }
            // run code on all hit objects
        }
    }
    public void ElementFieldEndAction(Racer racer)
    {
        if (racer != null)
        {
            var effect = racer.transform.Find(elementFieldEffect.effectName);
            if (effect)
            {
                var field = effect.gameObject;
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
                    //if (racer.GamePlayer.enemyPowerup == null)
                    //    racer.GamePlayer.powerupButton.UsePowerup(true);
                    //else if (racer.GamePlayer.powerupButton == null)
                    //    racer.GamePlayer.enemyPowerup.UsePowerup();
                    //else
                    //    return;
                    //racer.GamePlayer.powerup.isSelected = false;
                    //racer.GamePlayer.powerupButton.isSelected = false;
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
        if (racer.GamePlayer.powerup == null) { return; }
        if (racer.GamePlayer.powerup.powerupID != Powerup.PowerupID.Projectile) { return; }
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
        if (racer.GamePlayer.powerup == null) { return; }
        if (racer.GamePlayer.powerup.powerupID != Powerup.PowerupID.Projectile) { return; }
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
            this.racer = racer;

            GameObject bombInScene = GameObject.Find("Bomb(Clone)");
            if (!bombInScene)
            {
                bomb = Instantiate(bombEffect.effectGO, racer.bombArm.position, Quaternion.identity);
            }
            else
            {
                BombScript bombInSceneScript = bombInScene.GetComponent<BombScript>();
                if (bombInSceneScript.canControl) { return; }
                if (bombInSceneScript.isDiscarded || bombInSceneScript.hasFired)
                    bomb = Instantiate(bombEffect.effectGO, racer.bombArm.position, Quaternion.identity);
                else
                    bomb = bombInScene;
            }

            if (bomb == null) { return; }
            bombEffect.effectName = bomb.name;
            bombScript = bomb.GetComponent<BombScript>();

            Utils.SetBombVariables(racer, bombScript, colorCode, racer.powerupData);
        }
    }
    public void BombSelectedActiveAction(Racer racer)
    {
        if (racer != null)
        {
            if (bombScript == null) { return; }
            bombScript.FollowPlayer(racer.bombArm);
            // if (bombScript.hasFired)
            {

                bombScript.canControl = racer.StateMachine.AwakenedState == racer.playerAwakenedState || racer.StateMachine.AwakenedState == racer.opponentAwakenedState || GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID;
                if (bombScript.canControl)
                {

                }
                else
                {
                    //if (racer.GamePlayer.enemyPowerup == null)
                    //{
                    //    racer.GamePlayer.powerupButton.TurnSelectableState(false);
                    //    racer.GamePlayer.powerupButton.UsePowerup(true);
                    //}
                    //else if (racer.GamePlayer.powerupButton == null)
                    //    racer.GamePlayer.enemyPowerup.UsePowerup();
                    //else
                    //    return;

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
                if (bombScript.hasFired) { return; }
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
        #region Checks
        if (racer == null) { return; }
        if (racer.GamePlayer.powerup == null) { return; }
        if (racer.GamePlayer.powerup.powerupID != Powerup.PowerupID.Bomb) { return; }
        if (!racer.GamePlayer.powerup.isSelected) { return; }
        #endregion

        if (bombScript == null) { return; }
        if (!bombScript.hasFired || bombScript.canControl)
        {
            Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
            Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
            worldCoordinates.z = 0f;
            bombScript.throwForce = racer.powerupData.bombThrowForce;
            bombScript.startScreenCoordinates = screenCoordinates;
            bombScript.OnBombDragStart(worldCoordinates);
        }
    }
    void UpdateDragging(Vector2 touchPosition, float time, Finger finger)
    {
        #region Checks
        if (racer == null) { return; }
        if (racer.GamePlayer.powerup == null) { return; }
        if (racer.GamePlayer.powerup.powerupID != Powerup.PowerupID.Bomb) { return; }
        if (!racer.GamePlayer.powerup.isSelected) { return; }
        #endregion

        if (bombScript == null) { return; }
        if (!bombScript.hasFired || bombScript.canControl)
        {
            Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane);
            Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
            worldCoordinates.z = 0f;
            bombScript.OnDrag(worldCoordinates);
        }
    }
    void EndDragging(Vector2 touchPosition, float time, Finger finger)
    {
        #region Checks
        if (racer == null) { return; }
        if (racer.GamePlayer.powerup == null) { return; }
        if (racer.GamePlayer.powerup.powerupID != Powerup.PowerupID.Bomb) { return; }
        if (!racer.GamePlayer.powerup.isSelected) { return; }
        #endregion

        if (bombScript == null) { return; }
        if (!bombScript.hasFired || bombScript.canControl)
        {
            Utils.SetBombVariables(racer, bombScript, colorCode, racer.powerupData);
            bombScript.OnBombDragEnd();
        }

        if (!bombScript.canControl)
        {
            racer.GamePlayer.powerupButton.TurnSelectableState(false);
            racer.GamePlayer.powerupButton.UsePowerup(true);
            bombScript = null;
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
            projectileScript.damagePercentage = powerupData.projectileDamagePercentage;
            projectileScript.speed = powerupData.projectileSpeed;
            projectileScript.direction = direction;
            if (!projectileScript.canControl)
            {
                racer.GamePlayer.powerupButton.TurnSelectableState(false);
                racer.GamePlayer.powerupButton.UsePowerup(true);
                this.projectileScript = null;
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
