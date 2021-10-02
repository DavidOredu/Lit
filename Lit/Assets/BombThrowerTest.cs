using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class BombThrowerTest : MonoBehaviour
{
    private InputManager inputManager;
    private Camera cam;
    public BombScript bomb;
    public PlayerInputHandlerNetwork inputHandler;
    public float throwForce;
    // Start is called before the first frame update
    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }
    private void OnEnable()
    {
        inputManager.OnStartTouch += StartDragging;
        inputManager.OnMoveTouch += UpdateDragging;
        inputManager.OnStationaryTouch += UpdateDragging;
        inputManager.OnEndTouch += EndDragging;
        cam = Camera.main;
    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= StartDragging;
        inputManager.OnMoveTouch -= UpdateDragging;
        inputManager.OnStationaryTouch -= UpdateDragging;
        inputManager.OnEndTouch -= EndDragging;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
          //  inputHandler.UseDragInput();
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            
        }
        if (bomb.isDragging)
        {
        //   UpdateDragging(finger.screenPosition, Time.time, finger);

        }
    }
    void StartDragging(Vector2 screenPosition, float time, Finger finger)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0f;
        bomb.throwForce = throwForce;
        bomb.OnBombDragStart(worldCoordinates);
        Debug.Log("Dragging the flame bomb!");
    }
    void UpdateDragging(Vector2 screenPosition, float time, Finger finger)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0f;
        bomb.OnDrag(worldCoordinates);
    }
    void EndDragging(Vector2 screenPosition, float time, Finger finger)
    {
        bomb.OnBombDragEnd();
    }
}

