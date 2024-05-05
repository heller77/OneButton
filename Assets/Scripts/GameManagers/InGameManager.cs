using System;
using Character;
using Character.LockOns;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Enemys;
using Enemys.Animations;
using GameManagers.EventImplements;
using GameManagers.EventImplements.PlayerDetector;
using GameManagers.ResultDisplays;
using GameManagers.ScoreCalculater;
using GameManagers.SeManagers;
using R3;
using TMPro;
using Tutorials;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using unityroom.Api;

namespace GameManagers
{
    /// <summary>
    /// インゲームのシーン遷移を管理（シーンといってもunityのシーンではなく、乗り物に乗るとかイベントを再生する的な意味）
    /// </summary>
    public class InGameManager : MonoBehaviour
    {
        [SerializeField] private PlayerRobotManager _playerRobotManager;

        [SerializeField] private FirstPositionDoorOpen _firstPositionDoorOpen;

        [SerializeField] private TextMeshProUGUI resultTextui;

        [SerializeField] private float bgmStartFadeTime = 1.0f;
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private LockOn _lockOn;

        [SerializeField] private TutorialUiManager _tutorialUiManager;

        private void Start()
        {
            StartGame();
        }

        /// <summary>
        /// ゲームを始める
        /// </summary>
        private async void StartGame()
        {
            _playerRobotManager.StopBattleMode();

            await RidingAndRobotPowerOn();
            await StartOpenStartDoorScene();
            await IntroductionDeparture();
            await Departure();
            await GetOutOfRobot();
            await DisplayScore();
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            SceneManager.LoadScene("Title");
        }

        [SerializeField] private PlayableDirector _playerdetector_ridingAndRobotPowerOn;
        private AudioPlayerID _bgmPlayerID;

        /// <summary>
        /// 乗って、ロボットの電源が付く
        /// </summary>
        private async UniTask RidingAndRobotPowerOn()
        {
            this._playerRobotManager.PowerOn();

            _playerdetector_ridingAndRobotPowerOn.Play();
            AudioManager.Instance.PlaySe(SeVariable.RobotOnSE, Vector3.zero, 0.1f);
            await UniTask.WaitForSeconds((float)_playerdetector_ridingAndRobotPowerOn.duration);
        }

        [SerializeField] private PlayerDetector _playerDetector_beyondDoor;

        /// <summary>
        /// 最初のドアを開ける
        /// </summary>
        private async UniTask StartOpenStartDoorScene()
        {
            //todo : ドアを開ける
            // await _firstPositionDoorOpen.OpenDoor();
            //
            // //ドアをあけたら
            // _playerRobotManager.StartMove();
            //
            // //ドアの向こうにプレイヤーが来るまでまって、来たら止める
            // // await _playerDetector_beyondDoor.WaitDetect();
            // Debug.Log("stop by ingamemanager");
            // this._playerRobotManager.StopMove();
        }

        [SerializeField] private PlayableDirector robotdepartureDirector;

        /// <summary>
        /// 出発前の説明
        /// </summary>
        private async UniTask IntroductionDeparture()
        {
            robotdepartureDirector.Play();
            await UniTask.WaitForSeconds(1.0f);
        }

        [SerializeField] private PlayerDetector arriveDetector_endPoint;

        /// <summary>
        /// バトルモードが始まる場所
        /// </summary>
        [SerializeField] private PlayerDetector battlemodeStartPoint;

        /// <summary>
        /// プレイヤーがここまでくるとボスが登場する場所
        /// </summary>
        [SerializeField] private PlayerDetector bossAppearPoint;

        [SerializeField] private BossGateAnimation _bossGateAnimation;

        /// <summary>
        /// 出発
        /// </summary>
        private async UniTask Departure()
        {
            _playerRobotManager.StartMove();
            //敵を動かし始める
            _enemyManager.StartMoveEnemys();

            //bgm始まる
            _bgmPlayerID = AudioManager.Instance.PlayBattleBGM(bgmStartFadeTime);

            battlemodeStartPoint.playerDetect.Subscribe(_ =>
            {
                //プレイヤーが攻撃できるように
                _playerRobotManager.StartBattleMode();
                _tutorialUiManager.DisplaySpaceKeyForSelectEnemy();
                //最初に敵を選択した時
                _playerRobotManager.SelectEnemy.Take(1).Subscribe(_ =>
                {
                    _tutorialUiManager.ExecuteFirstSpaceClickForSelectEnemy();
                    _tutorialUiManager.DisplaySpaceKeyForAttackEnemy();
                });
                //最初に敵を攻撃した時
                _playerRobotManager.Attack.Take(1).Subscribe(_ =>
                {
                    _tutorialUiManager.ExecuteSpaceClickForAttackEnemy();
                });
            });

            //ボス登場箇所までプレイヤーが来たら、ボスを登場させる
            bossAppearPoint.playerDetect.SubscribeAwait(async (_, ct) =>
            {
                _enemyManager.RemoveAllEnemy();
                _playerRobotManager.StopMove();
                await _bossGateAnimation.GateOpenAsync();


                _lockOn.StopAutoChangeCursor();
                _lockOn.Display();

                _playerRobotManager.StartMove();
                _lockOn.SelectTargetAbsolutely(_enemyManager.GetBossEnemy());
            });

            //最終地点につくまで待機
            await arriveDetector_endPoint.playerDetect.FirstAsync();
        }

        public PlayerDetector GetEndPointDetector()
        {
            return arriveDetector_endPoint;
        }

        [SerializeField] private CinemachineVirtualCamera getoutofvirtualCamera;

        /// <summary>
        /// 終了地点についたので、乗り物に降りる
        /// </summary>
        public async UniTask GetOutOfRobot()
        {
            // getoutofvirtualCamera.gameObject.SetActive(true);
            // float pathposition = 0.0f;
            // var dolly = getoutofvirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            // float speed = 0.2f;
            // while (true)
            // {
            //     dolly.m_PathPosition = pathposition;
            //     pathposition += speed * Time.deltaTime;
            //     await UniTask.DelayFrame(1);
            //     if (pathposition > 1.0f)
            //     {
            //         break;
            //     }
            // }
            _playerRobotManager.HideWeapon();
            //攻撃できないように
            _playerRobotManager.StopBattleMode();
        }

        [SerializeField] private ResultDisplay _resultDisplay;
        [SerializeField] private ScoreWeight _scoreWeight;
        [SerializeField] private float bgmStopFadeTime;

        public async UniTask DisplayScore()
        {
            AudioManager.Instance.StopSound(this._bgmPlayerID, bgmStopFadeTime);

            var battledata = BattleResultManager.GetInstance().GetBattleResultData();
            // Debug.Log(battledata);
            var scoreCalculater = new ScoreCalculater.ScoreCalculater(battledata, _scoreWeight);
            var score = scoreCalculater.Calculate();
            // this.resultTextui.text = score.ToString();

            //スコアを登録
            UnityroomApiClient.Instance.SendScore(1, scoreCalculater.GetScore().sum, ScoreboardWriteMode.HighScoreAsc);

            await _resultDisplay.resulting(scoreCalculater);
        }
    }
}