using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_DamagedState : PlayerNetwork_AbilityState
{
    protected GameObject damageEffect;
    public PlayerNetwork_DamagedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        racer.Anim.SetBool("damaged", true);
        base.Enter();
    }

    public override void Exit()
    {
        racer.Anim.SetBool("damaged", false);
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //slow down runner to a slow stop
        racer.SetBrakes();
        racer.SetVelocityX(racer.movementVelocity);
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
