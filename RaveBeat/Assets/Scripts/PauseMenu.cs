using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static float pauseTime = 0.0f;
    private float pausedTime = 0.0f;

    public GameObject pauseMenuUi;
    public GameObject HUDMenuUi;

    public GameObject resumeHover;
    public GameObject restartHover;
    public GameObject quitHover;

    private bool resumeHovered;
    private bool restartHovered;
    private bool quitHovered;

    private bool axisInUse;


    private void Start()
    {
        resumeHovered = true;
        quitHovered = false;
        restartHovered = false;
        axisInUse = false;
    }


    void Update() 
    {
        if (Input.GetButtonDown("Start"))
        {
            if (!gameIsPaused)
            {
                Pause();
            }
        }

        if(gameIsPaused)
        {
            if (resumeHovered)
            {
                if (Input.GetButtonDown("PauseMenuSelect"))
                {
                    Resume();
                }

                if (Input.GetAxis("LeftVertical") == 1)
                {
                    if (!axisInUse)
                    {
                        resumeHover.SetActive(false);
                        quitHover.SetActive(true);
                        resumeHovered = false;
                        quitHovered = true;
                        axisInUse = true;
                    }
                }

                if (Input.GetAxis("LeftVertical") == -1)
                {
                    if (!axisInUse)
                    {
                        resumeHover.SetActive(false);
                        restartHover.SetActive(true);
                        resumeHovered = false;
                        restartHovered = true;
                        axisInUse = true;
                    }
                }

                if (Input.GetAxis("LeftVertical") == 0)
                {
                    axisInUse = false;
                }
            }

            if (restartHovered)
            {
                if (Input.GetButtonDown("PauseMenuSelect"))
                {
                    Restart();
                }

                if (Input.GetAxis("LeftVertical") == 1)
                {
                    if (!axisInUse)
                    {
                        restartHover.SetActive(false);
                        resumeHover.SetActive(true);
                        restartHovered = false;
                        resumeHovered = true;
                        axisInUse = true;
                    }

                }

                if (Input.GetAxis("LeftVertical") == -1)
                {
                    if (!axisInUse)
                    {
                        restartHover.SetActive(false);
                        quitHover.SetActive(true);
                        restartHovered = false;
                        quitHovered = true;
                        axisInUse = true;
                    }

                }

                if (Input.GetAxis("LeftVertical") == 0)
                {
                    axisInUse = false;
                }

            }

            if (quitHovered)
            {
                if (Input.GetButtonDown("PauseMenuSelect"))
                {
                    Quit();
                }

                if (Input.GetAxis("LeftVertical") == 1)
                {
                    if (!axisInUse)
                    {
                        quitHover.SetActive(false);
                        restartHover.SetActive(true);
                        quitHovered = false;
                        restartHovered = true;
                        axisInUse = true;
                    }

                }

                if (Input.GetAxis("LeftVertical") == -1)
                {
                    if (!axisInUse)
                    {
                        quitHover.SetActive(false);
                        resumeHover.SetActive(true);
                        quitHovered = false;
                        resumeHovered = true;
                        axisInUse = true;
                    }

                }

                if (Input.GetAxis("LeftVertical") == 0)
                {
                    axisInUse = false;
                }
            }
        }

    }


    void Pause() 
    {
        pausedTime = (float)AudioSettings.dspTime;
        HUDMenuUi.SetActive(false);
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    void Resume() 
    {
        pauseTime = (float)(AudioSettings.dspTime - pausedTime);
        pauseMenuUi.SetActive(false);
        HUDMenuUi.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Restart()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseMenuUi.SetActive(false);
        HUDMenuUi.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Quit()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseMenuUi.SetActive(false);
        HUDMenuUi.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
