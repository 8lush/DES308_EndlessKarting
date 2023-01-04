using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenScore : MonoBehaviour
{
    public TextMeshProUGUI score;

    public GameObject newHighscore;

    void Start()
    {
        score.text = string.Format($"{PlayerPrefs.GetInt("LastScore"):D5}\n Highscore: {PlayerPrefs.GetInt("Highscore"):D5}");

   

        if (PlayerPrefs.GetInt("NewHighscore") == 1)
        {
            PlayerPrefs.SetInt("NewHighscore", 0);

            newHighscore.SetActive(true);
        }
    }
}
