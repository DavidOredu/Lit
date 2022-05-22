using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandlerNetwork : NetworkBehaviour
{
    public InputManager inputManager;
    public PlayerInputActions inputActions;
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public float inputHoldTimeNormalized { get; private set; }
    public bool PowerupInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DragInput { get; private set; }
    public bool DragInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool AttackInputStop { get; private set; }
    public float lastInputTime { get; private set; } = Mathf.NegativeInfinity;

    [SerializeField]
    private float inputHoldTime = 0.2f;



    private float jumpInputStartTime;

    // private float attackInputStartTime;
    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        inputActions = inputManager.inputActions;
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        DeregisterInputs();
    }

    #region Input Assignment Functions
    public void RegisterInputs()
    {
        inputActions.Mechanics.Jump.started += ctx => Jump_Started(ctx);
        inputActions.Mechanics.Jump.performed += ctx => Jump_Performed(ctx);
        inputActions.Mechanics.Jump.canceled += ctx => Jump_Canceled(ctx);

        inputActions.Mechanics.Powerup.performed += ctx => Powerup_Performed(ctx);
        inputActions.Mechanics.Powerup.canceled += ctx => Powerup_Canceled(ctx);
    }

    public void DeregisterInputs()
    {
        inputActions.Mechanics.Jump.started -= ctx => Jump_Started(ctx);
        inputActions.Mechanics.Jump.performed -= ctx => Jump_Performed(ctx);
        inputActions.Mechanics.Jump.canceled -= ctx => Jump_Canceled(ctx);
    }
#endregion

    #region Jump Action
    void Jump_Started(InputAction.CallbackContext ctx)
    {
        if (inputManager.inputModule.IsPointerOverGameObject(0)) { return; }
        jumpInputStartTime = Time.time;
        Debug.Log("Jump Action has started!");
    }
    void Jump_Performed(InputAction.CallbackContext ctx)
    {
        if (inputManager.inputModule.IsPointerOverGameObject(0)) { return; }
        JumpInputStop = false;
        JumpInput = true;
        Debug.Log("Jump Action has been performed!");
    }
    void Jump_Canceled(InputAction.CallbackContext ctx)
    {
        if (inputManager.inputModule.IsPointerOverGameObject(0)) { return; }
        JumpInputStop = true;
    }
    #endregion
    
    #region Powerup Action
    void Powerup_Performed(InputAction.CallbackContext context)
    {
        if (inputManager.inputModule.IsPointerOverGameObject(0)) { return; }
        PowerupInput = true;
        Debug.Log("Powerup Action has been performed!");
    }
    void Powerup_Canceled(InputAction.CallbackContext context)
    {
        if (inputManager.inputModule.IsPointerOverGameObject(0)) { return; }
    }
    #endregion
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
    public void OnDragInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DragInput = true;
            DragInputStop = false;
        }
        if (context.canceled)
        {
            DragInputStop = true;
        }
    }
    public void OnLookInput(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();

    }
    public void OnTapInput(InputAction.CallbackContext context)
    {

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
    public void UsePowerupInput()
    {
        PowerupInput = false;
    }
    public void UseAttackInput()
    {
        AttackInput = false;
    }
    public void UseDragInput()
    {
        DragInput = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
            inputHoldTimeNormalized = 1f;
        }
        else
        {
            var inputHoldTime = (jumpInputStartTime + this.inputHoldTime) - Time.time;
            inputHoldTimeNormalized = inputHoldTime / this.inputHoldTime;
        }
    }
}
