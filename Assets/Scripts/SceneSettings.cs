using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    [SerializeField] private AudioSource musicBG;

    void Start()
    {
        int volume = PlayerPrefs.GetInt("volume");
        musicBG.volume = (float)volume / 100.0f;
    }

    void Update()
    {
        
    }
}
