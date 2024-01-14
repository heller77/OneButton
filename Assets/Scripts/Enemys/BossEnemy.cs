using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Enemys
{
    public class BossEnemy : MonoBehaviour, ITrapable, IHitable
    {
        [SerializeField] private GameObject defaultObject;

        [SerializeField] private GameObject destructionObject;

        [SerializeField] private List<Rigidbody> parts;
        [SerializeField] private float destructionPower = 1.0f;
        [SerializeField] private Transform powerOriginTransform;
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private float hp = 1;


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
                part.AddForce(destructionPower * powerDir);
            }
        }

        public void Hitted(float damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                this.Destruction();
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}