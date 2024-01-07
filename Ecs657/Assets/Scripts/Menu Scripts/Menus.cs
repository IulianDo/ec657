using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{

    private bool gameIsPaused = false;
    private bool gameIsOver = false;
    private bool upgrading = false;
    private bool hasWon = false;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public GameObject pauseMenuUI;
    public GameObject spellMenuUI;
    public GameObject crosshair;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameIsOver && !hasWon)
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

    //freezes gameplay
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

    //goes to main menu scene
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

    //make player start from beggining of the game
    public void Restart()
    {  
        Cursor.lockState = CursorLockMode.Locked;
        gameOverUI.SetActive(false);
        crosshair.SetActive(true);
        Time.timeScale = 1f;
        hasWon = false;
        gameIsOver = false;
        SceneManager.LoadScene(1);   //SceneManager.GetActiveScene().buildIndex);
    }

    //displays gameOver when player dies
    public void GameOver()
    {  
        Cursor.lockState = CursorLockMode.None;
        crosshair.SetActive(false);
        gameOverUI.SetActive(true);
        Destroy(player);
        Time.timeScale = 0f;
        gameIsOver = true;
    }

    //displays victory scree when player dies
    public void Win()
    {  
        Cursor.lockState = CursorLockMode.None;
        crosshair.SetActive(false);
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
        hasWon = true;
    }

    //resumes gameplay
    public void Resume()
    {
        
        if(!upgrading)
        {
            Cursor.lockState = CursorLockMode.Locked;
            crosshair.SetActive(true);
            Time.timeScale = 1f;
        }
        spellMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;

    }
    
    public void Save()
    {

    }

    public void QuitButton()
    {
        Save();
        Time.timeScale = 1f;
        Application.Quit();
    }


}
