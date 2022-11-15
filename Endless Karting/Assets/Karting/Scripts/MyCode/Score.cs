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
    }

    void OnDisable()
    {
        EventManager.StartTrack -= StartScoreCounter;
    }

    void Start()
    {
        currentScore = 0f;
    }

    void StartScoreCounter()
    {
        scoreCounting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreCounting)
        currentScore += Time.deltaTime * scoreMultiplier;

        score.text = string.Format($"{Mathf.FloorToInt(currentScore):D7}");
    }
}
