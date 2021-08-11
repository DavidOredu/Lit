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
        Action
    }

    public Power currentPower;

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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
        {
            Racer racer = other.GetComponent<Racer>();
            racer.PowerTriggerEnter(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
        {
            Racer racer = other.GetComponent<Racer>();
            racer.PowerTriggerExit(this);
        }
    }
}
