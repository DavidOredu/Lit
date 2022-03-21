﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_DamageKnockDownState : PlayerNetwork_KnockedDownState
{
    private Timer knockoutTimer;
    public PlayerNetwork_DamageKnockDownState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        knockoutTimer = new Timer(playerData.knockoutTime);
        knockoutTimer.SetTimer();
    }

    public override void Exit()
    {
        base.Exit();

        racer.racerDamages.Recover();
        racer.moveVelocityResource = playerData.topSpeed;
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (knockoutTimer.isTimeUp)
        {
            StateMachine.ChangeDamagedState(racer.playerRevivedState);
            StateMachine.ChangeState(racer.playerMoveState);
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

        if (!knockoutTimer.isTimeUp)
            knockoutTimer.UpdateTimer();
    }
}
