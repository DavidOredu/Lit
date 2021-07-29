using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkState 
{
    
    protected FiniteStateMachine StateMachine;
    
    protected PlayerEntity racerEntity;
    protected PlayerData playerData;
    protected Racer racer;

    protected bool isAnimationFinished;

    protected float startTime { get; private set; }

    private string animBoolName;

    public PlayerNetworkState(FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData, Racer playerNN)
    {
        
        this.StateMachine = stateMachine;
        this.playerData = playerData;
        this.racer = playerNN;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        racer.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        racer.Anim.SetBool(animBoolName, false);

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void LateUpdate()
    {

    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }

    public virtual void OnCollisionEnter(Collision2D collision) { }

    public virtual void OnCollisionStay(Collision2D collision) { }
    

    public virtual void OnCollisionExit(Collision2D collision) { }
}
