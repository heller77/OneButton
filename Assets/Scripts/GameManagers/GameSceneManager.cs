using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    public class GameSceneManager : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Mainシーンに遷移
        /// </summary>
        public void GoToInGameScene()
        {
            SceneManager.LoadScene("Main");
        }
    }
}