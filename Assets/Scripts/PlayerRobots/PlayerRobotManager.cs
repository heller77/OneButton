using System;
using Character.CockpitButtons;
using Character.LockOns;
using DG.Tweening;
using Enemys;
using GameManagers;
using MyInputs;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
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
    ///
    [ExecuteAlways]
    public class PlayerRobotManager : MonoBehaviour, GameLoops.IInitializable, GameLoops.ITickable
    {
        // [SerializeField] private SplineAnimate _splineAnimate;
        [SerializeField] private MoverOnSpline _moverOnSpline;

        [FormerlySerializedAs("_playerAttackComponent")] [SerializeField]
        private AttackComponent attackComponent;

        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private LockOn _lockOn;

        private GameInputs _inputs;

        [SerializeField] private bool isBattleMode = true;

        [SerializeField] private GameObject weaponParent;

        [SerializeField] private Button _button;

        [SerializeField] private CockpitDiplayManager _cockpitDiplayManager;

        /// <summary>
        /// ロボットのTransform
        /// </summary>
        [SerializeField] private Transform robotTransform;

        /// <summary>
        /// ロボットが注視するオブジェクト
        /// </summary>
        [SerializeField] private TrackingTransformMono robotTarget;

        /// <summary>
        /// カメラが注視するオブジェクト
        /// </summary>
        [SerializeField] private TrackingTransformMono cameraTarget;

        private readonly Subject<IHitable> _selectEnemy = new Subject<IHitable>();

        /// <summary>
        /// 敵を選択したら通知される
        /// </summary>
        public Observable<IHitable> SelectEnemy
        {
            get { return this._selectEnemy; }
        }

        private readonly Subject<Unit> _attack = new Subject<Unit>();

        /// <summary>
        /// 攻撃したら通知される
        /// </summary>
        public Observable<Unit> Attack
        {
            get { return this._attack; }
        }

        public void Initialize()
        {
            //入力設定
            _inputs = new GameInputs();
            _inputs.Enable();
            _inputs.Player.PushButton.performed += PushButton;
            _cockpitDiplayManager.AllHide();

            _lockOn.LockOnstateChange.Subscribe((state =>
            {
                if (state == LockOn.LockOnState.SelectEnemy)
                {
                    Debug.Log("lockonstate change event selecteenmy");
                    _cockpitDiplayManager.ShowSelect();
                }
            }));

            //敵を選択した時のイベント
            this._selectEnemy.Subscribe(target =>
            {
                // robotTarget.DOMove(target.GetTransform().position, 1.4f);
                // cameraTarget.DOMove(target.GetTransform().position, 0.3f);
                robotTarget.ChangeTracking(target.GetTransform(), 1.4f);
                cameraTarget.ChangeTracking(target.GetTransform(), 1.4f);
            });
        }

        public void Tick()
        {
            //ターゲットにキャラを向ける
            robotTransform.LookAt(robotTarget.transform);
        }

        /// <summary>
        /// 動きをスタート
        /// </summary>
        public void StartMove()
        {
            _moverOnSpline.Play();
        }

        /// <summary>
        /// 動きを止める
        /// </summary>
        public void StopMove()
        {
            _moverOnSpline.Pause();
        }

        /// <summary>
        /// バトルモードOn : カーソルを表示したり攻撃出来たりするようになる
        /// </summary>
        public void StartBattleMode()
        {
            this._cockpitDiplayManager.ShowSelect();
            this.isBattleMode = true;
            //カーソル表示
            _lockOn.ChangeTarget();
            _lockOn.Display();
        }

        /// <summary>
        /// バトルモードOff : カーソルを表示したり攻撃出来なくなる
        /// </summary>
        public void StopBattleMode()
        {
            this._cockpitDiplayManager.AllHide();

            this.isBattleMode = false;
            this._lockOn.Hide();
        }

        private void PushButton(InputAction.CallbackContext callbackContext)
        {
            //バトルモードじゃないなら攻撃しない
            if (!this.isBattleMode)
            {
                return;
            }

            if (_lockOn.GetState() == LockOn.LockOnState.SelectEnemy)
            {
                _lockOn.DecideEnemy();
                //攻撃までチャージする
                attackComponent.StartCharge();
                _cockpitDiplayManager.ShowAttack();

                var target = _lockOn.GetTarget();
                //通知
                _selectEnemy.OnNext(target);
            }
            else if (_lockOn.GetState() == LockOn.LockOnState.DecideAttackTarget)
            {
                //攻撃する敵を決めた状態でさらにボタンを押したら。

                //攻撃！
                attackComponent.ChargeAttack(_lockOn.GetTarget());

                this._button.Push();

                _lockOn.CancellationDecideEnemy();
                _cockpitDiplayManager.ShowSelect();

                _cockpitDiplayManager.DisplayNice();

                //通知
                _attack.OnNext(Unit.Default);
            }
        }

        /// <summary>
        /// 武器を非表示にする（リザルトシーン時に使用）
        /// </summary>
        public void HideWeapon()
        {
            weaponParent.transform.DOMove(new Vector3(0, -5.1f, 0), 0.1f).OnComplete(() =>
            {
                this.weaponParent.SetActive(false);
            });
        }

        public void DisplayHide()
        {
            _cockpitDiplayManager.AllHide();
        }

        public void PowerOn()
        {
            _button.PowerOn();
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