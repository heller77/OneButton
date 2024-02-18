using System;
using UnityEngine;

namespace GameManagers.SeManagers
{
    public class SeManager : MonoBehaviour
    {
        [SerializeField] private float seVolume = 1;
        [SerializeField] private float bgmVolume = 1;

        [SerializeField] private AudioClip bulletse;

        private SePlayer[] audioSourceList = new SePlayer[20];

        private void Awake()
        {
            //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
            for (var i = 0; i < audioSourceList.Length; ++i)
            {
                audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlayBuuletSe(Transform)
            {
                sePlayer.Play(bulletse);
            }
        }
    }