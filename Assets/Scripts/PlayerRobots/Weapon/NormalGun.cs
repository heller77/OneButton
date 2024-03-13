using System;
using Character.Weapon.Lasers;
using Cysharp.Threading.Tasks;
using Enemys;
using GameManagers;
using GameManagers.SeManagers;
using R3;
using UnityEngine;

namespace Character.Weapon
{
    public class NormalGun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletInstancePosition;

        [SerializeField] private AudioManager _audioManager;

        [SerializeField] private Laser _laser;
        [SerializeField] private Transform ta;

        /// <summary>
        /// 
        /// </summary>
        public async void Attack(IHitable target, float attackPower)
        {
            AudioManager.Instance.PlaySe(SeVariable.normalbulletFireSE, this.transform.position, 0.05f);

            // var bulletInstance = Instantiate(bulletPrefab, bulletInstancePosition.position, Quaternion.identity);
            //
            // float elapsedTime = 0;
            //
            // //弾を飛ばす
            // float duration = 1.0f;
            // while (elapsedTime <= duration)
            // {
            //     //ターゲットが攻撃出来る状態でないならば、
            //     if (target != null && !target.isHitable())
            //     {
            //         VanishBullet(bulletInstance);
            //         return;
            //     }
            //
            //     Vector3 targetPos = target.GetTransform().position;
            //
            //
            //     float t = elapsedTime / duration;
            //     var nowPosition = bulletInstance.transform.position;
            //     var diff = targetPos - nowPosition;
            //     if (diff.magnitude < 1.1)
            //     {
            //         Debug.Log("近すぎる");
            //         break;
            //     }
            //
            //     var newpos = nowPosition + t * diff;
            //     bulletInstance.transform.position = newpos;
            //
            //     await UniTask.DelayFrame(1);
            //     elapsedTime += Time.deltaTime;
            // }
            // transform.LookAt(target.GetTransform());
            Debug.Log("target look ", target.GetTransform().gameObject);

            //敵までの距離を計算
            var distance = Vector3.Distance(target.GetTransform().position, this.transform.position);

            transform.LookAt(target.GetTransform().position);
            _laser.LaunchLaser(target.GetTransform(), attackPower);

            bool isHit = false;
            _laser.hitObservable.Subscribe(_ => { isHit = true; });
            while (!isHit)
            {
                transform.LookAt(target.GetTransform().position);
                await UniTask.DelayFrame(1);
            }

            //あたるまで待つ
            // await _laser.hitObservable.FirstAsync();

            // target.Hitted(attackPower);

            //レーザ止める
            _laser.StopLaser();

            // VanishBullet(bulletInstance);

            //スコア追加
            BattleResultManager.GetInstance().AddConsumeBullet();
        }

        private void VanishBullet(GameObject bullet)
        {
            bullet.SetActive(false);
        }
    }
}