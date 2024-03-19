using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenProperties : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
