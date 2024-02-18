using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Enemys
{
    /// <summary>
    /// 敵の母艦
    /// </summary>
    public class BossEnemy : MonoBehaviour, ITrapable, IHitable
    {
        [SerializeField] private GameObject defaultObject;

        [SerializeField] private GameObject destructionObject;

        [SerializeField] private List<Rigidbody> parts;
        [SerializeField] private float destructionPower = 1.0f;
        [SerializeField] private Transform powerOriginTransform;
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private float hp = 1;
        [SerializeField] private EnemyManager _enemyManager;

        private bool isArrive = true;

        public void Boot()
        {
        }

        public void Destruction()
        {
            _particleSystem.Play();
            _enemyManager.RemoveBoss();
            defaultObject.SetActive(false);
            destructionObject.SetActive(true);
            foreach (var part in parts)
            {
                var powerDir = part.transform.position - powerOriginTransform.transform.position;
                part.AddForce(destructionPower * powerDir);
            }
        }

        public void Hitted(float damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                this.Destruction();
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public bool isHitable()
        {
            return isArrive;
        }

        /// <summary>
        /// ボスを攻撃できる距離の範囲（はたしてボスがもっているのが正しいのか？）
        /// </summary>
        [SerializeField] private float bossAttackableRadius = 1000.0f;

        public float GetBossAttackableRadius()
        {
            return this.bossAttackableRadius;
        }

        /// <summary>
        /// ボスを攻撃できる範囲をギズモで表示
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, bossAttackableRadius);
        }
    }
}