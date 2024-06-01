using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private GameObject settingsMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void HowToPlay()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
