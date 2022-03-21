using UnityEngine;

public class PoweredPlatform : MonoBehaviour
{
    public enum Power
    {
        RollUnder,
        SuperJump,
        Stop,
        JumpBooster,
        SpeedBooster,
        Action,
        Null,
    }
    /// <summary>
    /// Determines if the power platform is good for defense or self-aid
    /// </summary>
    public enum PowerPlatformAidType
    {
        /// <summary>
        /// This type only defends the platform against incoming or potential danger and not meant to elevate the player directly
        /// </summary>
        Defensive,
        /// <summary>
        /// This type boosts a certain player stat in response to input or contact.
        /// </summary>
        Booster,
    }
    public Platform.PlatformType platformType = Platform.PlatformType.PowerPlatform;
    public Power currentPower;
    /// <summary>
    /// Determines the usage nature of the power platform.
    /// </summary>
    public PowerPlatformAidType currentPowerAidType;

    [Header("SUPER JUMP AND JUMP BOOSTER POWER")]
    public float jumpForce;
    [Header("SPEED BOOSTER POWER")]
    public float speedBoost = 50f;


    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public void DefinePower(Racer racer)
    {
        switch (currentPower)
        {
            case Power.RollUnder:
                RollUnder(racer);
                break;
            case Power.SuperJump:
                SuperJump(racer);
                break;
            case Power.Stop:
                Stop(racer);
                break;
            case Power.JumpBooster:
                JumpBooster(racer);
                break;
            case Power.SpeedBooster:
                SpeedBooster(racer);
                break;
            case Power.Action:
                Action(racer);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    #region Power Functions
    public void RollUnder(Racer racer)
    {
        if (racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.StateMachine.ChangeState(racer.playerSlideState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.StateMachine.ChangeState(racer.opponentSlideState);
                    break;
                default:
                    break;
            }

        }
    }
    public void SuperJump(Racer racer)
    {
        if (racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.jumpVelocity = jumpForce;
                    racer.playerJumpState.poweredJump = true;
                    racer.StateMachine.ChangeState(racer.playerJumpState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.jumpVelocity = jumpForce;
              //      racer.opponentJumpState.poweredJump = true;
                    racer.StateMachine.ChangeState(racer.opponentJumpState);
                    break;
                default:
                    break;
            }
        }
    }
    public void JumpBooster(Racer racer)
    {
        if (racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.jumpVelocity = jumpForce;
                    racer.StateMachine.ChangeState(racer.playerJumpState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.jumpVelocity = jumpForce;
                    racer.StateMachine.ChangeState(racer.opponentJumpState);
                    break;
                default:
                    break;
            }
        }
    }
    public void SpeedBooster(Racer racer)
    {
        if (racer != null)
        {
            if ((racer.StateMachine.CurrentState == racer.playerLandState || racer.StateMachine.CurrentState == racer.opponentLandState) || (racer.StateMachine.CurrentState == racer.playerMoveState || racer.StateMachine.CurrentState == racer.opponentMoveState))
            {
                switch (racer.currentRacerType)
                {

                    case Racer.RacerType.Player:
                        racer.RB.AddForce(new Vector2(speedBoost, 0f), ForceMode2D.Impulse);
                        break;
                    case Racer.RacerType.Opponent:
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void Stop(Racer racer)
    {
        if(racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.StateMachine.ChangeState(racer.playerFullStopState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.StateMachine.ChangeState(racer.opponentFullStopState);
                    break;
                default:
                    break;
            }
        }
    }
    public void Action(Racer racer)
    {
        if(racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.StateMachine.ChangeState(racer.playerActionState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.StateMachine.ChangeState(racer.opponentActionState);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Opponent"))
        {
            Racer racer = other.GetComponent<Racer>();
            racer.PowerTriggerEnter(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Opponent"))
        {
            Racer racer = other.GetComponent<Racer>();
            racer.PowerTriggerExit();
        }
    }
}
