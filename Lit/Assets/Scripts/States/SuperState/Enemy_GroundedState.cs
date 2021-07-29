using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : State
{
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isGrounded;
    protected bool canJump;

    public Enemy_GroundedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        //isDetectingLedge = entity.CheckLedge();
        //isDetectingWall = entity.CheckWall();
        isGrounded = racerEntity.CheckIfGrounded();
        canJump = racerEntity.CheckIfCanJump();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (canJump && !racerEntity.isOnPower && isGrounded)
        {
            StateMachine.ChangeState(racer.opponentJumpState);
        }
        else if (!isGrounded)
        {
            StateMachine.ChangeState(racer.opponentInAirState);
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
