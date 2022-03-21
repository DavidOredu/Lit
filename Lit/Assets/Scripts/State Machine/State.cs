using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class State 
{
    protected FiniteStateMachine StateMachine;
    protected Entity racerEntity;
    protected PlayerData playerData;
    protected D_DifficultyData difficultyData;
    public Racer racer { get; private set; }

    public float startTime { get; protected set; }

    public string animBoolName { get; private set; }

    protected bool isAnimationFinished;

    public State(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null)
    {
        this.racerEntity = entity;
        this.StateMachine = StateMachine;
        this.animBoolName = animBoolName;
        this.playerData = playerData;
        this.difficultyData = difficultyData;
        this.racer = racer;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        racer.Anim.SetBool(animBoolName, true);
        DoChecks();
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
