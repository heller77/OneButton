using Character;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace GameManagers.EventImplements.PlayerDetector
{
    /// <summary>
    /// プレイヤーを検知する
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        private Subject<Unit> _playerdetect = new Subject<Unit>();

        /// <summary>
        /// プレイヤーが来たら発火する
        /// </summary>
        public R3.Observable<Unit> playerDetect
        {
            get { return this._playerdetect; }
        }

        public void Detect()
        {
            _playerdetect.OnNext(Unit.Default);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerRobotManager>(out var player))
            {
                Detect();
            }
        }
    }
}