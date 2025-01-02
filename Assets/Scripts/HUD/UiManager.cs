using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject HUDGame;
    public GameObject HUDEndGame;
    public GameObject PauseMenu;
    public GameObject DeathScreen;


    public bool GameFinish = false;
    // private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        DeathScreen.SetActive(false);
        ShowUIGame();
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUDEndGame.activeSelf)
        {
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
                {
                    if (PauseMenu.activeSelf)
                    {
                        ShowUIGame();
                        ResumeGame();
                    }
                    else
                    {
                        ShowPauseMenu();
                        PauseGame();
                    }
                }
            }
        }
    }

    public void goBackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ShowUIGame()
    {
        HUDGame.SetActive(true);
        HUDEndGame.SetActive(false);
        PauseMenu.SetActive(false);
        ResumeGame();
    }

    public void ShowEndGame()
    {
        GameFinish = true;
        HUDEndGame.SetActive(true);
        HUDGame.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        HUDGame.SetActive(false);
        HUDEndGame.SetActive(false);
        PauseMenu.SetActive(true);
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Met le temps du jeu à 0 (pause)
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Rétablit le temps normal
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
    }
}
