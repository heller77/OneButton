using System;
using Enemys.EnemyParameter;
using GameManagers;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

            BattleResultManager.GetInstance().AddKnockMobEnemy();

            Destroy(gameObject);
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