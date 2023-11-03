using UnityEngine;

namespace Enemys
{
    /// <summary>
    /// ターゲット
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// ターゲットの位置を返す
        /// </summary>
        /// <returns>ターゲットの位置</returns>
        Vector3 GetPosition();
    }
}