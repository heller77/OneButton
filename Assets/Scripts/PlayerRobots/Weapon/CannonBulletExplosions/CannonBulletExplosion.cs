using System;
using Enemys;
using UnityEngine;

namespace Character.Weapon.CannonBulletExplosions
{
    public class CannonBulletExplosion : MonoBehaviour
    {
        [SerializeField] private float attackPower = 3.0f;

        [SerializeField] private float radius = 3.0f;
        private EnemyManager _enemyManager;

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this._enemyManager = enemyManager;
        }

        public void Explosion()
        {
            if (_enemyManager is null)
            {
                throw new Exception("enemymanager is null");
            }

            var enemiesInExplosionArea = _enemyManager.SearchEnemy(this.transform, radius);
            foreach (var enemy in enemiesInExplosionArea)
            {
                enemy.Hitted(attackPower);
            }
        }

        private void OnValidate()
        {
            this.transform.localScale = new Vector3(radius, radius, radius);
        }
    }
}