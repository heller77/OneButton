using UnityEngine;

namespace GameManagers.SeManagers
{
    /// <summary>
    /// seを再生する
    /// </summary>
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
        public void Play(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }
    }
}