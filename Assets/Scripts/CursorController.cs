using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class CursorController : MonoBehaviour
{
    [SerializeField] float speed = 1000;
    [SerializeField] FittsButtonHandler fittsButtonHandler;
    ControllerSupport controller;

    Vector2 movement, cursor;
    private void Awake()
    {
        controller = new ControllerSupport();

        controller.Subject.Click.performed += ctx => ClickButton();

        controller.Subject.Movement.performed += move => movement = move.ReadValue<Vector2>();
        controller.Subject.Movement.canceled += move => movement = Vector2.zero;
    }

    void ClickButton()
    {
        bool hasCheckedToggle = false;
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        var buttons = new List<Button>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Where(r => r.gameObject.layer == 5).Count() > 0) //6 being my UILayer
        {
            foreach(var r in results)
            {
                Debug.Log(r.gameObject.name);
                if (r.gameObject.TryGetComponent<Button>(out Button button))
                {
                    buttons.Add(button);
                    //button.onClick.Invoke();
                }
                if (r.gameObject.transform.parent.TryGetComponent<Toggle>(out Toggle toggle) && !hasCheckedToggle)
                {
                    hasCheckedToggle = true;
                    toggle.isOn = !toggle.isOn;
                }
            }
        }
        if (buttons.Count() == 0)
            return;
        else if (buttons.Contains(fittsButtonHandler.errorButton) && buttons.Count != 1)
        {
            buttons.Remove(fittsButtonHandler.errorButton);
        }
        foreach (var button in buttons)
        {
            button.onClick.Invoke();
        }
    }

    private void Update()
    {
        if (movement != Vector2.zero)
        {
            //Debug.Log("movement");
            Debug.Log(movement * speed * Time.deltaTime);
            cursor += (movement * speed * Time.deltaTime);
            Mouse.current.WarpCursorPosition(cursor);
        }
        else
        {
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
