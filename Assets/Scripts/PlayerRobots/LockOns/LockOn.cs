using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemys;
using GameLoops;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;

namespace Character.LockOns
{
    /// <summary>
    /// 敵にカーソルを表示したりする。
    /// </summary>
    public class LockOn : SerializedMonoBehaviour, IInitializable
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
        public IHitable targetEnemy;

        [SerializeField] private LockOnState _lockOnState;

        /// <summary>
        /// ロックオンの状態を表す
        /// None 何もしない、SelectEnemy　攻撃する敵を選んでいる最中、 
        /// </summary>
        public enum LockOnState
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

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public void Initialize()
        {
            cameraTransform = UnityEngine.Camera.main.transform;
            this._lockOnState = LockOnState.SelectEnemy;
            ChangeCursorPositionEverySomeSeconds(_tokenSource.Token);

            //enemymanagerから敵を倒したときのイベント通知を受け取る
            _enemyManager.enemyDestroy.Subscribe((x) => { ChangeTarget(); });
        }

        public void AutoChangeCursorStart()
        {
            ChangeCursorPositionEverySomeSeconds(_tokenSource.Token);
        }

        public void StopAutoChangeCursor()
        {
            _tokenSource.Cancel();
        }

        private void LateUpdate()
        {
            if (targetEnemy != null)
                cursor.Move(CulcurateCursorPosition(targetEnemy.GetTransform()));
        }

        private int index = 0;

        /// <summary>
        /// ターゲットを指定する
        /// </summary>
        public async UniTask SelectTarget(IHitable target)
        {
            cursor.Move(CulcurateCursorPosition(target.GetTransform()));

            cursor.DisplayInfo(target.GetEnemyType());

            //今ターゲットにしている敵を取得できるようにフィールドに代入。
            this.targetEnemy = target;
        }

        public async UniTask SelectTargetAbsolutely(IHitable target)
        {
            this.CancellationDecideEnemy();
            SelectTarget(target);
        }

        public void ChangeTarget()
        {
            //選択候補の敵取得
            var enemies =
                this._enemyManager.SearchEnemyInCamera(transform, attackableDistance, cameraTransform.forward);
            //次にカーソルを合わせる敵を取得
            if (enemies.Count <= index)
            {
                //indexがenemiesの数より大きかったリセット
                index = 0;
            }

            if (enemies.Count == 0)
            {
                targetEnemy = null;
                cursor.Hide();
                cursor.HideInfo();
                return;
            }

            var target = enemies[index++];

            //カーソルを表示し、移動
            // cursor.Display();
            SelectTarget(target);
        }

        /// <summary>
        /// 何秒かに一回カーソルを移動。攻撃できる対象の敵の上にカーソルを表示。
        /// </summary>
        private async UniTask ChangeCursorPositionEverySomeSeconds(CancellationToken token)
        {
            //startで読んでしまうと、_enemyManager.SearchEnemyがnullでエラーがでｒｙ
            await UniTask.DelayFrame(1);
            // int index = 0;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Debug.Log("lockon cancel");
                    this._tokenSource = new CancellationTokenSource();
                    return;
                }

                // Debug.Log("ChangeCursorPositionEverySomeSeconds update");
                if (this._lockOnState == LockOnState.SelectEnemy)
                {
                    ChangeTarget();
                }

                //cursorChangeTime秒だけまって、またカーソルを移動させる
                await UniTask.Delay(TimeSpan.FromSeconds(this.cursorChangeTime), cancellationToken: token);
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
            var cursorPosition = CulcurateCursorPosition(target);
            cursor.Move(cursorPosition);
            cursor.Display();
        }

        public void Display()
        {
            cursor.Display();
        }

        public void Hide()
        {
            cursor.Hide();
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
            this._lockOnState = LockOnState.SelectEnemy;
            cursor.ChangeVisualizeToSelectMode();
        }

        public LockOnState GetState()
        {
            return this._lockOnState;
        }

        /// <summary>
        /// カーソルが配置される球の半径
        /// </summary>
        [SerializeField] private float cursorSphereRadius = 3.0f;

        [SerializeField] private Transform cursorSphereCenter;

        private Vector3 CulcurateCursorPosition(Transform enemy)
        {
            //球の中心
            var origin = cursorSphereCenter.position;

            var rayDir = enemy.position - origin;
            return CulcurateCrossingPointsLineAndSphere(cursorSphereRadius, origin, rayDir);
        }

        /// <summary>
        /// 直線と球の交差点
        /// centerを通ってlineDir方向の直線と、centerを中心とする半径sphereRadiusの球との交差点を返す
        /// 原点からlineDirの逆方向へ進んだ時の交差点は計算しない点に注意。（直線と球の交差点となれば、本来2点得られる場合がある）
        /// </summary>
        /// <returns></returns>
        private Vector3 CulcurateCrossingPointsLineAndSphere(float sphereRadius, Vector3 center, Vector3 lineDir)
        {
            return center + lineDir.normalized * sphereRadius;
        }

#if UNITY_EDITOR
        /// <summary>
        /// ギズモを表示
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, this.attackableDistance);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(cursorSphereCenter.position, cursorSphereRadius);

            if (targetEnemy != null)
            {
                var dir = targetEnemy.GetTransform().position - cursorSphereCenter.position;
                Gizmos.DrawRay(cursorSphereCenter.position, dir);
            }
        }
#endif
    }
}