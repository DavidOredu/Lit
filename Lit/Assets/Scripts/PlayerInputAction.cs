// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Player.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""Mechanics"",
            ""id"": ""b9858717-ce06-43f5-9808-85bc983bb120"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""8a1584f8-8702-4cf1-8def-6893af493194"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e70baee3-3219-4a36-a1b5-d15505774395"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""046bd92a-664e-47ef-bb1a-e6719254870b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""44118a5c-550b-4cf1-ba7e-5b2d4544b7aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Powerup"",
                    ""type"": ""Button"",
                    ""id"": ""8319b939-7f0d-4317-a532-72ff8c754e77"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Awaken"",
                    ""type"": ""Button"",
                    ""id"": ""664d576f-99a3-4d1c-87d5-c922eae76239"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""00cd5082-4613-4d7a-a49c-46713d60bfd0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3e6447e2-b628-4d4b-acf1-0c52e87cbbc1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ac64ef11-fba3-4634-a317-817c9bf929d4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7282c2ec-a58f-4d77-951c-9459469275e2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a141f81f-b4b3-4d13-b857-67bfd74a7e4f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""26988de0-3863-4750-b189-782458d9a2b6"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f060d3c-696c-4571-a9d6-4a85798a277c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a568010-155a-4623-bb28-2ab0907d9a63"",
                    ""path"": ""<Touchscreen>/touch1/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f12ecba0-a5e4-4fd7-bb6e-cf639fd82459"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""973ab0e2-f6df-4898-a025-5ef0248834f3"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""178d2645-0ca1-409d-9adf-69946dbeaced"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89472c99-814b-4c59-8c47-5a49049a3ef9"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Powerup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4f6470d-8861-40c6-b5a3-4b7824d944d8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Awaken"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""id"": ""6c42a011-c2f2-43df-8f6a-459870c1ba47"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7c7de7de-503f-4e1f-b888-29a0688db568"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""Button"",
                    ""id"": ""94f153c3-ac06-44b1-8541-ed4f6fe52ebb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a7e05e1e-2e57-4fd8-ab1d-2af128428b8d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""883a745d-df1c-4e67-9aab-ae5f3f87f7d5"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f5a0ab6-f9a6-4673-9dbb-a20918e57c89"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a3af718-5834-41b7-9695-680a28cc9b3c"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""ea26e187-4409-40a4-a4b7-0d35cc0fa97b"",
            ""actions"": [
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""3f44e0bf-0243-4ff0-a1b4-5fe0da5eae14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""eb047b1e-8ccf-49c1-9116-bcb8b766c78a"",
                    ""path"": ""*/{Back}"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Mechanics
        m_Mechanics = asset.FindActionMap("Mechanics", throwIfNotFound: true);
        m_Mechanics_Movement = m_Mechanics.FindAction("Movement", throwIfNotFound: true);
        m_Mechanics_Jump = m_Mechanics.FindAction("Jump", throwIfNotFound: true);
        m_Mechanics_Fire = m_Mechanics.FindAction("Fire", throwIfNotFound: true);
        m_Mechanics_Drag = m_Mechanics.FindAction("Drag", throwIfNotFound: true);
        m_Mechanics_Powerup = m_Mechanics.FindAction("Powerup", throwIfNotFound: true);
        m_Mechanics_Awaken = m_Mechanics.FindAction("Awaken", throwIfNotFound: true);
        // Touch
        m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
        m_Touch_TouchInput = m_Touch.FindAction("TouchInput", throwIfNotFound: true);
        m_Touch_TouchPress = m_Touch.FindAction("TouchPress", throwIfNotFound: true);
        m_Touch_TouchPosition = m_Touch.FindAction("TouchPosition", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Back = m_UI.FindAction("Back", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Mechanics
    private readonly InputActionMap m_Mechanics;
    private IMechanicsActions m_MechanicsActionsCallbackInterface;
    private readonly InputAction m_Mechanics_Movement;
    private readonly InputAction m_Mechanics_Jump;
    private readonly InputAction m_Mechanics_Fire;
    private readonly InputAction m_Mechanics_Drag;
    private readonly InputAction m_Mechanics_Powerup;
    private readonly InputAction m_Mechanics_Awaken;
    public struct MechanicsActions
    {
        private @PlayerInputActions m_Wrapper;
        public MechanicsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Mechanics_Movement;
        public InputAction @Jump => m_Wrapper.m_Mechanics_Jump;
        public InputAction @Fire => m_Wrapper.m_Mechanics_Fire;
        public InputAction @Drag => m_Wrapper.m_Mechanics_Drag;
        public InputAction @Powerup => m_Wrapper.m_Mechanics_Powerup;
        public InputAction @Awaken => m_Wrapper.m_Mechanics_Awaken;
        public InputActionMap Get() { return m_Wrapper.m_Mechanics; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MechanicsActions set) { return set.Get(); }
        public void SetCallbacks(IMechanicsActions instance)
        {
            if (m_Wrapper.m_MechanicsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnJump;
                @Fire.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnFire;
                @Drag.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnDrag;
                @Drag.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnDrag;
                @Drag.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnDrag;
                @Powerup.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnPowerup;
                @Powerup.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnPowerup;
                @Powerup.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnPowerup;
                @Awaken.started -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnAwaken;
                @Awaken.performed -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnAwaken;
                @Awaken.canceled -= m_Wrapper.m_MechanicsActionsCallbackInterface.OnAwaken;
            }
            m_Wrapper.m_MechanicsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
                @Powerup.started += instance.OnPowerup;
                @Powerup.performed += instance.OnPowerup;
                @Powerup.canceled += instance.OnPowerup;
                @Awaken.started += instance.OnAwaken;
                @Awaken.performed += instance.OnAwaken;
                @Awaken.canceled += instance.OnAwaken;
            }
        }
    }
    public MechanicsActions @Mechanics => new MechanicsActions(this);

    // Touch
    private readonly InputActionMap m_Touch;
    private ITouchActions m_TouchActionsCallbackInterface;
    private readonly InputAction m_Touch_TouchInput;
    private readonly InputAction m_Touch_TouchPress;
    private readonly InputAction m_Touch_TouchPosition;
    public struct TouchActions
    {
        private @PlayerInputActions m_Wrapper;
        public TouchActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchInput => m_Wrapper.m_Touch_TouchInput;
        public InputAction @TouchPress => m_Wrapper.m_Touch_TouchPress;
        public InputAction @TouchPosition => m_Wrapper.m_Touch_TouchPosition;
        public InputActionMap Get() { return m_Wrapper.m_Touch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
        public void SetCallbacks(ITouchActions instance)
        {
            if (m_Wrapper.m_TouchActionsCallbackInterface != null)
            {
                @TouchInput.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchInput;
                @TouchInput.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchInput;
                @TouchInput.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchInput;
                @TouchPress.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPress;
                @TouchPress.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPress;
                @TouchPress.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPress;
                @TouchPosition.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouchPosition;
            }
            m_Wrapper.m_TouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchInput.started += instance.OnTouchInput;
                @TouchInput.performed += instance.OnTouchInput;
                @TouchInput.canceled += instance.OnTouchInput;
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
            }
        }
    }
    public TouchActions @Touch => new TouchActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Back;
    public struct UIActions
    {
        private @PlayerInputActions m_Wrapper;
        public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Back => m_Wrapper.m_UI_Back;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Back.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    public interface IMechanicsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnDrag(InputAction.CallbackContext context);
        void OnPowerup(InputAction.CallbackContext context);
        void OnAwaken(InputAction.CallbackContext context);
    }
    public interface ITouchActions
    {
        void OnTouchInput(InputAction.CallbackContext context);
        void OnTouchPress(InputAction.CallbackContext context);
        void OnTouchPosition(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnBack(InputAction.CallbackContext context);
    }
}
