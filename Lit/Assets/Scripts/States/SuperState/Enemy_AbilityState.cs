using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AbilityState : State
{
    protected bool isGrounded;
    protected bool isAbilityDone;

    public Enemy_AbilityState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = racerEntity.CheckIfGrounded();
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
            if (isGrounded && racerEntity.CurrentVelocity.y < 0.01f)
            {
                StateMachine.ChangeState(racer.opponentIdleState);
            }
            else
            {
                StateMachine.ChangeState(racer.opponentInAirState);
            }
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
