using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerNetwork_ReadyingState : PlayerNetwork_AbilityState
{
    public PlayerNetwork_ReadyingState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        InputManager.instance.OnStartTouch += CheckRacerStart;     
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.OnStartTouch -= CheckRacerStart;
        racer.InputHandler.RegisterInputs();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (racer.canRace)
        {
            if (racer.perfectLaunch)
            {
                racer.movementVelocity = racer.moveVelocityResource;
            }

            racer.StateMachine.ChangeState(racer.playerMoveState);
        }
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        base.OnCollisionExit(collision);
    }

    public override void OnCollisionStay(Collision2D collision)
    {
        base.OnCollisionStay(collision);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void CheckRacerStart(Vector2 position, float time, Finger finger = null)
    {
        if (0.1 <= GameManager.instance.raceCountdownTimer.CurrentTime() && GameManager.instance.raceCountdownTimer.CurrentTime() < 0.7)
        {
            racer.perfectLaunch = true;
            racer.canRace = true;
        }
        else if(GameManager.instance.raceCountdownTimer.isTimeUp)
        {
            racer.canRace = true;
        }
    }
}
