using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Character.Weapon.Lasers
{
    public class Laser : MonoBehaviour
    {
        /// <summary>
        /// レーザの末尾の部分の当たり判定
        /// </summary>
        private SphereCollider _laserEndCollider;

        [SerializeField] private GameObject laserEnd;

        [SerializeField] private float length;
        [SerializeField] private Transform hitEffectTransform;
        [SerializeField] private float zfitting;
        [SerializeField] private float launchLaserStretchTime = 0.1f;

        private void Awake()
        {
            _laserEndCollider = laserEnd.GetComponent<SphereCollider>();
            //レーザ末尾があたった時

            _laserEndCollider.OnCollisionEnterAsObservable()
                .Subscribe(other =>
                {
                    Debug.Log("hit");
                    Vector3 hitPos;
                    foreach (ContactPoint point in other.contacts)
                    {
                        hitPos = point.point;
                        Debug.Log(hitPos);
                        var nowLaserPos = transform.position;
                        var vec_hitposToLaser = nowLaserPos - hitPos;
                        hitEffectTransform.position = hitPos + vec_hitposToLaser * zfitting;
                    }
                });
            this.StretchLaser(0);

            // _laserEndCollider.collisionEnterObserable.Subscribe(collision =>
            // {
            //     Debug.Log("");
            //     foreach (var contact in collision.contacts)
            //     {
            //         Debug.Log(contact.point);
            //     }
            // });
        }

        /// <summary>
        /// レーザを伸ばす
        /// </summary>
        public void StretchLaser(float length)
        {
            var scale = this.transform.localScale;
            scale.y = length;
            this.transform.localScale = scale;
            //
            var vector3 = laserEnd.transform.localPosition;
            vector3.y = length;
            laserEnd.transform.localPosition = vector3;
        }

        private bool isLaunching = false;

        public void LaunchLaser()
        {
            if (isLaunching)
            {
                return;
            }


            //あたるまでレーザを進める
            StretchLaserUntilCollider();
        }

        private async UniTask StretchLaserUntilCollider()
        {
            isLaunching = true;

            int laserLength = 0;
            bool stretchLaserFlag = false;
            _laserEndCollider.OnCollisionEnterAsObservable()
                .Subscribe(other => { stretchLaserFlag = true; });
            float preLength = 0;
            while (!stretchLaserFlag)
            {
                laserLength++;
                DOTween.To(() => preLength, x => this.StretchLaser(x), laserLength, launchLaserStretchTime)
                    .SetEase(Ease.Linear);

                await UniTask.Delay(TimeSpan.FromSeconds(launchLaserStretchTime));
                preLength = laserLength;
            }

            isLaunching = false;
        }
#if UNITY_EDITOR

        // インスペクターから編集されたとき
        private void OnValidate()
        {
            StretchLaser(length);
        }

#endif
    }
}