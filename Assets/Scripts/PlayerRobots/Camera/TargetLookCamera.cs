using System;
using UnityEngine;

namespace Character.Camera
{
    [ExecuteInEditMode]
    public class TargetLookCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        /// <summary>
        /// プレイヤーのルートを指定して上ベクトルをローカルで固定する
        /// </summary>
        [SerializeField] private Transform parenttransform;

        private void Update()
        {
            transform.LookAt(target, parenttransform.up);
        }
    }
}