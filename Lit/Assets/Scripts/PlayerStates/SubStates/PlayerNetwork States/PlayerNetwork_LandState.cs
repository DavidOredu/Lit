using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_LandState : PlayerNetwork_GroundedState
{
    public PlayerNetwork_LandState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
      //  playerData.spawnDust = true;

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
            StateMachine.ChangeState(racer.playerMoveState);
        }
        else if (isAnimationFinished)
        {
            StateMachine.ChangeState(racer.playerIdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

      //  SpawnDust();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }

   // public void SpawnDust()
   // {
     //   if (isGrounded == true)
     //   {
     //       if (playerData.spawnDust == true)
      //      {
     //           GameObject.Instantiate(player.dustPS, player.GroundCheck.position, Quaternion.identity);
     //           playerData.spawnDust = false;
     //       }
     //   }
    //    else
    //    {
    //        playerData.spawnDust = true;
    //    }

  //  }
}
