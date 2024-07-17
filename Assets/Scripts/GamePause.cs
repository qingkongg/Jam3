using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    // In order to avoid "compiler error", notes will be given in English. 
    // Confirmed: Axis "Cancel" = ESC (by default), Input.GetAxis("Cancel") has value 0 OR 1 (Integer)
    // Define boolean "paused" (if ESC has been pressed)
    public bool paused = false;
    // No fade in/out wanted. 
    // public float fadeDuration
    // Define alpha value of pausing UI
    public float alphaOfPausingPicture = 0.75f;
    // Define boolean "stateAlreadyChanged" (to avoid ESC being scanned when being hold)
    bool stateAlreadyChanged = false;
    public CanvasGroup pauseBackgroundImageCanvasGroup;
    // Scan if button ESC is pressed in every frame, let paused = Input.GetAxis("Cancel") as a boolean
    void Update()
    {
        // float escapeButton = Input.GetAxis("Cancel");
        // Debug.Log(escapeButton);
        if (Input.GetAxis("Cancel") == 1 && stateAlreadyChanged == false)
        {
            if (paused)
            {
                paused = false;
                Debug.Log("Unpaused");
            }
            else
            {
                paused = true;
                Debug.Log("Paused");
            }
            stateAlreadyChanged = true;
        }
        if (Input.GetAxis("Cancel") == 0)
        {
            stateAlreadyChanged = false;
        }
        /*
         * Anything contains "while" loop causes crashes
        if (Input.GetAxis("Cancel") == 1)
        {
            paused = true;
            while (Input.GetAxis("Cancel") == 1)
            {

            }
        }
        else
        {
            paused = false;
        }
        */
        /*
         * Causes Infinite loop (Unity becomes static)
        if (paused == false)
        {
            paused = true;
            escPressed = true;
            while (escPressed)
            {
                Debug.Log("ESC held");
                Debug.Log(Input.GetAxis("Cancel"));
                if (escapeButton < 0.5)
                {
                    escPressed = false;
                }
            }
            Debug.Log("ESC released");
            Debug.Log(Input.GetAxis("Cancel"));
        }
        else
        {
            paused = false;
            escPressed = true;
            while (escPressed)
            {
                if (escapeButton < 0.5)
                {
                    escPressed = false;
                }
            }
        }
        */
        if (paused)
        {
            PauseLevel(pauseBackgroundImageCanvasGroup, alphaOfPausingPicture);
            if (Input.GetAxis("Submit") == 1)
            {
                Debug.Log("Quit game");
                Application.Quit();
            }
        }
        else
        {
            PauseLevel(pauseBackgroundImageCanvasGroup, 0);
        }
    }
    void PauseLevel(CanvasGroup imageCanvasGroup, float alphaX)
    {
        imageCanvasGroup.alpha = alphaX;
    }
}
/*
 * Template for other scripts.cs
 * Put the lines below in public class *** : MonoBehaviour {}
public bool paused = false;
bool stateAlreadyChanged = false;
 * Put the lines below in Update {}
        if (Input.GetAxis("Cancel") == 1 && stateAlreadyChanged == false)
    {
        if (paused)
        {
            paused = false;
        }
        else
        {
            paused = true;
        }
        stateAlreadyChanged = true;
    }
    if (Input.GetAxis("Cancel") == 0)
    {
        stateAlreadyChanged = false;
    }
*/
