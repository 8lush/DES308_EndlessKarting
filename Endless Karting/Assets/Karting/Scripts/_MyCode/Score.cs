using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KartGame.KartSystems;

namespace KartGame.UI
{
    public class Score : MonoBehaviour
    {
        public TextMeshProUGUI score;

        public bool AutoFindKart = true;
        public ArcadeKart KartController;

        private float currentScore;
        private bool scoreCounting = false;

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

            int finalScore = (int)currentScore;
            EventManager.TrackComplete(finalScore);
        }

        void Update()
        {
            float speed = KartController.Rigidbody.velocity.magnitude;
            
            // Updates current score
            if (scoreCounting)
                currentScore += Time.deltaTime * (speed / topSpeed) * scoreMultiplier[currentThreshold];

            // Gradual speed increase
            topSpeed = baseTopSpeed + ((baseTopSpeed / 100) * (currentScore / 20));
            acceleration = baseAcceleration + ((baseAcceleration / 100) * (currentScore / 20));
            reverseTopSpeed = topSpeed / 2f;
            reverseAcceleration = acceleration;

            KartController.baseStats.TopSpeed = topSpeed;
            KartController.baseStats.Acceleration = acceleration;
            KartController.baseStats.ReverseSpeed = reverseTopSpeed;
            KartController.baseStats.ReverseAcceleration = reverseAcceleration;

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
                    Debug.Log(currentThreshold);
                    EventManager.EventNextThreshold();

                    Debug.Log(topSpeed);
                    Debug.Log(acceleration);
                }
            }
        }
    }
}


