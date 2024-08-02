using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public InputActions actions;
    private InputAction touchPositionAction;
    private InputAction touchDragAction;
    private bool touchPressed = false;

    private Camera mainCamera;
    public int baseZoom = 1;
    public int currentZoom;

    public float dragSpeed = 1f; // Speed at which the camera moves

    public bool logging = false;

    void Awake()
    {
        mainCamera = Camera.main;

        actions = new InputActions();

        // Find the actions from the Input Action Asset
        touchPositionAction = actions.Touch.Press;
        touchDragAction = actions.Touch.Drag;

        actions.Touch.Press.performed += ctx =>
        {
            touchPressed = true;
            if(logging) Debug.Log("Touch Enabled");
        };

        actions.Touch.Press.canceled += ctx =>
        {
            touchPressed = false;
            if(logging) Debug.Log("Touch Released");
        };

        mainCamera.orthographicSize = baseZoom;
        currentZoom = baseZoom;
    }

    private void OnEnable()
    {
        // Enable the input actions
        touchPositionAction.Enable();
        touchDragAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions
        touchPositionAction.Disable();
        touchDragAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchPressed)
        {
            Vector2 dragDelta = touchDragAction.ReadValue<Vector2>();
            if (logging && (dragDelta.x > 0.01f || dragDelta.y > 0.01f)) Debug.Log("Drag Delta: " + dragDelta);
            MoveCamera(dragDelta);
        }

        mainCamera.orthographicSize = currentZoom;
    }

    private void MoveCamera(Vector2 dragDelta)
    {
        // Create a movement vector based on the drag delta
        Vector3 move = new Vector3(-dragDelta.x, -dragDelta.y, 0) * dragSpeed * Time.deltaTime;

        // Calculate the rotation needed to align with the camera's 45-degree rotation on the Y axis
        Quaternion rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);

        // Apply the rotation to the movement vector
        move = rotation * move;

        // Apply the movement to the camera's position
        mainCamera.transform.position += move;
    }
}
