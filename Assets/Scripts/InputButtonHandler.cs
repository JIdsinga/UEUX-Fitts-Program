using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButtonHandler : MonoBehaviour
{
    public Button selectedButton;
    [SerializeField] Color selectedColour;
    [SerializeField] public Color defaultColour = Color.white;

    // Determines what menu button is selected and colours that button accordingly
    public void InputButtonClicked(Button button)
    {
        if (selectedButton == button)
        {
            button.GetComponent<Image>().color = defaultColour;
            selectedButton = null;
        }
        else
        {
            if (selectedButton != null)
            {
                selectedButton.GetComponent<Image>().color = defaultColour;
            }
            button.GetComponent<Image>().color = selectedColour;
            selectedButton = button;
        }
    }
}
