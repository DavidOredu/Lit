﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FiniteStateMachine
{
    public State CurrentState { get; private set; }
    public State DamagedState { get; private set; }
    public State AwakenedState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void InitializeDamage(State DamageState)
    {
        DamagedState = DamageState;
        DamagedState.Enter();
    }
    public void InitializeAwakened(State AwakenedState)
    {
        this.AwakenedState = AwakenedState;
        this.AwakenedState.Enter();
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    public void ChangeDamagedState(State newDamagedState)
    {
        if(DamagedState.racer != null)
            DamagedState.Exit();
        DamagedState = newDamagedState;
        DamagedState.Enter();
    }
    public void ChangeAwakenedState(State newAwakenedState)
    {
        if (AwakenedState.racer != null)
            AwakenedState.Exit();
        AwakenedState = newAwakenedState;
        AwakenedState.Enter();
    }
    public void TurnOffDamageState()
    {
        DamagedState.Exit();
    }
    public void TurnOnAwakenedState()
    {
        AwakenedState.Enter();
    }
    public void TurnOffAwakenedState()
    {
        AwakenedState.Exit();
    }
}