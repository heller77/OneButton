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

        [SerializeField] private IHitable targetEnemy;
        [SerializeField] private Transform debugObjecttransform;
        [SerializeField] private Transform debugTarget;

        [SerializeField] private GameObject targetgameob;


        private void Start()
        {
            cameraTransform = UnityEngine.Camera.main.transform;
        }

        public IHitable GetTarget()
        {
            return this.targetEnemy;
        }

        private void FixedUpdate()
        {
            var nearEnemy = _enemyManager.GetMostNearEnemyInCameraDirection(transform, cameraTransform.forward);
            // IHitable nearEnemy = null;

            // targetgameob = nearEnemy.GetTransform().gameObject;
            if (nearEnemy != null)
            {
                DisplayLockOnUI(nearEnemy.GetTransform().position);
                targetEnemy = nearEnemy;

                debugObjecttransform.transform.position = targetEnemy.GetTransform().position;
            }
            else
            {
                HideLockOnUI();
                debugObjecttransform.gameObject.SetActive(false);
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
            // var ray = new Ray(position, new Vector3(-1, 0, 0));
            // var ray = new Ray(position, debugTarget.position - position);

            int DisplaylayerMask = 1 << 6;

            if (Physics.Raycast(ray, out rayhit, 10))
            {
                //ターゲットかどうか
                if (rayhit.collider.gameObject.TryGetComponent<Display>(out var display))
                {
                    Debug.Log("hit Display");
                    Vector2 uv = rayhit.textureCoord;
                    lockonUiTransform.anchoredPosition = new Vector2(uv.x * 1920, uv.y * 1080);
                    // Debug.Log(uv);
                    Debug.DrawRay(ray.origin, ray.direction * 40, Color.red, 1, false);
                }
                else
                {
                    Debug.Log("not hit to display", rayhit.collider.gameObject);
                    // HideLockOnUI();
                }
            }
            else
            {
                Debug.Log("non hit to anyobject");
                Debug.DrawRay(ray.origin, ray.direction * 40, Color.green, 1, false);
            }
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