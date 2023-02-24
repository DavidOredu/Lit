using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_JumpState : PlayerNetwork_AbilityState
{
    protected int amountOfJumpsLeft;
    public bool poweredJump;

    public PlayerNetwork_JumpState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        racer.SetVelocityY(racer.jumpVelocity);
        racer.runnerFeedbacks.jumpFeedback?.PlayFeedbacks();
        isAbilityDone = true;
        amountOfJumpsLeft--;
        racer.playerInAirState.SetIsJumping();

       // racer.canSpawnDust = true;
      //  racer.playerLandState.SpawnDust();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpLeft()
    {
        amountOfJumpsLeft = racer.amountOfJumps;
    }

    public void DecreaseAmountOfJumpsLeft()
    {
        amountOfJumpsLeft--;
    }

}
