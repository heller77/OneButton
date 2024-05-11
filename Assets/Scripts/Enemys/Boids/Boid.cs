using System;
using GameLoops;
using UnityEngine;

namespace Enemys.Boids
{
    /// <summary>
    /// モブ敵の動きを管理する
    /// </summary>
    public class Boid : MonoBehaviour, ITickable
    {
        private Vector3 _velocity;

        [SerializeField] private Vector3 _acceleration;
        private BoidParameter _parameter;
        private BoidsManager _manager;

        public Vector3 GetAcceleration()
        {
            return this._acceleration;
        }

        public void SetAcceleration(Vector3 accel)
        {
            this._acceleration = accel;
        }

        public void SetBoidManager(BoidsManager boidsManager)
        {
            this._manager = boidsManager;
        }

        public void SetBoidParameter(BoidParameter parameter)
        {
            this._parameter = parameter;
        }
        

        /// <summary>
        /// 破壊
        /// </summary>
        public void Destruction()
        {
            _manager.RemoveBoid(this);
        }

        public Vector3 GetVelocity()
        {
            return this._velocity;
        }

        private void Start()
        {
            this._velocity = Vector3.zero;
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

        public void Tick()
        {
            MoveByAccelaration();
        }
    }
}