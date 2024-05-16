using UnityEngine;

namespace Enemys
{
    /// <summary>
    ///     攻撃が当たる
    /// </summary>
    public interface IHitable
    {
        /// <summary>
        ///     攻撃を受ける
        /// </summary>
        public void Hitted(float damage);

        /// <summary>
        ///     位置を取得
        /// </summary>
        /// <returns></returns>
        public Transform GetTransform();

        /// <summary>
        ///     攻撃できるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsHitable();

        public EnemyType GetEnemyType();
    }
}