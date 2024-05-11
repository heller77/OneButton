using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    /// <summary>
    /// シーン遷移にを管理
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
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