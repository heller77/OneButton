using System;
using GameManagers.AudioManagers;
using GameManagers.SeManagers;
using GameManagers.SeManagers.AudioVolumes;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Observable = UnityEngine.InputSystem.Utilities.Observable;

namespace UI
{
    /// <summary>
    /// 音のボリュームを変更するスライダー
    /// </summary>
    public class AudioVolumeSlider : MonoBehaviour, IPointerUpHandler
    {
        private Slider _seslider;

        private AudioVolume _audioVolume;
        private Subject<float> _sliderSubject = new Subject<float>();

        public Observable<float> sliderObservable
        {
            get { return this._sliderSubject; }
        }

        private void Awake()
        {
            this._seslider = GetComponent<Slider>();


            _audioVolume = new AudioVolume();
            _audioVolume.Initialize();
            _seslider.value = _audioVolume.GetVolume();


            _sliderSubject.Subscribe(_ =>
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetAudioVolume(_seslider.value);
                }

                _audioVolume.SetVolume(_seslider.value);
                _audioVolume.Save();
            }).AddTo(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _sliderSubject.OnNext(this._seslider.value);
        }
    }
}