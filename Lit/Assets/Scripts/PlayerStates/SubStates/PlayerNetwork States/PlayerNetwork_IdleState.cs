using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_IdleState : PlayerNetwork_GroundedState
{
    public PlayerNetwork_IdleState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

     //   player.SetDecelerations();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

       
     //   player.SetVelocityX(player.movementVelocity);

        if (racer.CurrentVelocity.x != 0f && racer.StateMachine.AwakenedState != racer.playerAwakenedState)
        {
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
