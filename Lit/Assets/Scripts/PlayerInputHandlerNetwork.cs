using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerInputHandlerNetwork : NetworkBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool AttackInputStop { get; private set; }
    public float lastInputTime { get; private set; } = Mathf.NegativeInfinity;

    [SerializeField]
    private float inputHoldTime = 0.2f;
    

    private float jumpInputStartTime;
   // private float attackInputStartTime;

    private void Update()
    {
      
        CheckJumpInputHoldTime();
        lastInputTime = Time.time;
    }
    public override void OnStartAuthority()
    {
    //    enabled = true;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
        
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            AttackInputStop = false;
           // attackInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            AttackInputStop = true;
        }
    }
    public void UseJumpInput()
    {
        JumpInput = false;
    }

    public void UseAttackInput()
    {
        AttackInput = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
}
