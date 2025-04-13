using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject m_pauseMenu;

    // Check if the player hits escape then pause the game
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            m_pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Resume button, hides the pause menu and unpauses the game
    public void Resume()
    {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Menu button, loads the menu scene and unpauses the game
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    // Quit button closes the application
    public void Quit() { Application.Quit(); }
}
