using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_AbilityState : State
{
    
    protected bool isAbilityDone;
    protected bool isComboTimeOver;
    protected bool isGrounded;
    protected bool attackInput;
    protected int XInput;
    protected bool firstAttack;

    public PlayerNetwork_AbilityState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        isGrounded = racer.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        

        if (isAbilityDone)
        {
            if (isGrounded && racer.CurrentVelocity.y < 0.01f)
            {
                StateMachine.ChangeState(racer.playerIdleState);
            }
            else
            {
                StateMachine.ChangeState(racer.playerInAirState);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        attackInput = racer.inputHandler.AttackInput;
        XInput = racer.inputHandler.NormalizedInputX;

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }


    
}
