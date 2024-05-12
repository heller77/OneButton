using UnityEngine;

namespace GameManagers.SeManagers.AudioVolumes
{
    /// <summary>
    ///     音量を管理
    ///     保存して、Initializeで読み込む
    /// </summary>
    public class AudioVolume
    {
        private float volume = 0.1f;

        private readonly string volumeText = "volume";

        public void Initialize()
        {
            //volumeが保存されていたら、それで初期化
            if (PlayerPrefs.HasKey(volumeText))
            {
                volume = PlayerPrefs.GetFloat(volumeText);
            }
        }

        public float GetVolume()
        {
            return volume;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(volumeText, volume);
            PlayerPrefs.Save();
        }
    }
}