using UnityEngine;

namespace GameManagers.SeManagers
{
    [CreateAssetMenu(fileName = "sedata", menuName = "ScriptableObjects/sedata")]
    public class SeData : ScriptableObject
    {
        [SerializeField] public AudioClip bulletse;
        [SerializeField] public AudioClip robotOnSe;
        [SerializeField] public AudioClip canonSe;
        [SerializeField] public AudioClip enemyDeathSe;
    }
}