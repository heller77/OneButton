using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLoops;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Playables;

namespace Character.Weapon.Lasers
{
    /// <summary>
    ///     レーザ
    /// </summary>
    public class Laser : MonoBehaviour, IInitializable
    {
        private static readonly int Launch = Animator.StringToHash("launch");

        /// <summary>
        ///     レーザの先端オブジェクト
        /// </summary>
        [SerializeField] private GameObject laserEnd;

        [SerializeField] private float length;
        [SerializeField] private Transform hitEffectTransform;
        [SerializeField] private float zfitting;
        [SerializeField] private float launchLaserStretchTime = 0.1f;

        /// <summary>
        ///     レーザ末尾の球（見た目用）
        /// </summary>
        [SerializeField] private GameObject laserEndSphere;

        [SerializeField] private PlayableDirector laserStartEffectTimeline;

        /// <summary>
        ///     マテリアル
        /// </summary>
        [SerializeField] private Material laserMaterial;

        [SerializeField] private GameObject _laserParent;

        /// <summary>
        ///     貫通した後どれだけ進むかという値
        /// </summary>
        [SerializeField] float extraDistance = 1000f;

        [SerializeField] private Transform origin;

        /// <summary>
        ///     accelerationDistanceまでレーザが到着する時間
        /// </summary>
        [SerializeField] private float accelerationDistance_Time = 0.3f;

        [SerializeField] private float arriveTime = 1;
        [SerializeField] private float laserThickness = 20;

        /// <summary>
        ///     ここまでは一定の速度でレーザを進めるという距離
        /// </summary>
        private readonly float accelerationDistance = 100;

        private readonly Subject<Unit> hitSubject = new Subject<Unit>();

        /// <summary>
        ///     レーザの末尾の部分の当たり判定
        /// </summary>
        private SphereCollider _laserEndCollider;

        private LaserMaterialController _materialController;

        private bool isLaunching;

        public Observable<Unit> hitObservable
        {
            get { return hitSubject; }
        }
#if UNITY_EDITOR

        // インスペクターから編集されたとき
        private void OnValidate()
        {
            StretchLaser(length);
        }

#endif

        public void Initialize()
        {
            _laserEndCollider = laserEnd.GetComponent<SphereCollider>();
            //レーザ末尾があたった時
            _laserEndCollider.OnCollisionEnterAsObservable()
                .Subscribe(other =>
                {
                    Vector3 hitPos;
                    foreach (ContactPoint point in other.contacts)
                    {
                        hitEffectTransform.gameObject.SetActive(true);
                        hitPos = point.point;
                        var nowLaserPos = transform.position;
                        var vec_hitposToLaser = nowLaserPos - hitPos;
                        hitEffectTransform.transform.LookAt(transform.position);
                        hitEffectTransform.position = hitPos + vec_hitposToLaser * zfitting;
                    }
                });
            StretchLaser(0);

            //lasermaterialcontrollerを初期化
            _materialController = new LaserMaterialController(laserMaterial);

            //初めは非表示に
            _laserParent.SetActive(false);
        }

        public void LookTarget(Transform target)
        {
            transform.LookAt(target, Vector3.right);
        }

        /// <summary>
        ///     レーザを伸ばす
        /// </summary>
        private void StretchLaser(float length)
        {
            var scale = transform.localScale;
            scale.y = length;
            transform.localScale = scale;
            //
            var vector3 = laserEnd.transform.localPosition;
            vector3.y = length;
            laserEnd.transform.localPosition = vector3;
        }

        /// <summary>
        ///     レーザを止める
        /// </summary>
        public void StopLaser()
        {
            _laserParent.SetActive(false);
            laserEndSphere.SetActive(false);

            StretchLaser(0);
        }

        /// <summary>
        ///     レーザを発射
        /// </summary>
        public void LaunchLaser(Transform target, float damage)
        {
            _laserParent.SetActive(true);
            laserEndSphere.SetActive(true);
            if (isLaunching)
            {
                return;
            }

            float distance = Vector3.Distance(target.position, transform.position);
            //レーザ発射アニメーションを再生する
            laserStartEffectTimeline.Play();


            //あたるまでレーザを進める
            StretchLaserUntilCollider(distance + extraDistance, damage);
            RayAttacker.RayAttack(origin, target.transform, damage);
        }

        /// <summary>
        ///     物体にあたるまでレーザを伸ばす
        /// </summary>
        private async UniTask StretchLaserUntilCollider(float distance, float damage)
        {
            isLaunching = true;

            int laserLength = 0;
            bool stretchLaserFlag = false;
            _laserEndCollider.OnCollisionEnterAsObservable()
                .Subscribe(other => { stretchLaserFlag = true; });

            //accelerationDistanceまでレーザを進める時間
            float elapsedTimeToaccelerationDistance = 0;
            if (distance < accelerationDistance)
            {
                //レーザがaccelerationDistanceまで
                elapsedTimeToaccelerationDistance = accelerationDistance_Time * (distance / accelerationDistance);
            }
            else
            {
                elapsedTimeToaccelerationDistance = accelerationDistance_Time;
            }

            //レーザをaccelerationDistanceまで進める
            DOTween.To(() => 0, x => { StretchLaser(x); }, distance, elapsedTimeToaccelerationDistance)
                .SetEase(Ease.Linear);

            DOTween.To(() => 0, x => _materialController.SetLaserExpandValue(x), laserThickness,
                    elapsedTimeToaccelerationDistance)
                .SetEase(Ease.Linear);


            await UniTask.Delay(TimeSpan.FromSeconds(elapsedTimeToaccelerationDistance));

            RayAttacker.RayAttack(origin, laserEnd.transform, damage);

            //レーザをaccelerationDistanceまで進めた後、まで敵との間に距離があるなら
            //残りは時間内に到着するように
            if (accelerationDistance < distance)
            {
                DOTween.To(() => accelerationDistance, x => StretchLaser(x), distance,
                        arriveTime - launchLaserStretchTime)
                    .SetEase(Ease.Linear);
                await UniTask.Delay(TimeSpan.FromSeconds(arriveTime - launchLaserStretchTime));
            }

            //攻撃する
            RayAttacker.RayAttack(origin, laserEnd.transform, damage);

            //フラグをおろす
            isLaunching = false;

            // float perforateTime = 1.0f;
            // Observable.Timer(TimeSpan.FromSeconds(perforateTime)).Subscribe(_ =>
            // {
            //     this.hitEffectTransform.gameObject.SetActive(false);
            // }).AddTo(this);
            await UniTask.DelayFrame(1);
            //あたったことを通知
            hitSubject.OnNext(Unit.Default);
        }
    }
}