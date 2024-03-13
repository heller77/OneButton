using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Playables;

namespace Character.Weapon.Lasers
{
    public class Laser : MonoBehaviour, GameLoops.IInitializable
    {
        /// <summary>
        /// レーザの末尾の部分の当たり判定
        /// </summary>
        private SphereCollider _laserEndCollider;

        /// <summary>
        /// レーザの先端オブジェクト
        /// </summary>
        [SerializeField] private GameObject laserEnd;

        [SerializeField] private float length;
        [SerializeField] private Transform hitEffectTransform;
        [SerializeField] private float zfitting;
        [SerializeField] private float launchLaserStretchTime = 0.1f;

        private Subject<Unit> hitSubject = new Subject<Unit>();

        /// <summary>
        /// レーザ末尾の球（見た目用）
        /// </summary>
        [SerializeField] private GameObject laserEndSphere;

        [SerializeField] private PlayableDirector laserStartEffectTimeline;

        /// <summary>
        /// マテリアル
        /// </summary>
        [SerializeField] private Material laserMaterial;

        [SerializeField] private GameObject _laserParent;

        private LaserMaterialController _materialController;

        /// <summary>
        /// 貫通した後どれだけ進むかという値
        /// </summary>
        [SerializeField] float extraDistance = 1000f;

        public Observable<Unit> hitObservable
        {
            get { return hitSubject; }
        }

        public void Initialize()
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
                        hitEffectTransform.gameObject.SetActive(true);
                        hitPos = point.point;
                        Debug.Log(hitPos);
                        var nowLaserPos = transform.position;
                        var vec_hitposToLaser = nowLaserPos - hitPos;
                        hitEffectTransform.transform.LookAt(this.transform.position);
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

            //lasermaterialcontrollerを初期化
            this._materialController = new LaserMaterialController(this.laserMaterial);

            //初めは非表示に
            this._laserParent.SetActive(false);
        }

        public void LookTarget(Transform target)
        {
            this.transform.LookAt(target, Vector3.right);
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

        /// <summary>
        /// レーザを止める
        /// </summary>
        public void StopLaser()
        {
            this._laserParent.SetActive(false);
            laserEndSphere.SetActive(false);

            StretchLaser(0);
        }

        /// <summary>
        /// レーザを発射
        /// </summary>
        public void LaunchLaser(Transform target)
        {
            this._laserParent.SetActive(true);
            laserEndSphere.SetActive(true);
            if (isLaunching)
            {
                return;
            }

            float distance = Vector3.Distance(target.position, transform.position);
            //レーザ発射アニメーションを再生する
            laserStartEffectTimeline.Play();

            //あたるまでレーザを進める
            StretchLaserUntilCollider(distance + this.extraDistance);
        }

        /// <summary>
        /// ここまでは一定の速度でレーザを進めるという距離
        /// </summary>
        private float accelerationDistance = 100;

        /// <summary>
        /// accelerationDistanceまでレーザが到着する時間
        /// </summary>
        [SerializeField] private float accelerationDistance_Time = 0.3f;

        [SerializeField] private float arriveTime = 1;
        private static readonly int Launch = Animator.StringToHash("launch");
        [SerializeField] private float laserThickness = 20;

        /// <summary>
        /// 物体にあたるまでレーザを伸ばす
        /// </summary>
        private async UniTask StretchLaserUntilCollider(float distance)
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
            Debug.Log("stretch");
            DOTween.To(() => 0, x => { this.StretchLaser(x); }, distance, elapsedTimeToaccelerationDistance)
                .SetEase(Ease.Linear);

            Debug.Log("expand");
            DOTween.To(() => 0, x => _materialController.SetLaserExpandValue(x), laserThickness,
                    elapsedTimeToaccelerationDistance)
                .SetEase(Ease.Linear);


            await UniTask.Delay(TimeSpan.FromSeconds(elapsedTimeToaccelerationDistance));

            //レーザをaccelerationDistanceまで進めた後、まで敵との間に距離があるなら
            //残りは時間内に到着するように
            if (accelerationDistance < distance)
            {
                DOTween.To(() => accelerationDistance, x => this.StretchLaser(x), distance,
                        arriveTime - launchLaserStretchTime)
                    .SetEase(Ease.Linear);
                await UniTask.Delay(TimeSpan.FromSeconds(arriveTime - launchLaserStretchTime));
            }


            //フラグをおろす
            isLaunching = false;

            // float perforateTime = 1.0f;
            // Observable.Timer(TimeSpan.FromSeconds(perforateTime)).Subscribe(_ =>
            // {
            //     this.hitEffectTransform.gameObject.SetActive(false);
            // }).AddTo(this);
            await UniTask.DelayFrame(1);
            //あたったことを通知
            this.hitSubject.OnNext(Unit.Default);
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