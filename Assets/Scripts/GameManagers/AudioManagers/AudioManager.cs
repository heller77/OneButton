using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameManagers.SeManagers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private float seVolume = 1;
        [SerializeField] private float bgmVolume = 1;

        [SerializeField] private AudioClip bulletse;

        private AudioPlayer[] audioPlayersList = new AudioPlayer[20];

        private void Awake()
        {
            for (var i = 0; i < audioPlayersList.Length; ++i)
            {
                var gameobject = Instantiate(transform);
                audioPlayersList[i] = gameObject.AddComponent<AudioPlayer>();
                var audioSource = gameobject.AddComponent<AudioSource>();
                audioPlayersList[i].Initialize(audioSource);
            }
        }

        private AudioPlayer GetUnusedAudioPlayer()
        {
            for (var i = 0; i < audioPlayersList.Length; ++i)
            {
                if (audioPlayersList[i].GetIsPlaying() == false)
                {
                    return audioPlayersList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 音を再生するだけ
        /// </summary>
        /// <param name="audioPlayer"></param>
        /// <param name="clip"></param>
        private void Play(AudioPlayer audioPlayer, AudioClip clip)
        {
            audioPlayer.Play(clip);
        }

        public void PlayBuuletSe(Vector3 sePosition)
        {
            var audiosPlayer = GetUnusedAudioPlayer();
            if (audiosPlayer == null)
            {
                return;
            }

            audiosPlayer.SetPosition(sePosition);

            Play(audiosPlayer, bulletse);
        }
    }
}