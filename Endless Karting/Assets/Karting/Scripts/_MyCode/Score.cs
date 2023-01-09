using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KartGame.KartSystems;
using Abertay.Analytics;
using GameAnalyticsSDK;
using System;

namespace KartGame.UI
{
    public class Score : MonoBehaviour
    {
        public TextMeshProUGUI score;

        public bool AutoFindKart = true;
        public ArcadeKart KartController;

        private float currentScore;
        private bool scoreCounting = false;

        private float sessionHighscore = 0;

        [Header("Difficulty Thresholds")]
        public int[] scoreThresholds;
        public int[] scoreMultiplier;
        private int currentThreshold = 0;

        // Speed variables
        private float baseTopSpeed;
        private float topSpeed;
        private float baseAcceleration;
        private float acceleration;
        private float reverseTopSpeed;
        private float reverseAcceleration;

        [SerializeField] private float maxTopSpeedScore;
        private float speedIncreaseCof;

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
            sessionHighscore = PlayerPrefs.GetInt("sessionHighscore");

            if (AutoFindKart)
            {
                ArcadeKart kart = FindObjectOfType<ArcadeKart>();
                KartController = kart;
                baseTopSpeed = KartController.baseStats.TopSpeed;
                baseAcceleration = KartController.baseStats.Acceleration;
            }

            if (!KartController)
            {
                gameObject.SetActive(false);
            }
        }

        void StartScoreCounter()
        {
            scoreCounting = true;
            AnalyticsManager.GetGAInstance.SendDesignEvent("Score:Threshold_" + currentThreshold, 1);
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

            if (currentScore > sessionHighscore)
            {
                sessionHighscore = currentScore;
                PlayerPrefs.SetInt("sessionHighscore", Mathf.FloorToInt(sessionHighscore));
            }

            int finalScore = (int)currentScore;
            EventManager.TrackComplete(finalScore);
        }

        void Update()
        {
            float speed = KartController.Rigidbody.velocity.magnitude;
            
            // Updates current score
            if (scoreCounting)
                currentScore += Time.deltaTime * (speed / topSpeed) * scoreMultiplier[currentThreshold];

            if (currentScore < maxTopSpeedScore)
            {
                // Two constants for the speed increase calculation
                float constantFunction = 5f;
                float constantMultiplier = 40f;

                // Get percent of the way to maxTopSpeedScore
                speedIncreaseCof = (currentScore / maxTopSpeedScore);

                // Uses f(x) = x^1/2 + xc to get a gradual increase based off of score
                speedIncreaseCof = Mathf.Pow(speedIncreaseCof, 1f / 2f) + speedIncreaseCof * constantFunction;
                
                // Multiplies the f(x) by a multiplier to bring up the value
                speedIncreaseCof = speedIncreaseCof * constantMultiplier;

                topSpeed = baseTopSpeed + ((baseTopSpeed / 100) * speedIncreaseCof);
                acceleration = baseAcceleration + ((baseAcceleration / 100) * speedIncreaseCof);
                reverseTopSpeed = topSpeed / 2f;
                reverseAcceleration = acceleration;

                KartController.baseStats.TopSpeed = topSpeed;
                KartController.baseStats.Acceleration = acceleration;
                KartController.baseStats.ReverseSpeed = reverseTopSpeed;
                KartController.baseStats.ReverseAcceleration = reverseAcceleration;
            }

            // Displays score
            score.text = string.Format($"{Mathf.FloorToInt(currentScore):D5}");

            if (!scoreCounting)
                score.text += string.Format($"\n Highscore: {PlayerPrefs.GetInt("Highscore"):D5}");

            // Moves to the next threshold
            if (currentThreshold < scoreThresholds.Length - 1)
            {
                if (currentScore > scoreThresholds[currentThreshold])
                {
                    currentThreshold++;
                    EventManager.EventNextThreshold();

                    AnalyticsManager.GetGAInstance.SendDesignEvent("Score:Threshold_" + currentThreshold, 1);
                }
            }
        }
    }
}


