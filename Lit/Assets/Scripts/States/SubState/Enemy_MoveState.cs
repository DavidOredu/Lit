using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState: Enemy_GroundedState
{
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    public Enemy_MoveState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //isDetectingLedge = entity.CheckLedge();
        //isDetectingWall = entity.CheckWall();
        isPlayerInMinAgroRange = racerEntity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = racerEntity.CheckPlayerInMaxAgroRange();
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

        if (!isGrounded)
        {
            StateMachine.ChangeState(racer.opponentInAirState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded)
        {
            if (racer.moveVelocityResource > racer.movementVelocity)
            {
                racer.SetAccelerations(racer.moveVelocityResource);
            }
            else if (racer.moveVelocityResource < racer.movementVelocity)
            {
                racer.SetDecelerations(racer.moveVelocityResource);
            }

            if (racer.isOnSlope)
            {
                racer.SetVelocityX(racer.movementVelocity * racer.slopeNormalPerpendicular.x * -1);
                racer.SetVelocityY(racer.movementVelocity * racer.slopeNormalPerpendicular.y * -1);
            }
            else
            {
                racer.SetVelocityX(racer.movementVelocity);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

       
    }
}
