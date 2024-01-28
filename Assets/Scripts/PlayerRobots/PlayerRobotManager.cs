using System;
using Character.LockOns;
using Enemys;
using GameManagers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Character
{
    /// <summary>
    /// ロボットの動きや攻撃を管理する
    /// </summary>
    public class PlayerRobotManager : MonoBehaviour
    {
        // [SerializeField] private SplineAnimate _splineAnimate;
        [SerializeField] private MoverOnSpline _moverOnSpline;

        [FormerlySerializedAs("_playerAttackComponent")] [SerializeField]
        private AttackComponent attackComponent;

        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private LockOn _lockOn;

        /// <summary>
        /// 動きをスタート
        /// </summary>
        public void StartMove()
        {
            Debug.Log("start move");
            _moverOnSpline.Play();
        }

        /// <summary>
        /// 動きを止める
        /// </summary>
        public void StopMove()
        {
            _moverOnSpline.Pause();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_lockOn.GetState() == LockOn.LockOnState.SelectEnemy)
                {
                    Debug.Log("playerrobot space");
                    // var targetenemy = _lockOn.GetTarget();
                    // if (targetenemy != null)
                    // {
                    //     _lockOn.DecideEnemy();
                    //     Debug.Log("攻撃成功！");
                    //     attackComponent.Attack(targetenemy, 3);
                    //     attackComponent.FireBullet(targetenemy.GetTransform());
                    //
                    //     //弾を使った数を記録
                    //     BattleResultManager.GetInstance().AddConsumeBullet();
                    // }
                    _lockOn.DecideEnemy();
                    //攻撃までチャージする
                    attackComponent.StartCharge();
                }
                else if (_lockOn.GetState() == LockOn.LockOnState.DecideAttackTarget)
                {
                    //攻撃！
                    attackComponent.ChargeAttack(_lockOn.GetTarget());

                    _lockOn.CancellationDecideEnemy();
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //テキストの設定
            var guiStyle = new GUIStyle
                { fontSize = 20, normal = { textColor = Color.green }, alignment = TextAnchor.UpperCenter };
            //名前をシーンビュー上に表示
            Handles.Label(transform.position + new Vector3(0, 10, 0), this.gameObject.name, guiStyle);
        }
#endif
    }
}