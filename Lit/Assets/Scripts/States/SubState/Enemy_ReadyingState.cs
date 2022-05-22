using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ReadyingState : Enemy_AbilityState
{
    private bool perfectLaunchProbability;
    public Enemy_ReadyingState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        perfectLaunchProbability = racerEntity.actionController.RunPerfectLaunchProbability();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (racer.canRace)
        {
            if (racer.perfectLaunch)
            {
                racer.movementVelocity = racer.moveVelocityResource;
            }

            racer.StateMachine.ChangeState(racer.opponentMoveState);
        }
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        base.OnCollisionExit(collision);
    }

    public override void OnCollisionStay(Collision2D collision)
    {
        base.OnCollisionStay(collision);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (perfectLaunchProbability)
        {
            if (GameManager.instance.raceCountdownTimer.CurrentTime() <= 0.2)
            {
                racer.perfectLaunch = true;
                racer.canRace = true;
            }
        }
        else
        {
            if (GameManager.instance.raceCountdownTimer.isTimeUp)
            {
                racer.canRace = true;
            }
        }
    }
}
