using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject InGameUI;
    public GameObject SettingsUI;

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuUI.activeSelf == true)
            GameIsPaused = true;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(false);
        InGameUI.SetActive(true);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        InGameUI.SetActive(false);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("UIScene");
    }
}
