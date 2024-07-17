using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscQuit : MonoBehaviour
{
    public GameObject QuitButton;
    public GameObject ResumeButton;
    public bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        QuitButton.SetActive(false);
        ResumeButton.SetActive(false);
    }
    public void QuitEvent()
    {
        Application.Quit();
    }
    public void ResumeEvent()
    {
        isPause = false;
        QuitButton.SetActive(false);
        ResumeButton.SetActive(false);
        Time.timeScale = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            isPause = true;
            QuitButton.SetActive(true);
            ResumeButton.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
