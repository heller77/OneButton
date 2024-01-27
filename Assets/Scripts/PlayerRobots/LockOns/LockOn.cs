using System;
using Cysharp.Threading.Tasks;
using Enemys;
using UnityEngine;

namespace Character.LockOns
{
    /// <summary>
    /// 敵にカーソルを表示したりする。
    /// </summary>
    public class LockOn : MonoBehaviour
    {
        [SerializeField] private Cursor cursor;

        /// <summary>
        /// 攻撃可能距離
        /// </summary>
        [SerializeField] private float attackableDistance = 100.0f;

        [SerializeField] private float cursorChangeTime = 1.0f;

        [SerializeField] private EnemyManager _enemyManager;

        // [SerializeField] private RectTransform lockonUiTransform;
        private Transform cameraTransform;
        private IHitable targetEnemy;

        [SerializeField] private LockOnState _lockOnState;

        /// <summary>
        /// ロックオンの状態を表す
        /// None 何もしない、SelectEnemy　攻撃する敵を選んでいる最中、 
        /// </summary>
        enum LockOnState
        {
            /// <summary>
            /// 何もしない
            /// </summary>
            None,

            /// <summary>
            /// 攻撃する敵を選んでいる最中
            /// </summary>
            SelectEnemy,

            /// <summary>
            /// 攻撃する敵を決めた状態
            /// </summary>
            DecideAttackTarget
        }

        private void Start()
        {
            cameraTransform = UnityEngine.Camera.main.transform;
            this._lockOnState = LockOnState.SelectEnemy;
            ChangeCursorPositionEverySomeSeconds();
        }

        /// <summary>
        /// 何秒かに一回カーソルを移動。攻撃できる対象の敵の上にカーソルを表示。
        /// </summary>
        private async UniTask ChangeCursorPositionEverySomeSeconds()
        {
            int index = 0;

            while (true)
            {
                if (this._lockOnState == LockOnState.SelectEnemy)
                {
                    //選択候補の敵取得
                    var enemies = this._enemyManager.SearchEnemy(transform, attackableDistance);
                    //次にカーソルを合わせる敵を取得
                    if (enemies.Count <= index)
                    {
                        //indexがenemiesの数より大きかったリセット
                        index = 0;
                    }

                    var target = enemies[index++];

                    //カーソルを表示し、移動
                    cursor.Display();
                    cursor.MoveToTarget(target.GetTransform());

                    //cursorChangeTime秒だけまって、またカーソルを移動させる
                    await UniTask.Delay(TimeSpan.FromSeconds(this.cursorChangeTime));
                }
                else
                {
                    //cursorChangeTime秒だけまって、またカーソルを移動させる
                    await UniTask.Delay(TimeSpan.FromSeconds(this.cursorChangeTime));
                }
            }
        }

        public IHitable GetTarget()
        {
            return this.targetEnemy;
        }

        /// <summary>
        /// 敵にカーソルを表示する
        /// DecideEnemyメソッドでカーソルのあってる敵を攻撃対象に出来る。
        /// </summary>
        private void DisplayCursor(Transform target)
        {
            cursor.ChangeVisualizeToSelectMode();
            cursor.MoveToTarget(target);
            cursor.Display();
        }

        /// <summary>
        /// 攻撃対象を決定
        /// </summary>
        public void DecideEnemy()
        {
            this._lockOnState = LockOnState.DecideAttackTarget;
            cursor.ChangeVisualizeToDecisionMode();
        }

        /// <summary>
        /// 攻撃対象を決定している状態をやめる
        /// 攻撃し終えた時に呼ばれることを想定
        /// カーソルでどの敵に攻撃するか選ぶ状態に戻す。
        /// </summary>
        public void CancellationDecideEnemy()
        {
        }
    }
}