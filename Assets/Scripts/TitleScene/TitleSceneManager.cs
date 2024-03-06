using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagers;
using MyInputs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TitleScene
{
    /// <summary>
    /// タイトルシーンの管理をする
    /// </summary>
    public class TitleSceneManager : MonoBehaviour
    {
        private GameInputs input;

        [SerializeField] private GameSceneManager _gameSceneManager;

        [SerializeField] private Image blackScreen;
        [SerializeField] private GameObject loadCircle;
        [SerializeField] private Material loadingCircleMaterial;
        private readonly string _materialPropertyGauge = "_gauge";
        private float _gaugeStart = 1.6f;
        private float _gaugeEnd = 0.5f;

        //ローディング画面でのFade時間
        private readonly float _blackscreenFadeTime = 1;
        readonly float _loadingGaugeAnimationTime = 5;

        private void Start()
        {
            input = new GameInputs();
            input.Player.PushButton.started += GoToInGame;
            input.Enable();
        }

        private void GoToInGame(InputAction.CallbackContext obj)
        {
            GoToInGame();
        }

        /// <summary>
        /// インゲームを
        /// </summary>
        private async UniTaskVoid GoToInGame()
        {
            Debug.Log("gotogame");
            var asyncOperation = _gameSceneManager.GoToInGameScene();
            input.Disable();
            input.Dispose();


            loadingCircleMaterial.SetFloat(_materialPropertyGauge, _gaugeStart);


            await blackScreen.DOFade(endValue: 1f, duration: _blackscreenFadeTime);
            loadCircle.SetActive(true);
            await this.loadingCircleMaterial.DOFloat(_gaugeEnd, _materialPropertyGauge,
                _loadingGaugeAnimationTime);

            asyncOperation.allowSceneActivation = true;
        }
    }
}