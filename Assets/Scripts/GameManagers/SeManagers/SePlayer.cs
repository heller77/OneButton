using UnityEngine;

namespace GameManagers.SeManagers
{
    /// <summary>
    /// seを再生する
    /// </summary>
    public class SePlayer : MonoBehaviour
    {
        private AudioSource source;

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