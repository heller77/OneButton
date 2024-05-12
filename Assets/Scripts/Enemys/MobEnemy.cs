#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using Enemys.Boids;
using Enemys.EnemyParameter;
using Enemys.ExplosionEffect;
using GameManagers;
using GameManagers.AudioManagers;
using R3;
using UnityEngine;
using Observable = R3.Observable;

namespace Enemys
{
    /// <summary>
    ///     モブ敵
    /// </summary>
    public class MobEnemy : MonoBehaviour, ITarget, IHitable
    {
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private EnemyParameterAsset _parameterAsset;
        [SerializeField] private float hp;

        /// <summary>
        ///     同じGameObjectに付属しているBoid
        /// </summary>
        private Boid boid;

        private bool isArrive = true;

        private void Start()
        {
            _enemyManager.AddMobEnemy(this);
            hp = _parameterAsset.maxhp;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //テキストの設定
            var guiStyle = new GUIStyle
                { fontSize = 20, normal = { textColor = Color.red }, alignment = TextAnchor.UpperCenter };

            //名前をシーンビュー上に表示
            Handles.Label(transform.position + new Vector3(0, 10, 0), gameObject.name, guiStyle);
        }
#endif

        /// <summary>
        ///     攻撃される
        /// </summary>
        /// <param name="damage"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Hitted(float damage)
        {
            hp -= damage;
            //hpが0以下なら倒れる
            if (hp <= 0)
            {
                Destruction();
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
            return EnemyType.mob;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetBoid(Boid boid)
        {
            this.boid = boid;
        }

        public void DontDisplay()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     敵破壊（倒したときに呼ばれる）
        /// </summary>
        private void Destruction()
        {
            //se再生
            AudioManager.Instance.PlaySe(SeVariable.EnemyDeath, transform.position, 1);

            _enemyManager.RemoveEnemy(this);

            //boidも破棄処理を行う
            if (boid != null)
                boid.Destruction();

            BattleResultManager.GetInstance().AddKnockMobEnemy();

            // Destroy(gameObject);
            gameObject.SetActive(false);
            isArrive = false;
            //explosion配置（x秒後に非表示）
            var explosionEffect = EnemyDeathExplosionEffectPool.Instance.GetExplosionEffectPool();
            explosionEffect.transform.position = transform.position;
            float explosioNDisappearTime = 2;
            Observable.Timer(TimeSpan.FromSeconds(explosioNDisappearTime)).Subscribe(_ =>
            {
                EnemyDeathExplosionEffectPool.Instance.Release(explosionEffect);
            }).AddTo(this);
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }
    }
}