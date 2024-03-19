using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FittsButtonHandler : MonoBehaviour
{
    [SerializeField] Button leftButton;
    [SerializeField] Button middleButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button errorButton;

    Button currentButton;
    Button previousButton;

    [SerializeField] Color activeColour;
    [SerializeField] Color defaultColour = Color.white;

    [SerializeField] int totalClicks;
    [SerializeField] int errorClicks;
    [SerializeField] int successfulClicks;

    [SerializeField] int distance;
    [SerializeField] int size;

    private Camera camera;

    void Start()
    {
        currentButton = middleButton;
        previousButton = leftButton;
        currentButton.GetComponent<Image>().color = activeColour;

        camera = Camera.main;

        ChangeDistances();
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
    }

    public void GoToNextButton(Button button)
    {
        if (currentButton == button)
            successfulClicks++;
        else errorClicks++;

        totalClicks++;

        if (currentButton == leftButton || currentButton == rightButton)
        {
            previousButton = currentButton;
            currentButton = middleButton;
        }
        else if (previousButton == leftButton)
        {
            previousButton = currentButton;
            currentButton = rightButton;
        }
        else if (previousButton == rightButton)
        {
            previousButton = currentButton;
            currentButton = leftButton;
        }
        

        previousButton.GetComponent<Image>().color = defaultColour;
        currentButton.GetComponent<Image>().color = activeColour;        
    }

    void ChangeDistances()
    {
        //leftButton.transform.position = middleButton.transform.position + camera.ScreenToWorldPoint(new Vector3(distance, middleButton.transform.position.y, 0));
        leftButton.transform.position = new Vector3(camera.ScreenToWorldPoint(new Vector3(Screen.width / 2 - distance, 0, 0)).x, 0, 0);
        rightButton.transform.position = new Vector3(camera.ScreenToWorldPoint(new Vector3(Screen.width / 2 + distance, 0, 0)).x, 0, 0);
    }
}
