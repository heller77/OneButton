using System;
using Character;
using Cinemachine;
using Cysharp.Threading.Tasks;
using GameManagers.EventImplements;
using GameManagers.EventImplements.PlayerDetector;
using GameManagers.ScoreCalculater;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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

        private void Start()
        {
            StartGame();
        }

        /// <summary>
        /// ゲームを始める
        /// </summary>
        private async void StartGame()
        {
            await StartOpenStartDoorScene();
            await IntroductionDeparture();
            await Departure();
            await GetOutOfRobot();
            await DisplayScore();
            await UniTask.Delay(TimeSpan.FromSeconds(10));
            SceneManager.LoadScene("Title");
        }

        [SerializeField] private CinemachineVirtualCamera ridescne_virtualcamera;

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
            Debug.Log("倒してこい！");
            robotdepartureDirector.Play();
            await UniTask.WaitForSeconds(1.0f);
        }

        [SerializeField] private PlayerDetector depature_endPoint;

        /// <summary>
        /// 出発
        /// </summary>
        private async UniTask Departure()
        {
            Debug.Log("出発");
            _playerRobotManager.StartMove();
            //終点に到着
            await depature_endPoint.WaitDetect();
            Debug.Log("終了");
        }

        public PlayerDetector GetEndPointDetector()
        {
            return depature_endPoint;
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
        }

        [SerializeField] private ResultDisplay _resultDisplay;
        [SerializeField] private ScoreWeight _scoreWeight;

        public async UniTask DisplayScore()
        {
            var battledata = BattleResultManager.GetInstance().GetBattleResultData();
            // Debug.Log(battledata);
            var scoreCalculater = new ScoreCalculater.ScoreCalculater(battledata, _scoreWeight);
            var score = scoreCalculater.Calculate();
            // this.resultTextui.text = score.ToString();
            await _resultDisplay.resulting(scoreCalculater);
        }
    }
}