using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangeAttackStateData", menuName = "Data/State Data/Range Attack State")]

public class D_RangeAttackState : ScriptableObject
{
    public GameObject projectile;
    public int projectileDamage = 10;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance;
}
