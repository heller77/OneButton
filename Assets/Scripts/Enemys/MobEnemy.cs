using System;
using Enemys.EnemyParameter;
using UnityEngine;

namespace Enemys
{
    public class MobEnemy : MonoBehaviour, ITarget, IHitable
    {
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private EnemyParameterAsset _parameterAsset;
        [SerializeField] private int hp;

        private void Start()
        {
            _enemyManager.Add(this);
            hp = _parameterAsset.maxhp;
        }

        public Vector3 GetPosition()
        {
            return this.transform.position;
        }

        /// <summary>
        /// 攻撃された
        /// </summary>
        public void Hitted()
        {
        }
    }
}