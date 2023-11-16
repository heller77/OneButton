﻿using System;
using Character.LockOns;
using Enemys;
using UnityEngine;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("playerrobot space");
                var targetenemy = _lockOn.GetTarget();
                if (targetenemy)
                {
                    
                    _playerAttackComponent.Attack(targetenemy, 3);
                }
            }
        }
    }
}