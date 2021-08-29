using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Racer
{
    public Difficulty enemyDifficulty;
    public D_Entity entityData;
    
    //private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform jumpCheck;

    private float currentStunResistance;
    private float lastDamageTime;

    protected bool isStunned;

    
    public EnemyPowerup enemyPowerup { get; private set; }

    public enum Difficulty
    {
        Easy,
        Normal, 
        Hard,
    }
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        
        enemyPowerup = transform.GetComponentInChildren<EnemyPowerup>();
        //  atsm = aliveGO.GetComponent<AnimationToStateMachine>();
    }

    public override void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        StateMachine.DamagedState.LogicUpdate();
        StateMachine.AwakenedState.LogicUpdate();
        base.Update();
    }

    public override void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        StateMachine.DamagedState.PhysicsUpdate();
        StateMachine.AwakenedState.PhysicsUpdate();
        base.FixedUpdate();
  
        if (CheckIfHasPowerup())
        {
            enemyPowerup.UsePowerup();
        }
      
        
    }

    public override void LateUpdate()
    {
        StateMachine.CurrentState.LateUpdate();
        StateMachine.DamagedState.LateUpdate();
        StateMachine.AwakenedState.LateUpdate();
        base.LateUpdate();
        
    }
 
    public override void OnCollisionEnter2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionEnter(other);
        StateMachine.DamagedState.OnCollisionEnter(other);
        StateMachine.AwakenedState.OnCollisionEnter(other);
        base.OnCollisionEnter2D(other);
     
    }
    
    public override void OnCollisionStay2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionStay(other);
        StateMachine.DamagedState.OnCollisionStay(other);
        StateMachine.AwakenedState.OnCollisionStay(other);
        base.OnCollisionStay2D(other);
    }
    public override void OnCollisionExit2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionExit(other);
        StateMachine.DamagedState.OnCollisionExit(other);
        StateMachine.AwakenedState.OnCollisionExit(other);
        base.OnCollisionExit2D(other);
    }
    public virtual void CurrentStateAnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }
    public virtual void DamageStateAnimationTrigger()
    {
        StateMachine.DamagedState.AnimationTrigger();
    }
    public virtual void CurrentStateAnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
    public virtual void DamageStateAnimationFinishTrigger()
    {
        StateMachine.DamagedState.AnimationFinishTrigger();
    }
    #region Set Functions
    public virtual void SetAllVariables()
    {
        runner.player = GetComponent<Opponent>();
        runner.stickman = GetComponent<Stickman>();

        jumpCheck.position = difficultyData.jumpCheckPosition;
    }
    public virtual void SetAIDifficulty()
    {
        switch (enemyDifficulty)
        {
            case Difficulty.Easy:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Easy");
                break;
            case Difficulty.Normal:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Normal");
                break;
            case Difficulty.Hard:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Hard");
                break;
            default:
                break;
        }
    }
    #endregion


    #region Check Functions
    
    public virtual bool CheckIfCanJump()
    {
        return Physics2D.OverlapCircle(jumpCheck.position, difficultyData.jumpCheckRadius, difficultyData.whatToJumpTo);
    }
    
    public bool CheckIfHasPowerup()
    {
        if(powerup == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    #endregion
    //public virtual bool CheckLedge()
    //{
    //    return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    //}
    //public virtual bool CheckWall()
    //{
    //    return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    //}
   
 
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }
   
    public virtual bool CheckPlayerInTooCloseAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.tooCloseAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
   
    
    
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        //Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.tooCloseAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);

        Gizmos.DrawWireSphere(jumpCheck.position, difficultyData.jumpCheckRadius);

    }
}
