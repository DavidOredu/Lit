﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPerkData", menuName = "Data/Perk Data")]
public class PerkData : ScriptableObject
{
    /// <summary>
    /// The name of the perk.
    /// </summary>
    [Tooltip("The name of the perk.")]
    public string perkName;

    /// <summary>
    /// The value of the perk. Could be a value between 0 and 1 for percentage values or 0 or 1 for boolean values.
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("The value of the perk. Could be a value between 0 and 1 for percentage values or 0 or 1 for boolean values.")]
    public float perkValue;

    /// <summary>
    /// Type of perk. Is it a switch or value?
    /// </summary>
    [Tooltip("Type of perk. Is it a switch or value?")]
    public PerkType perkType;


    /// <summary>
    /// Perk's activity. When should the perk be used.
    /// </summary>
    [Tooltip("Perk's activity. When should the perk be used.")]
    public PerkState perkActivity;

    /// <summary>
    /// Perk's call rate. How much should a perk be called after activation. NOTE: If we need to set a constant value for number of perk calls, change the enum to an int and set the number of time's a perk should be called.
    /// </summary>
    [Tooltip("Perk's call rate. How much should a perk be called after activation. NOTE: If we need to set a constant value for number of perk calls, change the enum to an int and set the number of time's a perk should be called.")]
    public PerkCallRate perkCallRate;

    [Header("BOOLEAN PERK VARIABLES")]
    /// <summary>
    /// A probability for a perk to function.
    /// </summary>
    [Range(0, 100)]
    [Tooltip("A probability for a perk to function.")]
    public float perkChance;

    /// <summary>
    /// How should the perk's call chance scale when in an awakened state or in our native map. Can be used either for bool or float type perks.
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("How should the perk's call chance scale when in an awakened state or in our native map. Can be used either for bool or float type perks.")]
    public float hyperChanceMultiplier;

    /// <summary>
    /// How should the perk's call chance scale when in an awakened state and in our native map. Can be used either for bool or float type perks.
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("How should the perk's call chance scale when in an awakened state and in our native map. Can be used either for bool or float type perks.")]
    public float ultimateChanceMultiplier;

    [Header("FLOAT PERK VARIABLES")]
    /// <summary>
    /// Nature of perk. Does it deal positive or negative effects on racer?
    /// </summary>
    [Tooltip("Nature of perk. Does it deal positive or negative effects on racer?")]
    public PerkNature perkNature;

    /// <summary>
    /// How should the perk scale when in an awakened state or in our native map.
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("How should the perk scale when in an awakened state or in our native map.")]
    public float hyperMultiplier;

    /// <summary>
    /// How should the perk scale when in an awakened state and in our native map.
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("How should the perk scale when in an awakened state and in our native map.")]
    public float ultimateMultiplier;

    public GameObject effectParticles;

    public enum PerkNature
    {
        Positive,
        Negative,
    }
    public enum PerkType
    {
        numeric,
        boolean,
    }
    public enum PerkState
    {
        Base,
        Hyper,
        Ultimate,
    }
    public enum PerkCallRate
    {
        Once,
        Continuous
    }
}
