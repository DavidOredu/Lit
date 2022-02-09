using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_KnockbackState : PlayerNetwork_AbilityState
{
    public PlayerNetwork_KnockbackState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        racer.movementVelocity = 0f;
        
        
        racer.SetVelocityX(playerData.knockbackVelocity.x * -racer.FacingDirection);
        racer.SetVelocityY(playerData.knockbackVelocity.y);

      //  player.RB.AddForce(new Vector2(playerData.knockbackVelocity.x, playerData.knockbackVelocity.y));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        
        //racer.jumpVelocity = 0f;
        if (isAnimationFinished)
        {
            playerData.knockbackVelocity = Vector2.zero;
            StateMachine.ChangeState(racer.playerMoveState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

       
    }
}
