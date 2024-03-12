using System;
using Cysharp.Threading.Tasks;
using Enemys.Boids;
using Enemys.EnemyParameter;
using Enemys.ExplosionEffect;
using GameManagers;
using GameManagers.SeManagers;
using R3;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Observable = R3.Observable;

namespace Enemys
{
    public class MobEnemy : MonoBehaviour, ITarget, IHitable
    {
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private EnemyParameterAsset _parameterAsset;
        [SerializeField] private float hp;
        private bool isArrive = true;

        /// <summary>
        /// 同じGameObjectに付属しているBoid
        /// </summary>
        private Boid boid;

        private void Start()
        {
            _enemyManager.Add(this);
            hp = _parameterAsset.maxhp;
        }

        public Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public void SetBoid(Boid boid)
        {
            this.boid = boid;
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

        public Transform GetTransform()
        {
            return this.transform;
        }

        public bool isHitable()
        {
            return isArrive;
        }

        public EnemyType GetEnemyType()
        {
            return EnemyType.mob;
        }

        public void DontDisplay()
        {
            this.gameObject.SetActive(false);
        }

        private void Destruction()
        {
            //se再生
            AudioManager.Instance.PlaySe(SeVariable.EnemyDeath, this.transform.position, 1);

            Debug.Log("death");
            _enemyManager.RemoveEnemy(this);

            //boidも破棄処理を行う
            if (boid != null)
                this.boid.Destruction();

            BattleResultManager.GetInstance().AddKnockMobEnemy();

            // Destroy(gameObject);
            this.gameObject.SetActive(false);
            isArrive = false;
            //explosion配置（x秒後に非表示）
            var explosionEffect = EnemyDeathExplosionEffectPool.Instance.GetExplosionEffectPool();
            explosionEffect.transform.position = this.transform.position;
            float explosioNDisappearTime = 2;
            Observable.Timer(TimeSpan.FromSeconds(explosioNDisappearTime)).Subscribe(_ =>
            {
                EnemyDeathExplosionEffectPool.Instance.Release(explosionEffect);
            }).AddTo(this);
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this._enemyManager = enemyManager;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //テキストの設定
            var guiStyle = new GUIStyle
                { fontSize = 20, normal = { textColor = Color.red }, alignment = TextAnchor.UpperCenter };

            //名前をシーンビュー上に表示
            Handles.Label(transform.position + new Vector3(0, 10, 0), this.gameObject.name, guiStyle);
        }
#endif
    }
}