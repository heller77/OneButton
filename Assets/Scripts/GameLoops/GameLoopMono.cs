using Character;
using Character.LockOns;
using Character.Weapon.Lasers;
using Enemys;
using UnityEngine;

namespace GameLoops
{
    /// <summary>
    ///     明示的にゲーム開始時と毎フレーム関数を呼ぶクラス
    ///     unityのstartやupdateだと処理の順序が分かりにくいので、順序に意味がある物を管理
    /// </summary>
    public class GameLoopMono : MonoBehaviour
    {
        [SerializeField] private EnemyDisplayWindow _displayWindow;
        [SerializeField] private LockOn _lockOn;
        [SerializeField] private Laser _laser;

        [SerializeField] private PlayerRobotManager _playerRobotManager;

        [SerializeField] private EnemyManager _enemyManager;

        private void Start()
        {
            _displayWindow.Initialize();
            _lockOn.Initialize();

            _laser.Initialize();
        }

        private void Update()
        {
            _enemyManager.Tick();
            _playerRobotManager.Tick();
        }
    }
}