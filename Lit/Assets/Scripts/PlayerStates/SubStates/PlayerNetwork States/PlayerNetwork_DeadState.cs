using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_DeadState : PlayerNetwork_DamagedState
{
    private Timer dissolveTimer;
    public PlayerNetwork_DeadState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void AnimationFinishTrigger()
    {
    }

    public override void AnimationTrigger()
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        racer.SetVelocityY(0);
        racer.RB.gravityScale = 0;
        racer.StateMachine.ChangeState(racer.playerQuickHaltState);

        racer.runner.stickmanNet.material = Resources.Load<Material>($"{racer.runner.stickmanNet.currentColor.colorID}/DissolveMat");
        racer.runner.stickmanNet.DefineCode();

        dissolveTimer = new Timer(playerData.dissolveTime);
        dissolveTimer.SetTimer();
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


        var completion = 1 - (dissolveTimer.CurrentTime() / dissolveTimer.MainTime());
        var amount = Mathf.Lerp(0, 1, playerData.dissolveCurve.Evaluate(completion));
        racer.runner.stickmanNet.material.SetFloat("_DissolveAmount", amount);

        if (dissolveTimer.isTimeUp)
        {

        }
        else
        {
            dissolveTimer.UpdateTimer();
            Debug.Log($"current dissolve time is {dissolveTimer.CurrentTime()}");
        }
    }
}
