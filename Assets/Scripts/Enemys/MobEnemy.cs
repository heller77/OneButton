using System;
using UnityEngine;

namespace Enemys
{
    public class MobEnemy : MonoBehaviour, ITarget
    {
        [SerializeField] private EnemyManager _enemyManager;

        private void Start()
        {
            _enemyManager.Add(this);
        }

        public Vector3 GetPosition()
        {
            return this.transform.position;
        }
    }
}