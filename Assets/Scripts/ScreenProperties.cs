using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenProperties : MonoBehaviour
{
    private void Awake()
    {
        // Initializes the screen properties to 1920 x 1080 pixels with Fullscreen
        Screen.SetResolution(1920, 1080, true);

    }
}
