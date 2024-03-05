using System;
using Character;
using Character.LockOns;
using Character.Weapon.Lasers;
using UnityEngine;

namespace GameLoops
{
    public class GameLoopMono : MonoBehaviour
    {
        [SerializeField] private EnemyDisplayWindow _displayWindow;
        [SerializeField] private LockOn _lockOn;
        [SerializeField] private Laser _laser;

        [SerializeField] private PlayerRobotManager _playerRobotManager;

        private void Start()
        {
            _displayWindow.Initialize();
            _lockOn.Initialize();

            _laser.Initialize();

            _playerRobotManager.Initialize();
        }
    }
}