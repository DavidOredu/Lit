using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageKnockDownState : Enemy_KnockedDownState
{
    private Timer knockoutTimer;
    public Enemy_DamageKnockDownState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        knockoutTimer = new Timer(difficultyData.knockoutTime);
        knockoutTimer.SetTimer();
    }

    public override void Exit()
    {
        base.Exit();

        racer.Recover();
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
            StateMachine.ChangeDamagedState(racer.opponentRevivedState);
            StateMachine.ChangeState(racer.opponentMoveState);
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
