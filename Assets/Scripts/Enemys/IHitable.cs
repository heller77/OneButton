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
    }
}