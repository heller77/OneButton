using System;
using Character;
using Character.LockOns;
using Character.Weapon.Lasers;
using Enemys;
using UnityEngine;

namespace GameLoops
{
    public class GameLoopMono : MonoBehaviour
    {
        [SerializeField] private EnemyDisplayWindow _displayWindow;
        [SerializeField] private LockOn _lockOn;
        [SerializeField] private Laser _laser;

        [SerializeField] private PlayerRobotManager _playerRobotManager;

        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private PlayerRobotManager _robotManager;

        private void Start()
        {
            _displayWindow.Initialize();
            _lockOn.Initialize();

            _laser.Initialize();

            _playerRobotManager.Initialize();
        }

        private void Update()
        {
            _enemyManager.Tick();
            _robotManager.Tick();
        }
    }
}