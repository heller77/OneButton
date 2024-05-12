using DG.Tweening;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     特定のオブジェクトを追跡（トラッキング）させる
    /// </summary>
    public class TrackingTransformMono : MonoBehaviour
    {
        [SerializeField] private float positionSpeed = 5.0f;

        private readonly bool isUpdate = true;

        private Transform _trackingTarget;

        /// <summary>
        ///     最初の場所
        /// </summary>
        private Vector3 initPos;

        private void Start()
        {
            initPos = transform.localPosition;
        }

        private void Update()
        {
            if (_trackingTarget && isUpdate)
            {
                // 目標の位置に向かって線形補間する
                transform.position = Vector3.Lerp(transform.position, _trackingTarget.position,
                    positionSpeed * Time.deltaTime);
            }
        }

        public void Initialize(float duration)
        {
            _trackingTarget = null;
            transform.DOLocalMove(initPos, duration);
        }

        public void ChangeTracking(Transform trackingTarget, float duration)
        {
            _trackingTarget = trackingTarget;
            // this.transform.DOMove(this._trackingTarget.position, duration);
        }
    }
}