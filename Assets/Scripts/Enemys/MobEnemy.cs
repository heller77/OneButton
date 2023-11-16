using System;
using Enemys.EnemyParameter;
using UnityEngine;

namespace Enemys
{
    public class MobEnemy : MonoBehaviour, ITarget, IHitable
    {
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private EnemyParameterAsset _parameterAsset;
        [SerializeField] private float hp;

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
        /// 攻撃される
        /// </summary>
        /// <param name="damage"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Hitted(float damage)
        {
            this.hp -= damage;
            //hpが0以下なら倒れる
            if (this.hp <= 0)
            {
                this.Destruction();
            }
        }

        private void Destruction()
        {
            Debug.Log("death");
            _enemyManager.RemoveEnemy(this);

            Destroy(gameObject);
        }
    }
}