using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class CursorController : MonoBehaviour
{
    // Speed of the cursor when controlled by a controller
    [SerializeField] float speed = 1750;
    [SerializeField] FittsButtonHandler fittsButtonHandler;
    ControllerSupport controller;

    Vector2 movement, cursor;
    // Sets up Unity Input System and initializes events
    private void Awake()
    {
        controller = new ControllerSupport();

        // Event for when a controller click is performed
        controller.Subject.Click.performed += ctx => ClickButton();

        // Event for detecting when there is / isnt movement from the gamepad
        controller.Subject.Movement.performed += move => movement = move.ReadValue<Vector2>();
        controller.Subject.Movement.canceled += move => movement = Vector2.zero;
    }

    // Handles functionality for clicking objects
    void ClickButton()
    {
        // Checks if the consent toggle has already been pressed
        bool hasCheckedToggle = false;
        // Set up event data and Raycast
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        var buttons = new List<Button>();
        EventSystem.current.RaycastAll(eventData, results);
        // Evaluates if UI elements are present
        if (results.Where(r => r.gameObject.layer == 5).Count() > 0) // Layer 5 is the UI layer
        {
            foreach(var r in results)
            {
                // Adds to button list if a button is pressed
                if (r.gameObject.TryGetComponent<Button>(out Button button))
                {
                    buttons.Add(button);
                }
                // Toggles the consent toggle if is pressed and ensures retoggeling is disabled
                if (r.gameObject.transform.parent.TryGetComponent<Toggle>(out Toggle toggle) && !hasCheckedToggle)
                {
                    hasCheckedToggle = true;
                    toggle.isOn = !toggle.isOn;
                }
            }
        }
        // Early out if no button is pressed
        if (buttons.Count() == 0)
            return;
        // Edge case to ensure that a button cannot be pressed together with the error button
        else if (buttons.Contains(fittsButtonHandler.errorButton) && buttons.Count != 1)
        {
            buttons.Remove(fittsButtonHandler.errorButton);
        }
        // Executes onClick functionality of the buttons
        foreach (var button in buttons)
        {
            button.Select();
            button.onClick.Invoke();
        }
    }

    private void Update()
    {
        // Handles Controller Cursor movement
        if (movement != Vector2.zero)
        {
            // Updates the cursor position based on controller input
            cursor += (movement * speed * Time.deltaTime);
            Mouse.current.WarpCursorPosition(cursor);
        }
        else
        {
            // Only reads value when not reading controller input to avoid pixel movement jittering
            cursor = Mouse.current.position.ReadValue();
        }
    }

    private void OnEnable()
    {
        controller.Subject.Enable();
    }

    private void OnDisable()
    {
        controller.Subject.Disable();
    }
}
