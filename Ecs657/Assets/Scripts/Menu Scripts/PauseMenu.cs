using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private bool gameIsPaused = false;
    private bool upgrading = false;
    public GameObject pauseMenuUI;
    public GameObject crosshair;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //freezes time, activates pauseMenu and deactivates crosshair 
    void Pause()
    {
        if (Time.timeScale == 0f)
        {
            upgrading = true;
        }
        else
        {
            upgrading = false;
            crosshair.SetActive(false);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }

    //changes scene to menu scene
    public void MainMenuButton()
    {
        Save();
        pauseMenuUI.SetActive(false);
        crosshair.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Destroy(player);
        SceneManager.LoadScene(0);
    }

    //Resumes time, dactivates pauseMenu and activates crosshair 
    public void Resume()
    {
        if(!upgrading)
        {
            Cursor.lockState = CursorLockMode.Locked;
            crosshair.SetActive(true);
            Time.timeScale = 1f;
        }
        
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }
    
    public void Save()
    {
        //to be done later
    }

    //closes application
    public void QuitButton()
    {
        Save();
        Time.timeScale = 1f;
        Application.Quit();
    }


}
