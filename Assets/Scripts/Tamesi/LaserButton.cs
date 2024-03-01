using System;
using Character.Weapon.Lasers;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Tamesi
{
    public class LaserButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Laser _laser;

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => { _laser.LaunchLaser(); });
        }
    }
}