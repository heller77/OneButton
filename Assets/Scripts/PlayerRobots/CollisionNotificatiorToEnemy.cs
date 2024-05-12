using Enemys;
using UnityEngine;

namespace Character
{
    /// <summary>
    ///     Detectorコンポーネントを持つオブジェクトと衝突したら、衝突を通知するオブジェクト
    /// </summary>
    public class CollisionNotificatiorToEnemy : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Detector>(out var detector))
            {
                detector.Detect();
            }
        }
    }
}