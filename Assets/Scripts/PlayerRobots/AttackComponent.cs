using Character.Weapon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletFirePosition;

        private readonly float _cannonMinPower = 0.5f;
        [SerializeField] private Cannon cannon;

        [FormerlySerializedAs("_normalBullet")] [SerializeField]
        private NormalGun normalGun;

        [SerializeField] private float chargePercentage;
        [SerializeField] private float chargeSpeed = 1.0f;

        /// <summary>
        /// チャージを始める
        /// </summary>
        public async UniTask StartCharge()
        {
            this.chargePercentage = 0;
            await Chargeing();
        }

        /// <summary>
        /// チャージする（StartChargeメソッドから呼ばれて非同期で実行される）
        /// </summary>
        private async UniTask Chargeing()
        {
            while (chargePercentage <= 1.0f)
            {
                chargePercentage += Time.deltaTime * chargeSpeed;
                //1フレーム待つ
                await UniTask.DelayFrame(1);
            }
        }

        public void Attack(IHitable target, float attackPower)
        {
            Debug.Log("attack");
            target.Hitted(attackPower);
        }

        public void ChargeAttack(IHitable target)
        {
            if (chargePercentage > _cannonMinPower)
            {
                var cannonAttackPower = 10;
                cannon.Attack(target, cannonAttackPower);
            }
            else
            {
                var normalGunAttackPower = 1;
                normalGun.Attack(target, normalGunAttackPower);
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
            var moveBulletToTarget = MoveBulletToTarget(bullet.transform, target, 10.5f);
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
                var position = bullet.position;
                var diff = position - targetTransform.position;
                var newpos = position + t * diff;
                position = newpos;
                bullet.position = position;

                await UniTask.DelayFrame(1);
                time += Time.deltaTime;
            }
        }
    }
}