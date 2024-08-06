using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public InputActions actions;
    private InputAction pressAction;
    private InputAction touchDragAction;
    private InputAction scrollAction;
    private bool touchPressed = false;

    private Camera mainCamera;
    private float baseZoom = 10;
    private float currentZoom;
    public float maxZoom = 15;
    public float minZoom = 1;
    public float zoomSpeed = 1f;

    public float dragSpeed = 1f; // Speed at which the camera moves

    public bool logging = false;

    void Awake()
    {
        mainCamera = Camera.main;

        actions = new InputActions();

        // Find the actions from the Input Action Asset
        pressAction = actions.Touch.Press;
        touchDragAction = actions.Touch.Drag;
        scrollAction = actions.Touch.Scroll;

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

        actions.Touch.Scroll.performed += OnScroll;

        mainCamera.orthographicSize = baseZoom;
        currentZoom = baseZoom;
    }

    private void OnEnable()
    {
        // Enable the input actions
        pressAction.Enable();
        touchDragAction.Enable();
        scrollAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions
        pressAction.Disable();
        touchDragAction.Disable();
        scrollAction.Enable();
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

    private void OnScroll(InputAction.CallbackContext context)
    {
        // Read the scroll value
        float scrollValue = context.ReadValue<float>();

        // Adjust the current zoom based on the scroll value
        currentZoom -= scrollValue * zoomSpeed;

        // Clamp the zoom value to ensure it stays within the min and max bounds
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Optionally log the scroll and value
        if (logging)
        {
            Debug.Log("Scroll: " + scrollValue + " | Current Value: " + currentZoom);
        }
    }
}
