using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagers.ScoreCalculater;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace GameManagers.ResultDisplays
{
    public class ResultDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject resultCameraParentGameObject;

        [SerializeField] private PlayableDirector resultTimelineDirector;

        [SerializeField] private CinemachineVirtualCamera _resultCinemachineVirtualCamera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        [SerializeField] private Material exposure;

        /// <summary>
        /// カメラ切り替えが終わった後にリザルト表示までの時間（秒）
        /// </summary>
        [SerializeField] private float CameraMoveEndmarginTime = 1f;

        /// <summary>
        ///スコアの詳細を表示するUI
        /// </summary>
        [SerializeField] private TextMeshProUGUI resultScoreTextUi;

        /// <summary>
        /// スコアの合計を表示するUI
        /// </summary>
        [SerializeField] private TextMeshProUGUI scoreSumTextUi;

        /// <summary>
        /// リザルト表示
        /// </summary>
        public async UniTask resulting(ScoreCalculater.ScoreCalculater scoreCalculater)
        {
            _resultCinemachineVirtualCamera.gameObject.SetActive(true);

            // exposure.DOFloat(1.0f, "_exposure", 1);
            Debug.Log($"{_cinemachineBrain.m_DefaultBlend.m_Time}");
            await UniTask.Delay(
                TimeSpan.FromSeconds(_cinemachineBrain.m_DefaultBlend.m_Time + CameraMoveEndmarginTime));

            var battleData = scoreCalculater.GetBattleData();
            var scoreData = scoreCalculater.GetScore();
            Debug.Log("scoreData.sun " + scoreData.sum);
            resultScoreTextUi.text = $"mob : {scoreData.knockdownMobScore} \n" +
                                     $"survive : {scoreData.survivePlayerSideRobotScore} \n" +
                                     $"boss : {scoreData.bossDamageScore} \n" +
                                     $"bullet : -{scoreData.consumeBulletMinuxScore}";
            scoreSumTextUi.text = $"sum {scoreData.sum}";

            //カメラON
            resultCameraParentGameObject.SetActive(true);
            resultTimelineDirector.Play();
        }
    }
}