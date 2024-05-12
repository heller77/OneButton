using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

namespace Utils
{
    /// <summary>
    /// スプライン上を移動させるクラス
    /// </summary>
    [ExecuteAlways]
    public class MoverOnSpline : MonoBehaviour
    {
        [SerializeField] private SplineContainer spline;

        // 経路上の位置
        [SerializeField, Range(0.001f, 0.999f)]
        private float _t;

        [SerializeField] private bool isPlay = false;
        [SerializeField] private float completetime = 1.0f;

        private float splineLength = 0.0f;

        private void Start()
        {
            if (spline != null)
            {
                splineLength = spline.CalculateLength();
            }
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

        public float GetT()
        {
            return this._t;
        }

#endif

        public void MoveOnSpline()
        {
            // スプライン上の位置・向き・上ベクトルを取得
            if (spline is null || !spline.Evaluate(_t, out var position, out var tangent, out var upVector))
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

        /// <summary>
        /// 別のスプラインに乗る場合
        /// </summary>
        /// <param name="newsplienvalue"></param>
        /// <param name="new_spline"></param>
        /// <param name="movetime_tonewspline"></param>
        public void ChangeSpline(SplineContainer new_spline, float movetime_tonewspline)
        {
            this.spline = new_spline;
            var newsplienvalue =
                SplineUtility.GetNearestPoint(new_spline.Spline, this.transform.position, out var position, out var t);

            //移動方向をみル
            transform.DOLookAt(position, 0.1f);

            float duration = 1.0f;
            transform.DOMove(position, duration);
            transform.DOLookAt(position, 0.1f);
        }
    }
}