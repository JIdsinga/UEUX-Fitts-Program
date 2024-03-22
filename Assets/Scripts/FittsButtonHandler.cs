using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FittsButtonHandler : MonoBehaviour
{
    [SerializeField] Button leftButton;
    [SerializeField] Button middleButton;
    [SerializeField] Button rightButton;
    [SerializeField] public Button errorButton;

    Button currentButton;
    Button previousButton;

    [SerializeField] Color activeColour;
    [SerializeField] Color defaultColour = Color.white;

    [SerializeField] int totalClicks = 0;
    [SerializeField] int errorClicks = 0;
    [SerializeField] int successfulClicks = 0;
    bool successfulClick;

    [SerializeField] int distance = 400;
    [SerializeField] int size = 50;

    public List<DataEntry> dataEntries = new List<DataEntry>();

    RectTransform leftRect, middleRect, rightRect;

    public bool hasEnded, isTutorial;

    [SerializeField] GameObject experimentMenu;
    [SerializeField] GameObject fibbsMenu;
    [SerializeField] GameObject endingMenu;
    [SerializeField] InputButtonHandler inputButtonHandler;
    [SerializeField] FileIO fileIOSystem;

    public float time;

    private Camera camera;

    // Runs before first frame, Used for data initialization and default set-up
    void Start()
    {
        currentButton = middleButton;
        previousButton = leftButton;
        currentButton.GetComponent<Image>().color = activeColour;

        leftRect = leftButton.GetComponent<RectTransform>();
        middleRect = middleButton.GetComponent<RectTransform>();
        rightRect = rightButton.GetComponent<RectTransform>();

        camera = Camera.main;

        ChangeWidth();
        ChangeDistances();
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
    }

    // Increments time every frame
    void Update()
    {
        time += Time.deltaTime;
    }

    // Functionality for going to the next button
    public void GoToNextButton(Button button)
    {
        UpdateClickData(button);

        // Determines what the next button is
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

        // Adds a data entry if not in the tutorial
        if(!isTutorial)
            dataEntries.Add(new DataEntry(fileIOSystem.GetSubjectID(fileIOSystem.dir), inputButtonHandler.selectedButton.GetComponentInChildren<TMP_Text>().text, totalClicks, size, distance, CalculateIndexOfDifficulty(size, distance), time, successfulClick));

        // Sets the colours of the buttons correctly
        previousButton.GetComponent<Image>().color = defaultColour;
        currentButton.GetComponent<Image>().color = activeColour;
        
        ResetTimer();

        UpdateSizeAndDistance(totalClicks);
    }
    
    // Updates data for each click
    private void UpdateClickData(Button button)
    {
        if (currentButton == button)
        {
            successfulClicks++;
            successfulClick = true;
        }
        else
        {
            errorClicks++;
            successfulClick = false;
        }

        totalClicks++;
    }

    float CalculateIndexOfDifficulty(int size, int distance)
    {
        return Mathf.Log((distance / size) + 1, 2);
    }

    // Resets the timer to 0
    public void ResetTimer()
    {
        time = 0;
    }

    // Sets click data to 0
    public void ResetClickData()
    {
        successfulClicks = 0;
        errorClicks = 0;
        totalClicks = 0;
    }
    // Resets the distance and size of buttons to default values
    public void ResetSizeAndDistance()
    {
        distance = 400;
        size = 50;
        ChangeDistances();
        ChangeWidth();
    }

    // Swaps the tutorial state
    public void TutorialActive(bool tutorial) => isTutorial = tutorial;

    // Updates the size and distance of buttons depending on the current click
    void UpdateSizeAndDistance(int clicks)
    {
        if (isTutorial)
        {
            if(clicks == 12)
            {                
                isTutorial = false;
                fibbsMenu.SetActive(false);
                experimentMenu.SetActive(true);
            }
        }
        else
        {
            switch (clicks)
            {
                case 12:
                    size = 100;
                    ChangeWidth();
                    break;
                case 24:
                    size = 25;
                    ChangeWidth();
                    break;
                case 36:
                    size = 50;
                    ChangeWidth();
                    distance = 800;
                    ChangeDistances();
                    break;
                case 48:
                    size = 100;
                    ChangeWidth();
                    break;
                case 60:
                    size = 25;
                    ChangeWidth();
                    break;
                case 72:
                    fibbsMenu.SetActive(false);
                    endingMenu.SetActive(true);
                    //fileIOSystem.CreateFile();
                    fileIOSystem.WriteToFile();
                    dataEntries.Clear();
                    break;
                default:
                    break;
            }
        }
    } 

    // Changes the distances between buttons
    void ChangeDistances()
    {
        leftButton.transform.position = new Vector3(camera.ScreenToWorldPoint(new Vector3(Screen.width / 2 - distance, 0, 0)).x, 0, 0);
        rightButton.transform.position = new Vector3(camera.ScreenToWorldPoint(new Vector3(Screen.width / 2 + distance, 0, 0)).x, 0, 0);
    }
    // Changes the width of the buttons
    void ChangeWidth()
    {
        Vector2 newWidth = new Vector2(size, middleRect.sizeDelta.y);
        leftRect.sizeDelta = newWidth;
        middleRect.sizeDelta = newWidth;
        rightRect.sizeDelta = newWidth;
    }
}

// Struct to carry data for each entry
public struct DataEntry
{
    public int subjectID;
    public string inputType;
    public int currentClicks;
    public int buttonWidth;
    public int buttonDistance;
    public float indexOfDifficulty;
    public float timeTaken;
    public bool successfulPress;

    public DataEntry(int subjectID, string inputType, int currentClicks, int buttonWidth, int buttonDistance, float indexOfDifficulty, float timeTaken, bool successfulPress)
    {
        this.subjectID = subjectID;
        this.inputType = inputType;
        this.currentClicks = currentClicks;
        this.buttonWidth = buttonWidth;
        this.buttonDistance = buttonDistance;
        this.indexOfDifficulty = indexOfDifficulty;
        this.timeTaken = timeTaken;
        this.successfulPress = successfulPress;
    }
}
