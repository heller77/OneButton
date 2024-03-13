using System;
using Character.CockpitButtons;
using Character.LockOns;
using DG.Tweening;
using Enemys;
using GameManagers;
using MyInputs;
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
    public class PlayerRobotManager : MonoBehaviour, GameLoops.IInitializable
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

        public void Initialize()
        {
            //入力設定
            _inputs = new GameInputs();
            _inputs.Enable();
            _inputs.Player.PushButton.performed += PushButton;
            _cockpitDiplayManager.AllHide();
        }

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

        /// <summary>
        /// バトルモードOn : カーソルを表示したり攻撃出来たりするようになる
        /// </summary>
        public void StartBattleMode()
        {
            this._cockpitDiplayManager.ShowSelect();
            this.isBattleMode = true;
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
                Debug.Log("playerrobot space");
                _lockOn.DecideEnemy();
                //攻撃までチャージする
                attackComponent.StartCharge();
                _cockpitDiplayManager.ShowAttack();
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