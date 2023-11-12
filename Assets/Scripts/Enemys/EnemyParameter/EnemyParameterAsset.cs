using UnityEngine;

namespace Enemys.EnemyParameter
{
    [CreateAssetMenu(fileName = "enemydata", menuName = "ScriptableObjects/CreateEnemyParameter")]
    public class EnemyParameterAsset : ScriptableObject
    {
        [SerializeField] private int _maxhp;

        public int maxhp => _maxhp;
    }
}