using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LandState: Enemy_GroundedState
{
    public Enemy_LandState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
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
            StateMachine.ChangeState(racer.opponentMoveState);
        }
        else if (isAnimationFinished)
        {
            StateMachine.ChangeState(racer.opponentIdleState);
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
