using UnityEngine;

namespace Enemys.EnemyParameter
{
    /// <summary>
    /// Enemyのパラメータを表すScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "enemydata", menuName = "ScriptableObjects/CreateEnemyParameter")]
    public class EnemyParameterAsset : ScriptableObject
    {
        [SerializeField] private int _maxhp;

        public int maxhp => _maxhp;
    }
}