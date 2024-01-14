using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Enemys
{
    public class BossEnemy : MonoBehaviour, ITrapable
    {
        [SerializeField] private GameObject defaultObject;

        [SerializeField] private GameObject destructionObject;

        [SerializeField] private List<Rigidbody> parts;
        [SerializeField] private float destructionPower = 1.0f;
        [SerializeField] private Transform powerOriginTransform;
        [SerializeField] private ParticleSystem _particleSystem;

        private void Start()
        {
            Destruction();
        }

        public void Boot()
        {
        }

        public void Destruction()
        {
            _particleSystem.Play();
            defaultObject.SetActive(false);
            destructionObject.SetActive(true);
            foreach (var part in parts)
            {
                var powerDir = part.transform.position - powerOriginTransform.transform.position;
                Debug.Log(powerDir);
                part.AddForce(destructionPower * powerDir);
            }
        }
    }
}