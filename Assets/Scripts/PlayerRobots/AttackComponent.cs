using System.Collections.Generic;
using Character.Weapon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemys;
using UnityEngine;

namespace Character
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletFirePosition;

        private float CannonMinPower = 0.5f;
        [SerializeField] private Cannon cannon;
        [SerializeField] private NormalBullet _normalBullet;

        [SerializeField] private float chargePercentage = 0.0f;
        [SerializeField] private float chargeSpeed = 1.0f;

        /// <summary>
        /// チャージを始める
        /// </summary>
        public async UniTask StartCharge()
        {
            this.chargePercentage = 0;
            chargeing();
        }

        /// <summary>
        /// チャージする（StartChargeメソッドから呼ばれて非同期で実行される）
        /// </summary>
        private async UniTask chargeing()
        {
            while (chargePercentage <= 1.0f)
            {
                chargePercentage += Time.deltaTime * chargeSpeed;
                //1フレーム待つ
                await UniTask.DelayFrame(1);
            }
        }

        public void Attack(IHitable target, float AttackPower)
        {
            Debug.Log("attack");
            target.Hitted(AttackPower);
        }

        public void ChargeAttack(IHitable target)
        {
            if (chargePercentage > CannonMinPower)
            {
                cannon.Attack(target);
            }
            else
            {
                _normalBullet.Attack();
            }
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        public void FireBullet(Transform target)
        {
            var bullet = Instantiate(bulletPrefab, bulletFirePosition.position, Quaternion.identity);
            //memo : ここtargetが動くとあたっていないように見えるので、修正する
            bullet.transform.DOMove(target.position, 0.6f);
            MoveBulletToTarget(bullet.transform, target, 10.5f);
        }

        /// <summary>
        /// 弾をターゲットまで飛ばす
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="targetTransform"></param>
        /// <param name="duration"></param>
        private async UniTask MoveBulletToTarget(Transform bullet, Transform targetTransform, float duration)
        {
            float time = 0;
            var firstPos = bullet.position;
            while (time <= duration)
            {
                //0-1の
                float t = time / duration;
                var diff = bullet.position - targetTransform.position;
                var newpos = bullet.position + t * diff;
                bullet.position = newpos;

                await UniTask.DelayFrame(1);
                time += Time.deltaTime;
            }
        }
    }
}