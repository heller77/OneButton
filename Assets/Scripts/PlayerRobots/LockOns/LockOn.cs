using System;
using Enemys;
using UnityEngine;

namespace Character.LockOns
{
    public class LockOn : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;


        [SerializeField] private RectTransform lockonUiTransform;
        private Transform cameraTransform;

        [SerializeField] private MobEnemy targetEnemy;


        private void Start()
        {
            cameraTransform = UnityEngine.Camera.main.transform;
        }

        public MobEnemy GetTarget()
        {
            return this.targetEnemy;
        }

        private void Update()
        {
            var nearEnemy = _enemyManager.GetMostNearEnemyInCameraDirection(transform, cameraTransform.forward);
            if (nearEnemy)
            {
                DisplayLockOnUI(nearEnemy.transform.position);
                targetEnemy = nearEnemy;
            }
            else
            {
                HideLockOnUI();
            }
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
                    lockonUiTransform.anchoredPosition = new Vector2(uv.x * 1920, uv.y * 1080);
                }
                else
                {
                    HideLockOnUI();
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 5, false);
        }

        /// <summary>
        /// ロックオンUIを非表示にする（画面外に持ってくる）
        /// </summary>
        public void HideLockOnUI()
        {
            lockonUiTransform.anchoredPosition = new Vector2(-100, -100);
        }
    }
}