using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState: Enemy_GroundedState
{
    public Enemy_IdleState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        racerEntity.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (racer.CurrentVelocity.x != 0f && racer.StateMachine.AwakenedState != racer.opponentAwakenedState)
        {
            StateMachine.ChangeState(racer.opponentMoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
