using System;
using GameManagers;
using MyInputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TitleScene
{
    public class TitleSceneManager : MonoBehaviour
    {
        private GameInputs input;

        [SerializeField] private GameSceneManager _gameSceneManager;

        private void Start()
        {
            input = new GameInputs();
            input.Player.PushButton.started += GoToInGame;
            input.Enable();
        }

        /// <summary>
        /// インゲームを
        /// </summary>
        /// <param name="obj"></param>
        private void GoToInGame(InputAction.CallbackContext obj)
        {
            Debug.Log("gotogame");
            _gameSceneManager.GoToInGameScene();
            input.Disable();
        }
    }
}