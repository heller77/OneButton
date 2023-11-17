using System;
using Character.LockOns;
using Enemys;
using MyInputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using Utils;

namespace Character
{
    public class PlayerRobotManager : MonoBehaviour
    {
        // [SerializeField] private SplineAnimate _splineAnimate;
        [SerializeField] private MoverOnSpline _moverOnSpline;
        [SerializeField] private PlayerAttackComponent _playerAttackComponent;

        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private LockOn _lockOn;

        /// <summary>
        /// 動きをスタート
        /// </summary>
        public void StartMove()
        {
            Debug.Log("start move");
            _moverOnSpline.Play();
        }

        /// <summary>
        /// 動きを止める
        /// </summary>
        public void StopMove()
        {
            _moverOnSpline.Pause();
        }

        private MyInputs.MyInputMap input;

        private void Start()
        {
            input = new MyInputMap();
            input.Player.PushButton.started += Attack;
            input.Enable();
        }

        private void Attack(InputAction.CallbackContext callbackContext)
        {
            var targetenemy = _lockOn.GetTarget();
            if (targetenemy)
            {
                _playerAttackComponent.Attack(targetenemy, 3);
            }
        }
    }
}