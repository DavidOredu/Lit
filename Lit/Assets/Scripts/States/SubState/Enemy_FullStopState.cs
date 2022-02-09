﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FullStopState : Enemy_AbilityState
{
    public Enemy_FullStopState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Check if the player should stop for an obstacle, depending on the player's current difficulty and therefore intelligence
        // tell the player to change state if the obstacle is 

        if(!racerEntity.canUsePowerPlatform)
            racer.StateMachine.ChangeState(racer.opponentMoveState);
        else
        {
            racer.SetStop();
            racer.SetVelocityX(racer.movementVelocity);
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
}
