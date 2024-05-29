using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private TextMeshProUGUI volumeText = null;
    [SerializeField] private AudioSource musicBG;

    float maxVolume = 100.0f;
    int volumeValue = 100;

    public void AdjustVolume(float value)
    {
        volumeValue = Mathf.RoundToInt(value * maxVolume);
        volumeText.text = volumeValue.ToString();
        musicBG.volume = (float)volumeValue / 100.0f;
    }

    public void SaveAndReturn()
    {
        PlayerPrefs.SetInt("volume", volumeValue);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
