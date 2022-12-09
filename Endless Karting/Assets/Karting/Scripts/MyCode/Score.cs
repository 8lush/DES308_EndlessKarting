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

        // Update is called once per frame
        void Update()
        {
            float speed = KartController.Rigidbody.velocity.magnitude;
            float topSpeed = KartController.TopSpeedArray[currentThreshold];

            if (scoreCounting)
                currentScore += Time.deltaTime * (speed / topSpeed) * scoreMultiplier[currentThreshold];
            //currentScore += Time.deltaTime * scoreMultiplier[currentThreshold];

            score.text = string.Format($"{Mathf.FloorToInt(currentScore):D7}");

            if (!scoreCounting)
                score.text += string.Format($"\n Highscore: {PlayerPrefs.GetInt("Highscore"):D7}");

            if (currentScore > scoreThresholds[currentThreshold])
            {
                if (currentThreshold < scoreThresholds.Length - 1)
                {
                    currentThreshold++;
                    EventManager.EventNextThreshold();
                }
            }
        }
    }
}


