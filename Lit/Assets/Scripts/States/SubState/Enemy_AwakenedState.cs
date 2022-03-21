using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AwakenedState : State
{
    public bool canUseAbility = false;
    public Enemy_AwakenedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        racer.isAwakened = true;
        if (racer.GamePlayer.powerup != null)
        {
            if (GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID)
            {
                if (racer.GamePlayer.powerup.canBeMany)
                {
                    racer.GamePlayer.powerupButton.powerupBehaviour.powerupAmmo = int.MaxValue;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        racer.isAwakened = false;
        if (racer.GamePlayer.powerup != null)
        {
            if (GameManager.instance.currentLevel.buttonMap == racer.runner.stickmanNet.currentColor.colorID)
            {
                if (racer.GamePlayer.powerup.canBeMany)
                {
                    racer.GamePlayer.powerupButton.powerupBehaviour.powerupAmmo = 3;
                }
            }
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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

        if (canUseAbility)
        {
            racer.racerAwakening.AwakenedAbility(racer.runner.stickmanNet.currentColor.colorID);
            canUseAbility = false;
        }
    }
}
