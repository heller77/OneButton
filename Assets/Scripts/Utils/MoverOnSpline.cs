using System;
using UnityEngine;
using UnityEngine.Splines;

namespace Utils
{
    [ExecuteAlways]
    public class MoverOnSpline : MonoBehaviour
    {
        [SerializeField] private SplineContainer spline;

        // 経路上の位置
        [SerializeField, Range(0, 1)] private float _t;
        [SerializeField] private bool isPlay = false;
        [SerializeField] private float completetime = 1.0f;

        private float splineLength = 0.0f;

        private void Start()
        {
            splineLength = spline.CalculateLength();
        }

        private void Update()
        {
            if (isPlay)
            {
                // _t += Time.deltaTime / completetime;
                MoveOnSpline();
            }
        }
#if UNITY_EDITOR

        // インスペクターから編集されたとき
        private void OnValidate()
        {
            MoveOnSpline();
        }

#endif

        public void MoveOnSpline()
        {
            // スプライン上の位置・向き・上ベクトルを取得
            if (!spline.Evaluate(_t, out var position, out var tangent, out var upVector))
                return;
            // Transformに反映
            transform.SetPositionAndRotation(
                position,
                Quaternion.LookRotation(tangent, upVector)
            );
        }

        public void Play()
        {
            isPlay = true;
        }

        public void Pause()
        {
            isPlay = false;
        }
    }
}