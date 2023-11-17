using Character;
using Cysharp.Threading.Tasks;
using GameManagers.EventImplements;
using GameManagers.EventImplements.PlayerDetector;
using UnityEngine;

namespace GameManagers
{
    /// <summary>
    /// インゲームのシーン遷移を管理（シーンといってもunityのシーンではなく、乗り物に乗るとかイベントを再生する的な意味）
    /// </summary>
    public class InGameManager : MonoBehaviour
    {
        [SerializeField] private PlayerRobotManager _playerRobotManager;

        [SerializeField] private FirstPositionDoorOpen _firstPositionDoorOpen;


        private void Start()
        {
            StartGame();
        }

        /// <summary>
        /// ゲームを始める
        /// </summary>
        private async void StartGame()
        {
            await StartRideScene();
            await StartOpenStartDoorScene();
            await IntroductionDeparture();
            await Departure();
        }


        /// <summary>
        /// ライド
        /// </summary>
        private async UniTask StartRideScene()
        {
        }

        [SerializeField] private PlayerDetector _playerDetector_beyondDoor;

        /// <summary>
        /// 最初のドアを開ける
        /// </summary>
        private async UniTask StartOpenStartDoorScene()
        {
            //todo : ドアを開ける
            await _firstPositionDoorOpen.OpenDoor();

            //ドアをあけたら
            _playerRobotManager.StartMove();

            //ドアの向こうにプレイヤーが来るまでまって、来たら止める
            await _playerDetector_beyondDoor.WaitDetect();
            Debug.Log("stop by ingamemanager");
            this._playerRobotManager.StopMove();
        }

        /// <summary>
        /// 出発前の説明
        /// </summary>
        private async UniTask IntroductionDeparture()
        {
            Debug.Log("倒してこい！");
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
    }
}