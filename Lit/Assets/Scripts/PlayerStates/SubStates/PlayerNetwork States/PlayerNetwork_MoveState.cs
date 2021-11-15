using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_MoveState : PlayerNetwork_GroundedState
{
    public PlayerNetwork_MoveState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
           
       if(!isGrounded)
        {
            StateMachine.ChangeState(racer.playerInAirState);
        }
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isGrounded && !racer.isOnSlope)
        {

            racer.SetAccelerations(Racer.RacerType.Player);
            racer.SetVelocityX(racer.movementVelocity);
        }
        else if(isGrounded && racer.isOnSlope)
        {
                
            
            racer.SetAccelerations(Racer.RacerType.Player);
            racer.SetVelocityX(racer.movementVelocity * racer.slopeNormalPerpendicular.x * -1);
            racer.SetVelocityY(racer.movementVelocity * racer.slopeNormalPerpendicular.y * -1);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
}
