using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Utils;

public enum EnemyType
{
    mob,
    mother
}

namespace Enemys
{
    public class MobEnemyGroup : MonoBehaviour, ITrapable
    {
        [SerializeField] private List<MobEnemy> mobEnemies;

        [SerializeField] private Transform parentTransform;

        [SerializeField] private SplineAnimate _splineAnimate;

        public void MoveStart()
        {
            _splineAnimate.Play();
        }

        public void Boot()
        {
            MoveStart();
        }
    }
}