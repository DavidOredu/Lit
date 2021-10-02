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
        racer.canSpawnDust = true;
        SpawnDust();
    }

    public override void Exit()
    {
        base.Exit();
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

       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }
    public void SpawnDust()
    {
        if (isGrounded)
        {
            if (racer.canSpawnDust)
            {
                var dustPrefab = Resources.Load<GameObject>("Dust");
                racer.InstantiateObject(dustPrefab, racer.GroundCheck.position, Quaternion.identity, racer.transform);
                racer.canSpawnDust = false;
            }
        }
        else
        {
            racer.canSpawnDust = true;
        }

    }
}
