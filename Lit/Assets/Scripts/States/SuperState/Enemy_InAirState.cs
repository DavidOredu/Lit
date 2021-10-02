using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_InAirState : State
{
    public bool isGrounded { get; protected set; }

    public Enemy_InAirState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (isGrounded && racerEntity.CurrentVelocity.y < 0.01f)
        {
            StateMachine.ChangeState(racer.opponentLandState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!isGrounded)
        {

            //  player.SetVelocityX(player.movementVelocity);


            racerEntity.Anim.SetFloat("yVelocity", racerEntity.CurrentVelocity.y);
            racerEntity.Anim.SetFloat("xVelocity", Mathf.Abs(racerEntity.CurrentVelocity.x));
        }
    }
}
