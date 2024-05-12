using Character;
using R3;
using UnityEngine;

namespace GameManagers.EventImplements.PlayerDetector
{
    /// <summary>
    ///     プレイヤーを検知する
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        private readonly Subject<Unit> _playerdetect = new Subject<Unit>();

        /// <summary>
        ///     プレイヤーが来たら発火する
        /// </summary>
        public Observable<Unit> playerDetect
        {
            get { return _playerdetect; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerRobotManager>(out var player))
            {
                Detect();
            }
        }

        public void Detect()
        {
            _playerdetect.OnNext(Unit.Default);
        }
    }
}