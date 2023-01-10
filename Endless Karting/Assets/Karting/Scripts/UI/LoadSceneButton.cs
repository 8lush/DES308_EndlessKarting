using UnityEngine;
using UnityEngine.SceneManagement;
using Abertay.Analytics;
using GameAnalyticsSDK;
using UnityEngine.SocialPlatforms.Impl;

namespace KartGame.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [Tooltip("What is the name of the scene we want to load when clicking the button?")]
        public string SceneName;

        public void LoadTargetScene()
        {
            SceneManager.LoadSceneAsync(SceneName);
        }

        public void GameAnalyticsStartSession()
        {
            GameAnalyticsSDK.GameAnalytics.StartSession();
        }

        public void GameAnalyticsEndSession()
        {
            GameAnalyticsSDK.GameAnalytics.EndSession();
        }
    }
}
