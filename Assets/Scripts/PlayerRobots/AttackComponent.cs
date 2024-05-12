using Character.Weapon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    /// <summary>
    /// 敵へ攻撃するもの
    /// </summary>
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletFirePosition;

        private readonly float _cannonMinPower = 0.5f;

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

        public void ChargeAttack(IHitable target)
        {
            var normalGunAttackPower = 1;
            normalGun.Attack(target, normalGunAttackPower);
        }
    }
}