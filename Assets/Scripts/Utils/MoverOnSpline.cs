using UnityEngine;
using UnityEngine.Splines;

namespace Utils
{
    public class MoverOnSpline : MonoBehaviour
    {
        [SerializeField] private SplineContainer spline;

        // 経路上の位置
        [SerializeField, Range(0, 1)] private float _t;

        private void Update()
        {
        }
#if UNITY_EDITOR

        // インスペクターから編集されたとき
        private void OnValidate()
        {
            MoveOnSpline();
        }

#endif

        private void MoveOnSpline()
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
    }
}