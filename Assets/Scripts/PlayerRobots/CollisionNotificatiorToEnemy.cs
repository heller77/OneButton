﻿using System;
using Enemys;
using UnityEngine;

namespace Character
{
    public class CollisionNotificatiorToEnemy : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Detector>(out var detector))
            {
                detector.Detect();
            }
        }
    }
}