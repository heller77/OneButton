using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using R3;

namespace GameManagers.SeManagers
{
    public enum SeVariable
    {
        normalbulletFireSE = 0,
        RobotOnSE = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public struct AudioPlayerID
    {
        public readonly AudioPlayer audioPlayer;
        public readonly int audioPlayId;

        public AudioPlayerID(AudioPlayer audioPlayer, int audioPlayId)
        {
            this.audioPlayer = audioPlayer;
            this.audioPlayId = audioPlayId;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private R3.SerializableReactiveProperty<float> seVolume;
        [SerializeField] private R3.SerializableReactiveProperty<float> bgmVolume;

        [SerializeField] private AudioClip bulletse;
        [SerializeField] private AudioClip robotOnSe;

        [SerializeField] private GameObject audioPlayerGameObject;

        private List<AudioPlayer> sePlayersList;
        [SerializeField] private int audioPlayerCount = 1;

        [SerializeField] private AudioClip bgmSource;
        private AudioPlayer bgmPlayer;

        private Dictionary<int, AudioClip> audioClips;

        private void Awake()
        {
            sePlayersList = new List<AudioPlayer>(audioPlayerCount);
            for (var i = 0; i < audioPlayerCount; i++)
            {
                var audioPlayer = Instantiate(audioPlayerGameObject);
                sePlayersList.Add(audioPlayer.GetComponent<AudioPlayer>());
                sePlayersList[i].Initialize(audioPlayer.GetComponent<AudioSource>());
            }

            bgmPlayer = Instantiate(audioPlayerGameObject).GetComponent<AudioPlayer>();
            bgmPlayer.Initialize(bgmPlayer.GetComponent<AudioSource>());


            //sevolume変えたら伝える
            seVolume.Subscribe((seVolume) =>
            {
                foreach (var audioPlayer in sePlayersList)
                {
                    audioPlayer.SetVolume(seVolume);
                }
            });


            //bgmVolumeを変えたら伝える
            bgmVolume.Subscribe((bgmvolume) =>
            {
                Debug.Log("bgmvalue change!!");
                bgmPlayer.SetVolume(bgmvolume);
            });

            this.audioClips = new Dictionary<int, AudioClip>()
            {
                { (int)SeVariable.RobotOnSE, robotOnSe },

                { (int)SeVariable.normalbulletFireSE, bulletse },
            };
        }

        private AudioPlayer GetUnusedAudioPlayer()
        {
            for (var i = 0; i < audioPlayerCount; ++i)
            {
                if (sePlayersList[i].GetIsPlaying() == false)
                {
                    return sePlayersList[i];
                }
            }

            return null;
        }

        public AudioClip GetSeAudioClip(SeVariable seVariable)
        {
            return this.audioClips[(int)seVariable];
        }

        /// <summary>
        /// 音を再生するだけ
        /// </summary>
        /// <param name="audioPlayer"></param>
        /// <param name="clip"></param>
        private void Play(AudioPlayer audioPlayer, AudioClip clip, float audioVolume, bool isFade, float fadeTime = 1)
        {
            audioPlayer.Play(clip, audioVolume, isFade, fadeTime);
        }

        public AudioPlayerID PlaySe(SeVariable seVariable, Vector3 sePosition)
        {
            var audiosPlayer = GetUnusedAudioPlayer();
            if (audiosPlayer == null)
            {
                return　new AudioPlayerID();
            }

            audiosPlayer.SetPosition(sePosition);

            Play(audiosPlayer, GetSeAudioClip(seVariable), seVolume.Value, false);
            return new AudioPlayerID(audiosPlayer, audiosPlayer.GetNowPlaySouncCount());
        }

        public AudioPlayerID PlayBattleBGM(bool fade, float fadeTime)
        {
            Play(bgmPlayer, bgmSource, bgmVolume.Value, fade, fadeTime);
            return new AudioPlayerID(bgmPlayer, bgmPlayer.GetNowPlaySouncCount());
        }

        public void StopSound(AudioPlayerID id, float fadeTime)
        {
            id.audioPlayer.Stop(id.audioPlayId, fadeTime);
        }
    }
}