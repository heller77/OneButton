using System;
using Character.LockOns;
using Enemys;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Character
{
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
                Debug.Log("playerrobot space");
                var targetenemy = _lockOn.GetTarget();
                if (targetenemy)
                {
                    attackComponent.Attack(targetenemy, 3);
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