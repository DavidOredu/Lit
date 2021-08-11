using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LandState: Enemy_GroundedState
{
    public Enemy_LandState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
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

        racer.jumpVelocity = racer.jumpVelocityResource;
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (isGrounded)
        {
            StateMachine.ChangeState(racer.opponentMoveState);
        }
        else if (isAnimationFinished)
        {
            StateMachine.ChangeState(racer.opponentIdleState);
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
