using System;
using DG.Tweening;
using Enemys;
using UnityEngine;

namespace Character.Weapon.CannonBulletExplosions
{
    public class CannonBulletExplosion : MonoBehaviour
    {
        [SerializeField] private float attackPower = 3.0f;

        [SerializeField] private float radius = 3.0f;
        private EnemyManager _enemyManager;
        [SerializeField] private GameObject explosionSphere;
        [SerializeField] private ParticleSystem enerygyParticle;

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

            enerygyParticle.Play();
        }

        private void OnValidate()
        {
            this.transform.localScale = new Vector3(radius, radius, radius);
        }

        public void Fade()
        {
            explosionSphere.GetComponent<Renderer>().material.DOFade(0, 1);
        }
    }
}