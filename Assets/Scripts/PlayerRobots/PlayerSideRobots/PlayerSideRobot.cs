using Enemys;
using UnityEngine;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Character.PlayerSideRobots
{
    /// <summary>
    ///     プレイヤーサイドのロボット
    /// </summary>
    public class PlayerSideRobot : MonoBehaviour
    {
        [SerializeField] private MoverOnSpline _moverOnSpline;
        [SerializeField] private AttackComponent attackComponent;
        [SerializeField] private float attackPower;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //テキストの設定
            var guiStyle = new GUIStyle
                { fontSize = 20, normal = { textColor = Color.cyan }, alignment = TextAnchor.UpperCenter };

            //名前をシーンビュー上に表示
            Handles.Label(transform.position + new Vector3(0, 10, 0), gameObject.name, guiStyle);
        }
#endif

        /// <summary>
        ///     動きをスタート
        /// </summary>
        public void StartMove()
        {
            _moverOnSpline.Play();
        }

        /// <summary>
        ///     動きを止める
        /// </summary>
        public void StopMove()
        {
            _moverOnSpline.Pause();
        }

        public void Attack(MobEnemy enemy)
        {
        }
    }
}