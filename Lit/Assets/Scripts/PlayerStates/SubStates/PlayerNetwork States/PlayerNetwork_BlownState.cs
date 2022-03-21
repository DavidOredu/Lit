﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_BlownState : PlayerNetwork_DamagedState
{
    public PlayerNetwork_BlownState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        var damageEffectPrefab = Resources.Load<GameObject>($"{6}/DamageEffect");
        damageEffect = racer.InstantiateObject(damageEffectPrefab, racer.transform.position, Quaternion.identity, racer.transform);

        racer.runnerFeedbacks.windFeedback.PlayFeedbacks();
    }

    public override void Exit()
    {
        base.Exit();

        racer.runnerFeedbacks.windFeedback.StopFeedbacks();
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
    }
}
