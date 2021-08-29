using UnityEngine;

public class PlayerNetwork_SlideState : PlayerNetwork_AbilityState
{
    public PlayerNetwork_SlideState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (isAnimationFinished)
        {
            StateMachine.ChangeState(racer.playerMoveState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        racer.SetAccelerations(Racer.RacerType.Player);
        racer.SetVelocityX(racer.movementVelocity);
        racer.SetVelocityY(0);
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.collider.CompareTag("Obstacle"))
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, true);
        }
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        base.OnCollisionExit(collision);
        if (collision.collider.CompareTag("Obstacle"))
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, false);
        }
    }

    public override void OnCollisionStay(Collision2D collision)
    {
        base.OnCollisionStay(collision);
        if (collision.collider.CompareTag("Obstacle"))
        {
        //    Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, true);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
