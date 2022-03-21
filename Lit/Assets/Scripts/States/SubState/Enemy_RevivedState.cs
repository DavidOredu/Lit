﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RevivedState : Enemy_AbilityState
{
    protected Timer revivedStateTimer;
    public Enemy_RevivedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        racerEntity.actionController.ClearDetections();
        racer.racerDamages.RemovePowerups();

        if (racer.strength <= 0)
            racer.racerDamages.RestoreStrength();

        revivedStateTimer = new Timer(difficultyData.invulnerabilityTimer);
        revivedStateTimer.SetTimer();

        racer.isInvulnerable = true;

        // Speed computation is done in this manner because the first line below gives a current percentage value of speed. In order to reduce speed, we need to use the amount of speed reduced to calculate current speed
        var newMoveVelocityNormalized = difficultyData.strengthToTopSpeedCurve.Evaluate(racer.normalizedStrength);

        newMoveVelocityNormalized = racer.previousStrengthNormalized - newMoveVelocityNormalized;
        racer.moveVelocityResource -= newMoveVelocityNormalized * difficultyData.topSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        racer.isInvulnerable = false;
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (revivedStateTimer.isTimeUp)
        {
            StateMachine.ChangeDamagedState(racer.opponentNullState);
        }
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Projectile"))
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, true);
        }
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        base.OnCollisionExit(collision);

        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Projectile"))
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, false);
        }
    }

    public override void OnCollisionStay(Collision2D collision)
    {
        base.OnCollisionStay(collision);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!revivedStateTimer.isTimeUp)
            revivedStateTimer.UpdateTimer();
    }
}
