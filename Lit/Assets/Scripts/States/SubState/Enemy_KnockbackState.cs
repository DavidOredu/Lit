using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KnockbackState : Enemy_AbilityState
{
    public Enemy_KnockbackState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        racerEntity.movementVelocity = 0f;


        racerEntity.SetVelocityX(difficultyData.knockbackVelocity.x * -racerEntity.FacingDirection);
        racerEntity.SetVelocityY(difficultyData.knockbackVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        //racerEntity.jumpVelocity = 0f;
        if (isAnimationFinished)
        {
            difficultyData.knockbackVelocity = Vector2.zero;
            StateMachine.ChangeState(racer.opponentMoveState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
}
