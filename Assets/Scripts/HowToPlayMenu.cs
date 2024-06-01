using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private GameObject settingsMenu;

    public void ReturnToMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }
}
