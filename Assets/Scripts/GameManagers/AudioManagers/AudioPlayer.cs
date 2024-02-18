using System;
using DG.Tweening;
using UnityEngine;

namespace GameManagers.SeManagers
{
    /// <summary>
    /// seを再生する
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource source;

        public void Initialize(AudioSource source)
        {
            this.source = source;
        }

        //今再生中かどうか
        public bool GetIsPlaying()
        {
            return source.isPlaying;
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        /// <summary>
        /// clipを再生する
        /// </summary>
        /// <param name="clip"></param>
        public void Play(AudioClip clip, float audioVolume, bool isFade = false, float duration = 1)
        {
            if (isFade)
            {
                source.volume = 0;
                source.clip = clip;
                source.Play();
                source.DOFade(audioVolume, duration);
            }
            else
            {
                source.volume = audioVolume;
                source.clip = clip;
                source.Play();
            }

            nowPlaySoundCount++;
        }

        private int nowPlaySoundCount = 0;

        public void Stop(int id, float fadeTime)
        {
            if (nowPlaySoundCount == id)
            {
                source.DOFade(0, fadeTime).OnComplete(() => {source.Stop(); });
            }
        }

        public int GetNowPlaySouncCount()
        {
            return nowPlaySoundCount;
        }

        public void SetVolume(float volume)
        {
            this.source.volume = volume;
        }
    }
}