using UnityEngine;

namespace GameManagers.SeManagers.AudioVolumes
{
    public class AudioVolume
    {
        private float volume = 0.1f;

        private string volumeText = "volume";

        public void Initialize()
        {
            //volumeが保存されていたら、それで初期化
            if (PlayerPrefs.HasKey(volumeText))
            {
                this.volume = PlayerPrefs.GetFloat(volumeText);
            }
        }

        public float GetVolume()
        {
            return this.volume;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(volumeText, this.volume);
            PlayerPrefs.Save();
        }
    }
}