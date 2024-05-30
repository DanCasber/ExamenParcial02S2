using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private GameObject[] allCoins;

    void Start()
    {
        scoreText.text = "00";

        allCoins = GameObject.FindGameObjectsWithTag("Coin");
        PlayerPrefs.SetInt("TotalCoins", allCoins.Length);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            currentScore++;
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        scoreText.text = currentScore.ToString("D2");

        PlayerPrefs.SetInt("CollectedCoins", currentScore);
        PlayerPrefs.Save();
    }
}
