using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagers.SeManagers
{
    [Serializable]
    public class SoundEffect
    {
        public SeVariable soundType;
        public AudioClip audioClip;
    }

    [CreateAssetMenu(fileName = "sedata", menuName = "ScriptableObjects/sedata")]
    public class SeData : ScriptableObject
    {
        [SerializeField] private List<SoundEffect> seList = new List<SoundEffect>();

        public List<SoundEffect> GetSoundEffectList()
        {
            return this.seList;
        }
    }
}