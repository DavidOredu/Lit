using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_NullState : State
{
    public PlayerNetwork_NullState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }
}
