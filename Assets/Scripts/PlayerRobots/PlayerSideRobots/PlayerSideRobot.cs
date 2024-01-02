using Enemys;
using UnityEngine;
using Utils;

namespace Character.PlayerSideRobots
{
    public class PlayerSideRobot : MonoBehaviour

    {
        [SerializeField] private MoverOnSpline _moverOnSpline;

        [SerializeField] private AttackComponent attackComponent;
        [SerializeField] private float attackPower;

        /// <summary>
        /// 動きをスタート
        /// </summary>
        public void StartMove()
        {
            this._moverOnSpline.Play();
        }

        // <summary>
        /// 動きを止める
        /// </summary>
        public void StopMove()
        {
            _moverOnSpline.Pause();
        }

        public void Attack(MobEnemy enemy)
        {
            attackComponent.Attack(enemy, attackPower);
        }
    }
}