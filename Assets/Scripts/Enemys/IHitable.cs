using UnityEngine;

namespace Enemys
{
    /// <summary>
    /// 攻撃が当たる
    /// </summary>
    public interface IHitable
    {
        /// <summary>
        /// 攻撃を受ける
        /// </summary>
        public void Hitted(float damage);

        public Transform GetTransform();

        /// <summary>
        /// 攻撃できるかどうか
        /// </summary>
        /// <returns></returns>
        public bool isHitable();

        public EnemyType GetEnemyType();
    }
}