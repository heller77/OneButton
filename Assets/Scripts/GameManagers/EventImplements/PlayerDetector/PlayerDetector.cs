using Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameManagers.EventImplements.PlayerDetector
{
    /// <summary>
    /// プレイヤーを検知する
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        private bool isDetect;

        public async UniTask WaitDetect()
        {
            await UniTask.WaitUntil(() => isDetect);
        }

        public void Detect()
        {
            isDetect = true;
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