using GameManagers.AudioManagers;
using GameManagers.SeManagers.AudioVolumes;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    ///     音のボリュームを変更するスライダー
    /// </summary>
    public class AudioVolumeSlider : MonoBehaviour, IPointerUpHandler
    {
        private AudioVolume _audioVolume;
        private Slider _seslider;
        private readonly Subject<float> _sliderSubject = new Subject<float>();

        public Observable<float> sliderObservable
        {
            get { return _sliderSubject; }
        }

        private void Awake()
        {
            _seslider = GetComponent<Slider>();


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
            _sliderSubject.OnNext(_seslider.value);
        }
    }
}