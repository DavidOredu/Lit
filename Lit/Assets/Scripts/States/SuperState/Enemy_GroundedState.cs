using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : State
{
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isGrounded;
    protected bool canJump;
    protected bool jump;

    protected Probability<bool> canJumpToLitIfIsLit;
    List<bool> bools = new List<bool> { true, false };

    public Enemy_GroundedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        //isDetectingLedge = entity.CheckLedge();
        //isDetectingWall = entity.CheckWall();
        isGrounded = racerEntity.CheckIfGrounded();
        canJump = racerEntity.CheckIfCanJump().Length > 0;
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

        #region Jump Logic
        //if (canJump && !racerEntity.isOnPower && isGrounded)
        //{
        //    // check current speed
        //    if (racer.movementVelocity >= .8 * racer.playerData.topSpeed)
        //        jump = true;
        //    // check if litplatform is lit ||  check if opponent color is "black"
        //    jump = canJumpToLitIfIsLit.ProbabilityGenerator() || racer.runner.stickmanNet.currentColor.colorID == 0;

        //    // check if is already on lit and if so, check if the lit being found is not the one he's on
        //    foreach (var platform in racerEntity.CheckIfCanJump())
        //    {
        //        if (platform == racerEntity.currentPlatform)
        //        {
        //            jump = false;
        //            continue;
        //        }
        //        else
        //        {
        //            jump = true;
        //            break;
        //        }
        //    }
        //    // adjust with difficulty

        //    if(jump)
        //    StateMachine.ChangeState(racer.opponentJumpState);
            
        //}
        #endregion

        if (!isGrounded)
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
