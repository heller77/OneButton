using System.Collections.Generic;
using GameManagers.SeManagers;
using GameManagers.SeManagers.AudioVolumes;
using R3;
using UnityEngine;

namespace GameManagers.AudioManagers
{
    /// <summary>
    /// seのenum
    /// enumとseが紐づけられてAudioManagerで利用可能
    /// </summary>
    public enum SeVariable
    {
        normalbulletFireSE = 0,
        RobotOnSE = 1,
        CanonSe = 2,
        EnemyDeath = 3
    }

    /// <summary>
    /// 音を再生するAudioPlayerの識別子
    /// AudioManaerで再生して、それを止める時のAudioPlayerへの参照
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

    /// <summary>
    /// 音の再生を管理する
    /// 
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private R3.SerializableReactiveProperty<float> seVolume;
        [SerializeField] private R3.SerializableReactiveProperty<float> bgmVolume;

        /// <summary>
        /// enumに紐づけられたseのデータ
        /// enum指定で呼び出したいseがまとめられている
        /// </summary>
        [SerializeField] private SeData _seData;

        [SerializeField] private GameObject sePlayerPrefab;
        [SerializeField] private GameObject bgmPlayerPrefab;

        private List<AudioPlayer> sePlayersList;

        /// <summary>
        /// 同時に再生するseの数
        /// </summary>
        [SerializeField] private int audioPlayerCount = 1;

        [SerializeField] private AudioClip bgmSource;
        private AudioPlayer bgmPlayer;

        private Dictionary<int, AudioClip> audioClips = new Dictionary<int, AudioClip>();

        private AudioVolume _volumemanager;

        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _instance = this;
            sePlayersList = new List<AudioPlayer>(audioPlayerCount);
            for (var i = 0; i < audioPlayerCount; i++)
            {
                var audioPlayer = Instantiate(sePlayerPrefab);
                sePlayersList.Add(audioPlayer.GetComponent<AudioPlayer>());
                sePlayersList[i].Initialize(audioPlayer.GetComponent<AudioSource>());
            }

            bgmPlayer = Instantiate(bgmPlayerPrefab).GetComponent<AudioPlayer>();
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
            bgmVolume.Subscribe((bgmvolume) => { bgmPlayer.SetVolume(bgmvolume); });

            audioClips = new Dictionary<int, AudioClip>();
            foreach (SoundEffect se in _seData.GetSoundEffectList())
            {
                var type = (int)se.soundType;
                audioClips[type] = se.audioClip;
            }

            //volumemanager生成、初期化
            this._volumemanager = new AudioVolume();
            _volumemanager.Initialize();
            SetAudioVolume(_volumemanager.GetVolume());
        }

        public void SetAudioVolume(float volume)
        {
            this.seVolume.Value = volume;
            this.bgmVolume.Value = 0.2f * volume;
        }

        /// <summary>
        /// 使っていないAudioPlayerを取得
        /// </summary>
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

        private AudioClip GetSeAudioClip(SeVariable seVariable)
        {
            return this.audioClips[(int)seVariable];
        }

        /// <summary>
        /// 音を再生するだけ
        /// </summary>
        /// <param name="audioPlayer"></param>
        /// <param name="clip"></param>
        private void Play(AudioPlayer audioPlayer, AudioClip clip, float audioVolume, float fadeTime = 1)
        {
            audioPlayer.Play(clip, audioVolume, fadeTime);
        }

        /// <summary>
        /// enumで再生するAudioClipを指定して再生
        /// </summary>
        /// <param name="seVariable">seを指定するenum(enumにseが一つ紐づけられている)</param>
        /// <param name="sePosition">再生する場所</param>
        /// <param name="fadeTime">再生する時の始まりのフェード時間（急にはならないように）</param>
        /// <returns></returns>
        public AudioPlayerID PlaySe(SeVariable seVariable, Vector3 sePosition, float fadeTime)
        {
            return PlaySe(GetSeAudioClip(seVariable), sePosition, fadeTime);
        }

        /// <summary>
        /// AudioClipを指定して、それを再生する
        /// </summary>
        /// <param name="clip">再生すオーディオクリップ</param>
        /// <param name="sePosition">再生する場所</param>
        /// <param name="fadeTime">再生する時の始まりのフェード時間（急にはならないように）</param>
        /// <returns></returns>
        public AudioPlayerID PlaySe(AudioClip clip, Vector3 sePosition, float fadeTime)
        {
            var audiosPlayer = GetUnusedAudioPlayer();
            if (audiosPlayer == null)
            {
                return　new AudioPlayerID();
            }

            audiosPlayer.SetPosition(sePosition);
            Play(audiosPlayer, clip, seVolume.Value, fadeTime);
            return new AudioPlayerID(audiosPlayer, audiosPlayer.GetNowPlaySouncCount());
        }

        public AudioPlayerID PlayBattleBGM(float fadeTime)
        {
            Play(bgmPlayer, bgmSource, bgmVolume.Value, fadeTime);
            return new AudioPlayerID(bgmPlayer, bgmPlayer.GetNowPlaySouncCount());
        }

        public void StopSound(AudioPlayerID id, float fadeTime)
        {
            id.audioPlayer.Stop(id.audioPlayId, fadeTime);
        }
    }
}