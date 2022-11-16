using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;

    private float currentScore;
    private bool scoreCounting = false;

    public float scoreMultiplier = 5f;


    void OnEnable()
    {
        EventManager.StartTrack += StartScoreCounter;
        EventManager.EndTrack += StopScoreCounter;
    }

    void OnDisable()
    {
        EventManager.StartTrack -= StartScoreCounter;
        EventManager.EndTrack -= StopScoreCounter;
    }

    void Start()
    {
        currentScore = 0f;
        PlayerPrefs.SetInt("LastScore", 0);
    }

    void StartScoreCounter()
    {
        scoreCounting = true;
    }

    void StopScoreCounter()
    {
        scoreCounting = false;
        PlayerPrefs.SetInt("LastScore", Mathf.FloorToInt(currentScore));

        if (currentScore > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Mathf.FloorToInt(currentScore));
            PlayerPrefs.SetInt("NewHighscore", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreCounting)
        currentScore += Time.deltaTime * scoreMultiplier;

        score.text = string.Format($"{Mathf.FloorToInt(currentScore):D7}");

        if(!scoreCounting)
        score.text += string.Format($"\n Highscore: {PlayerPrefs.GetInt("Highscore"):D7}");
    }
}
