using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamagedState : State
{
    protected GameObject damageEffect;
    public Enemy_DamagedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        Utils.ParticleSystemAction(damageEffect, Utils.ParticleSystemActions.TurnOffLooping);

        // Speed computation is done in this manner because the first line below gives a current percentage value of speed. In order to reduce speed, we need to use the amount of speed reduced to calculate current speed
        
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
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
