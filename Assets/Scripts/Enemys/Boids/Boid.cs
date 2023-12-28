using System;
using UnityEngine;

namespace Enemys.Boids
{
    public class Boid : MonoBehaviour
    {
        private Vector3 _velocity;

        [SerializeField] private Vector3 _acceleration;
        private BoidParameter _parameter;

        public void SetAcceleration(Vector3 accel)
        {
            this._acceleration = accel;
        }

        public Vector3 GetAcceleration()
        {
            return this._acceleration;
        }

        public void SetBoidParameter(BoidParameter parameter)
        {
            this._parameter = parameter;
        }

        public Vector3 GetVelocity()
        {
            return this._velocity;
        }

        private void Start()
        {
            this._velocity = Vector3.zero;
        }

        private void Update()
        {
            MoveByAccelaration();
        }

        /// <summary>
        /// 加速度によって移動させる
        /// </summary>
        public void MoveByAccelaration()
        {
            _velocity += _acceleration * Time.deltaTime;
            var dir = _velocity.normalized;
            float speed = _velocity.magnitude;
            _velocity = Mathf.Clamp(speed, _parameter.minSpeed, _parameter.maxSpeed) * dir;

            var newPos = transform.position + _velocity;
            // Debug.Log("velocity : " + _velocity + " accelaration : " + _acceleration);

            var rotation = Quaternion.LookRotation(_velocity);
            transform.SetPositionAndRotation(newPos, rotation);
        }
    }
}