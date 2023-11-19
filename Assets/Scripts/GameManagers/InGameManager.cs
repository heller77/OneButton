using Character;
using Cinemachine;
using Cysharp.Threading.Tasks;
using GameManagers.EventImplements;
using GameManagers.EventImplements.PlayerDetector;
using UnityEngine;
using UnityEngine.Playables;

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
            await GetOutOfRobot();
        }

        [SerializeField] private CinemachineVirtualCamera ridescne_virtualcamera;

        /// <summary>
        /// ライド
        /// </summary>
        private async UniTask StartRideScene()
        {
            float pathposition = 0.0f;
            var dolly = ridescne_virtualcamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            float speed = 0.2f;
            while (true)
            {
                dolly.m_PathPosition = pathposition;
                pathposition += speed * Time.deltaTime;
                await UniTask.DelayFrame(1);
                if (pathposition > 1.0f)
                {
                    break;
                }
            }

            ridescne_virtualcamera.gameObject.SetActive(false);
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

        [SerializeField] private CinemachineVirtualCamera getoutofvirtualCamera;

        /// <summary>
        /// 終了地点についたので、乗り物に降りる
        /// </summary>
        public async UniTask GetOutOfRobot()
        {
            getoutofvirtualCamera.gameObject.SetActive(true);
            float pathposition = 0.0f;
            var dolly = getoutofvirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            float speed = 0.2f;
            while (true)
            {
                dolly.m_PathPosition = pathposition;
                pathposition += speed * Time.deltaTime;
                await UniTask.DelayFrame(1);
                if (pathposition > 1.0f)
                {
                    break;
                }
            }
        }
    }
}