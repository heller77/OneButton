using Enemys;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Character.Weapon.Lasers
{
    /// <summary>
    ///     レーザ終点の球
    ///     今は使ってない(2024 5 11)
    /// </summary>
    public class AttackBall : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        private float damage;
        private bool isAttackable;

        private void Start()
        {
            _collider.OnCollisionEnterAsObservable().Subscribe(collision =>
            {
                Debug.Log("あたった" + collision.gameObject.name);
                if (isAttackable && collision.gameObject.TryGetComponent(out IHitable hit))
                {
                    hit.Hitted(damage);
                }
            });
        }

        private void SetDamage(float damage)
        {
            this.damage = damage;
        }

        /// <summary>
        ///     攻撃できる状態にする
        /// </summary>
        public void AttackableOn(float damage)
        {
            SetDamage(damage);
            isAttackable = true;
        }

        public void AttackbleOff()
        {
            damage = 0;
            isAttackable = false;
        }
    }
}