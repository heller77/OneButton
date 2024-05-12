using System;
using DG.Tweening;
using Enemys;
using UnityEngine;

namespace Character.Weapon.CannonBulletExplosions
{
    /// <summary>
    ///     大砲の爆発
    /// </summary>
    public class CannonBulletExplosion : MonoBehaviour
    {
        [SerializeField] private float attackPower = 3.0f;

        [SerializeField] private float radius = 3.0f;
        [SerializeField] private GameObject explosionSphere;
        [SerializeField] private ParticleSystem enerygyParticle;
        private EnemyManager _enemyManager;

        private void OnValidate()
        {
            transform.localScale = new Vector3(radius, radius, radius);
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        public void Explosion()
        {
            if (_enemyManager is null)
            {
                throw new Exception("enemymanager is null");
            }

            var enemiesInExplosionArea = _enemyManager.SearchEnemy(transform, radius);
            foreach (var enemy in enemiesInExplosionArea)
            {
                enemy.Hitted(attackPower);
            }

            enerygyParticle.Play();
        }

        public void Fade()
        {
            explosionSphere.GetComponent<Renderer>().material.DOFade(0, 1);
        }
    }
}