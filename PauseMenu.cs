using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = true;

    public GameObject pauseMenuUI;
    public GameObject instructionsUI;
    public GameObject gamePresentationUI;

    void Start()
    {
        gamePresentationUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InstructionBoxOff()
    {
        instructionsUI.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void PresentationBoxOff()
    {
        gamePresentationUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = true;
    }
}
