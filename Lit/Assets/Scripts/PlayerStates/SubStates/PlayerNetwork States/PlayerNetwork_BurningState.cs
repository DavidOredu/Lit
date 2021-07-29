using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_BurningState : PlayerNetwork_DamagedState
{
    public PlayerNetwork_BurningState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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

        var damageEffectPrefab = Resources.Load<GameObject>($"{1}/DamageEffect");
        damageEffect = racer.InstantiateObject(damageEffectPrefab, racer.transform.position, Quaternion.identity, racer.transform);

    }

    public override void Exit()
    {
        base.Exit();

        var damageEffectPS = damageEffect.GetComponent<ParticleSystem>();
        if (damageEffectPS != null)
        {
            var main = damageEffectPS.main;
            main.loop = false;
        }
        var PSInDamageEffect = damageEffect.GetComponentsInChildren<ParticleSystem>();
        if(PSInDamageEffect != null)
        {
            foreach(var ps in PSInDamageEffect)
            {
                var main = ps.main;
                main.loop = false;
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
    }
}
