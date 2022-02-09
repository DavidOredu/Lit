using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StunState: Enemy_AbilityState
{
    protected D_StunState stateData;

    protected bool isStunTimeOver;
    
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    public Enemy_StunState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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





    //public StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(etity, stateMachine, animBoolName)
    //{
    //    this.stateData = stateData;
    //}

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = racerEntity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = racerEntity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        
        isStunTimeOver = false;
        isMovementStopped = false;
     //   entity.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);

    }

    public override void Exit()
    {
        base.Exit();

        
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (isAnimationFinished)
            StateMachine.ChangeState(racer.opponentMoveState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if(Time.time >= startTime + stateData.stunTime)
        //{
        //    isStunTimeOver = true;
        //}

        //if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        //{
        //    isMovementStopped = true;
        //    entity.SetVelocity(0f);

        //}
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
