using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;

public class InputManager : SingletonDontDestroy<InputManager>
{
    public ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> activeTouches { get; private set; }
    public EventSystem eventSystem;
    public InputSystemUIInputModule inputModule;

    public PlayerInputActions inputActions { get; private set; }

    public delegate void StartTouchEvent(Vector2 position, float time, Finger finger = null);
    public event StartTouchEvent OnStartTouch;
    public delegate void MoveTouchEvent(Vector2 position, float time, Finger finger = null);
    public event MoveTouchEvent OnMoveTouch;
    public delegate void StationaryTouchEvent(Vector2 position, float time, Finger finger = null);
    public event StationaryTouchEvent OnStationaryTouch;
    public delegate void EndTouchEvent(Vector2 position, float time, Finger finger = null);
    public event EndTouchEvent OnEndTouch;

    public delegate Finger TouchDownEvent();
    public event TouchDownEvent OnTouchDown;
    public delegate Finger TouchingEvent();
    public event TouchingEvent OnTouching;
    public delegate Finger TouchingStationaryEvent();
    public event TouchingStationaryEvent OnTouchWait;
    public delegate Finger TouchUpEvent();
    public event TouchUpEvent OnTouchUp;
    public override void Awake()
    {
        base.Awake();

        inputActions = new PlayerInputActions();
        TouchSimulation.Enable();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;

    }
    private void Update()
    {
        GetEventSystem();
        activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

        
        //for (int i = 0; i < touches.Count; i++)
        //{
        //    Debug.Log(touches[i]);
        //}
        
        //foreach (var touch in touches)
        //{
        //    Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        //}
    }
    
    private void FixedUpdate()
    {
        StationaryTouch();
    }
    void GetEventSystem()
    {
        if(eventSystem == null)
            eventSystem = EventSystem.current;
        if(eventSystem == null) { return; }
        if(inputModule == null)
            inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
    }
    private void OnDisable()
    {
        inputActions.Disable();
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;


    }
    private void Start()
    {
        //inputActions.Touch.TouchPress.started += ctx => StartTouch(ctx);
        //inputActions.Touch.TouchPress.performed += ctx => UpdateTouch(ctx);
        //inputActions.Touch.TouchPress.canceled += ctx => EndTouch(ctx);

        TouchSimulation.Enable();
    }
    private void StationaryTouch()
    {
        var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                if (!inputModule.IsPointerOverGameObject(0))
                {
                 //   Debug.Log("Touch stationary " + touches[i].finger.screenPosition + " " + Time.time);
                    OnStationaryTouch?.Invoke(touches[i].finger.screenPosition, Time.time, touches[i].finger);
                    OnTouchWait?.Invoke();
                }
            }
        }
    }
   

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + inputActions.Touch.TouchPosition.ReadValue<Vector2>());
        OnStartTouch?.Invoke(inputActions.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
    private void UpdateTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch moved " + inputActions.Touch.TouchPosition.ReadValue<Vector2>());
        OnMoveTouch?.Invoke(inputActions.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch ended " + inputActions.Touch.TouchPosition.ReadValue<Vector2>());
        OnEndTouch?.Invoke(inputActions.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }

    private void FingerDown(Finger finger)
    {
        if (!inputModule.IsPointerOverGameObject(0))
        {
         //   Debug.Log("Touch started " + finger.screenPosition + " " + Time.time);
            OnStartTouch?.Invoke(finger.screenPosition, Time.time, finger);
            OnTouchDown?.Invoke();
        }
    }
    private void FingerMove(Finger finger)
    {
        if (!inputModule.IsPointerOverGameObject(0))
        {
         //   Debug.Log("Touch moved " + finger.screenPosition + " " + Time.time);
            OnMoveTouch?.Invoke(finger.screenPosition, Time.time, finger);
            OnTouching?.Invoke();
        }
    }
    private void FingerStationary(Finger finger)
    {
        Debug.Log("Touch stationary " + finger.screenPosition + " " + Time.time);
        OnStationaryTouch?.Invoke(finger.screenPosition, Time.time, finger);
    }
    private void FingerUp(Finger finger)
    {
        if (!inputModule.IsPointerOverGameObject(0))
        {
         //   Debug.Log("Touch ended " + finger.screenPosition + " " + Time.time);
            OnEndTouch?.Invoke(finger.screenPosition, Time.time, finger);
            OnTouchUp?.Invoke();
        }
    }
}
