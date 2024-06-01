using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText = null;
    [SerializeField] private AudioSource musicBG;

    float maxVolume = 100.0f;
    int volumeValue = 100;

    private void Start()
    {
        int volume = PlayerPrefs.GetInt("volume");
        float volumeFloatValue = (float)volume / 100.0f;
        volumeSlider.value = volumeFloatValue;
        volumeValue = Mathf.RoundToInt(volumeFloatValue * maxVolume);
        volumeText.text = volumeValue.ToString();
    }

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
        howToPlayMenu.SetActive(false);
    }
}
