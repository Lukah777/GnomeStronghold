using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// MainMenu.cs
// Josiah Nistor
// Handels user UI input to navigate main menu
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_howToPlay;

    // Play button, loads the game scene
    public void Play() { SceneManager.LoadScene("GS_DE", LoadSceneMode.Single); }

    // How to Play button toggle how to play text, hides menu
    public void HTP()
    {
        m_mainMenu.SetActive(false);
        m_howToPlay.SetActive(true);
    }

    // Quit button closes the application
    public void Quit() { Application.Quit(); }

    // Back button hides the how to play, shows the menu
    public void Back()
    {
        m_mainMenu.SetActive(true);
        m_howToPlay.SetActive(false);
    }
}
