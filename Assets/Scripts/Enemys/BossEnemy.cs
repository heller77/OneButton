using System;
using System.Collections.Generic;
using GameManagers;
using GameManagers.SeManagers;
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

        /// <summary>
        /// 起動
        /// </summary>
        public void Boot()
        {
        }

        /// <summary>
        /// 破壊
        /// </summary>
        public void Destruction()
        {
            AudioManager.Instance.PlaySe(SeVariable.EnemyDeath, this.transform.position, 1);
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

        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void Hitted(float damage)
        {
            BattleResultManager.GetInstance().AddBossDamage((int)damage);

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

        public bool IsHitable()
        {
            return isArrive;
        }

        public EnemyType GetEnemyType()
        {
            return EnemyType.mother;
        }

        /// <summary>
        /// ボスを攻撃できる距離の範囲（はたしてボスがもっているのが正しいのか？）
        /// </summary>
        [SerializeField] private float bossAttackableRadius = 1000.0f;

        /// <summary>
        /// 攻撃できる範囲を取得
        /// </summary>
        /// <returns></returns>
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