using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagers;
using MyInputs;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace TitleScene
{
    /// <summary>
    ///     タイトルシーンの管理をする
    /// </summary>
    public class TitleSceneManager : MonoBehaviour
    {
        private static readonly int AddValue = Shader.PropertyToID("_AddValue");

        [SerializeField] private GameSceneManager _gameSceneManager;

        [SerializeField] private Image blackScreen;
        [SerializeField] private GameObject loadCircle;
        [SerializeField] private Material loadingCircleMaterial;

        [SerializeField] private CinemachineVirtualCamera _loading_virtualCamera;

        /// <summary>
        ///     最初のシネマシン（固定）
        /// </summary>
        [SerializeField] private GameObject defaultCinemachine;

        [SerializeField] private GameObject trackedCinemachine;
        [SerializeField] private CanvasGroup tittleUICanvasGroup;
        [SerializeField] private Image concentrationLineImage;

        [SerializeField] private float max;
        [SerializeField] private float duration;

        /// <summary>
        ///     ローディング画面を表示するUI(rendertextureを表示したimage)
        /// </summary>
        [SerializeField] private Image loadingUIPanel;

        [SerializeField] private PlayableDirector ride_timeline_playabledirector;

        //ローディング画面でのFade時間
        private readonly float _blackscreenFadeTime = 1;
        readonly float _loadingGaugeAnimationTime = 5;

        private readonly string _materialPropertyGauge = "_gauge";
        private float _gaugeEnd = 0.5f;
        private readonly float _gaugeStart = 1.6f;
        private CinemachineTrackedDolly _locadingcameradolly;

        //集中線マテリアルのプロパティブロック
        // private MaterialPropertyBlock concentrationLineMaterialPropertyBlock;
        // private int concentrationLineMaterial_PropertyID;
        private Material concentrationLineMaterial;

        readonly Subject<Unit> endCameraMoveSubject = new Subject<Unit>();
        private GameInputs input;

        private void Start()
        {
            input = new GameInputs();
            input.Player.PushButton.started += GoToInGame;
            input.Enable();
            _locadingcameradolly = _loading_virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            // concentrationLineMaterialPropertyBlock = new MaterialPropertyBlock();
            // concentrationLineMaterial_PropertyID = Shader.PropertyToID("_scale");
            var concentrationmaterial = concentrationLineImage.material;
            concentrationLineImage.material = new Material(concentrationmaterial);
            concentrationLineMaterial = concentrationLineImage.material;
        }

        private void GoToInGame(InputAction.CallbackContext obj)
        {
            GoToInGame();
        }

        /// <summary>
        ///     インゲームに遷移。
        /// </summary>
        private async UniTaskVoid GoToInGame()
        {
            //入力を無視
            input.Disable();
            //タイトルに表示されているUIを非表示に
            await tittleUICanvasGroup.DOFade(endValue: 0f, duration: _blackscreenFadeTime);

            //集中線
            DisplayconcentrationLine();
            // concentrationLineMeshRender.SetPropertyBlock(concentrationLineMaterialPropertyBlock);
            //カメラ操作
            defaultCinemachine.SetActive(false);
            loadingCircleMaterial.SetFloat(_materialPropertyGauge, _gaugeStart);
            GotoExit();


            var asyncOperation = _gameSceneManager.GoToInGameScene();
// await blackScreen.DOFade(endValue: 1f, duration: _blackscreenFadeTime);
            // loadCircle.SetActive(true);
            // await this.loadingCircleMaterial.DOFloat(_gaugeEnd, _materialPropertyGauge,
            //     _loadingGaugeAnimationTime);

            Debug.Log("move await");
            await endCameraMoveSubject.FirstAsync();
            Debug.Log("move end");
            HideconcentrationLine();

            //ローディング画面の表示
            await DisplayLoadingWindow();

            //メインシーンに遷移
            asyncOperation.allowSceneActivation = true;
            Debug.Log("go to main");
        }

        /// <summary>
        ///     出口までカメラを持っていき、ローディング画面に遷移
        /// </summary>
        private async UniTask GotoExit()
        {
            float speedPerSecond = max / duration;
            float frameTime = 0.1f;
            for (int i = 0; i < duration / frameTime; i++)
            {
                _locadingcameradolly.m_PathPosition += speedPerSecond * frameTime;

                await UniTask.WaitForSeconds(frameTime);
            }

            endCameraMoveSubject.OnNext(Unit.Default);
        }

        private async UniTask DisplayconcentrationLine()
        {
            // concentrationLineMaterial.SetFloat("_scale", 3.0f);
            concentrationLineMaterial.DOFloat(3.0f, "_scale", duration / 2).SetEase(Ease.InOutQuart);
        }

        private async UniTask HideconcentrationLine()
        {
            concentrationLineMaterial.DOFloat(1.0f, "_scale", 0.7f);
        }

        private async UniTask DisplayLoadingWindow()
        {
            loadingUIPanel.gameObject.SetActive(true);
            var material = loadingUIPanel.material;
            var newmaterial = new Material(material);
            loadingUIPanel.material = newmaterial;

            //カメラの映像を真っ白から、暗い色に
            newmaterial.SetFloat(AddValue, 1.0f);
            await newmaterial.DOFloat(0.0f, AddValue, 1.0f);
            //ライドシーン再生
            ride_timeline_playabledirector.Play();
            while (true)
            {
                Debug.Log("play timelien ");
                if (ride_timeline_playabledirector.time >= ride_timeline_playabledirector.duration)
                {
                    break;
                }

                await UniTask.DelayFrame(1);
            }
        }
    }
}