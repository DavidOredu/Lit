using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]

public class D_Entity : ScriptableObject
{
    public int maxHealth = 30;

    public float DamageHopSpeed = 3;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    public float minAgroDistance = 3;
    public float maxAgroDistance = 4;
    public float tooCloseAgroDistance = 1f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    public float playerKnockbackSpeed = 4f;
    public float playerKnockbackTime = 0.2f;
    public Vector2 playerKnockbackAngle;

    public float closeRangeActionDistance = 1;

    public GameObject hitParticle;

    public int direction;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
