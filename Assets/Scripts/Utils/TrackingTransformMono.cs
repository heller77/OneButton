using System;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class TrackingTransformMono : MonoBehaviour
    {
        /// <summary>
        /// 最初の場所
        /// </summary>
        private Vector3 initPos;

        private Transform _trackingTarget;

        [SerializeField] private float positionSpeed = 5.0f;
        private bool isUpdate = true;

        private void Start()
        {
            initPos = transform.localPosition;
        }

        public void Initialize(float duration)
        {
            _trackingTarget = null;
            transform.DOLocalMove(initPos, duration);
        }

        public void ChangeTracking(Transform trackingTarget, float duration)
        {
            this._trackingTarget = trackingTarget;
            // this.transform.DOMove(this._trackingTarget.position, duration);
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
    }
}