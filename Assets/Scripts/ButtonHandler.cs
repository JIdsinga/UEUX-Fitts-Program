using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] InputButtonHandler inputButtonHandler;
    [SerializeField] Toggle consentToggle;

    void Update()
    {
        ActivateStartButton();
    }

    void ActivateStartButton()
    {

        if (inputButtonHandler.selectedButton != null && consentToggle.isOn)
            startButton.interactable = true;
        else startButton.interactable = false;
    }
}
