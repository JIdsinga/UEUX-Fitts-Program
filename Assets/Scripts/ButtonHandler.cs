using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // References
    [SerializeField] Button startButton;
    [SerializeField] InputButtonHandler inputButtonHandler;
    [SerializeField] Toggle consentToggle;

    void Update()
    {
        ActivateStartButton();
    }

    // Allows the start button to be pressed when Consent is given and an Input Device is selected
    void ActivateStartButton()
    {
        if (inputButtonHandler.selectedButton != null && consentToggle.isOn)
            startButton.interactable = true;
        else startButton.interactable = false;
    }

    // Resets the button state to default
    public void ResetButtons()
    {
        inputButtonHandler.selectedButton.GetComponent<Image>().color = inputButtonHandler.defaultColour;
        inputButtonHandler.selectedButton = null;
        consentToggle.isOn = false;
    }

    // Allows for quiting of the application
    public void Quit()
    {
        Application.Quit();
    }
}
