namespace Enemys
{
    /// <summary>
    ///     任意のタイミングで発動する罠のようなもの
    /// </summary>
    public interface ITrapable
    {
        /// <summary>
        ///     起動する
        /// </summary>
        public void Boot();
    }
}