using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    public class GameSceneManager : MonoBehaviour
    {
        private void Start()
        {
            // DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Mainシーンに遷移
        /// 返り値のAsyncOperation.allowSceneActivationをtrueにする
        /// </summary>
        public AsyncOperation GoToInGameScene()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("Main");
            asyncOperation.allowSceneActivation = false;
            return asyncOperation;
        }
    }
}