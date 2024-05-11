using System;
using GameManagers.AudioManagers;
using GameManagers.SeManagers;
using R3;
using UI;
using UnityEngine;

namespace TitleScene
{
    public class AudioVolumeSliderChangeEventSet : MonoBehaviour
    {
        [SerializeField] private AudioVolumeSlider _audioVolumeSlider;
        [SerializeField] private ParticleSystem _particleSystem;

        /// <summary>
        /// seが鳴る場所
        /// ロボットアームがびりびりしているところ
        /// </summary>
        [SerializeField] private Transform sePlace;

        [SerializeField] private float emitMax = 30;
        [SerializeField] private float emitMin = 5;

        [SerializeField] private Animator armAnimator;
        private string moveArmState = "MoveArm";

        private void Start()
        {
            _audioVolumeSlider.sliderObservable.Subscribe(volume =>
            {
                int emitNum = (int)((emitMax - emitMin) * volume + emitMin);
                //アームの先から火花が散る
                _particleSystem.Emit(emitNum);
                //アームを動かす
                armAnimator.Play(moveArmState, 0, 0.0f);
                //seを再生
                AudioManager.Instance.PlaySe(SeVariable.CanonSe, sePlace.position, 1);
            });
        }
    }
}