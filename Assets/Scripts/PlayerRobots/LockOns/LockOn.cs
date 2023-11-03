using System;
using Enemys;
using UnityEngine;

namespace Character.LockOns
{
    public class LockOn : MonoBehaviour
    {
        [SerializeField] private Transform targettransform;
        [SerializeField] private RectTransform lockonUiTransform;

        private void Update()
        {
            DisplayLockOnUI(targettransform.position);
        }

        public ITarget GetTarget()
        {
            return null;
        }

        /// <summary>
        /// ロックオンするUIを表示する
        /// </summary>
        public void DisplayLockOnUI(Vector3 target)
        {
            //ロックオンUIを表示するディスプレイ上の位置を計算
            var rayhit = new RaycastHit();
            var position = this.transform.position;
            var ray = new Ray(position, target - position);

            if (Physics.Raycast(ray, out rayhit))
            {
                //ターゲットかどうか
                if (rayhit.collider.gameObject.TryGetComponent<Display>(out var display))
                {
                    Vector2 uv = rayhit.textureCoord;
                    Debug.Log(rayhit.textureCoord);
                    lockonUiTransform.anchoredPosition = new Vector2(uv.x * 1920, uv.y * 1080);
                    Debug.Log(uv.x * 1920 + " ," + uv.y * 1080);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 5, false);
        }
    }
}