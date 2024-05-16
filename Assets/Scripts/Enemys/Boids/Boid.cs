using GameLoops;
using UnityEngine;

namespace Enemys.Boids
{
    /// <summary>
    ///     モブ敵の動きを管理する
    /// </summary>
    public class Boid : MonoBehaviour, ITickable
    {
        [SerializeField] private Vector3 _acceleration;
        private BoidsManager _manager;
        private BoidParameter _parameter;
        private Vector3 _velocity;

        private void Start()
        {
            _velocity = Vector3.zero;
        }

        public void Tick()
        {
            MoveByAccelaration();
        }

        public Vector3 GetAcceleration()
        {
            return _acceleration;
        }

        public void SetAcceleration(Vector3 accel)
        {
            _acceleration = accel;
        }

        public void SetBoidManager(BoidsManager boidsManager)
        {
            _manager = boidsManager;
        }

        public void SetBoidParameter(BoidParameter parameter)
        {
            _parameter = parameter;
        }

        /// <summary>
        ///     破壊
        /// </summary>
        public void Destruction()
        {
            _manager.RemoveBoid(this);
        }

        public Vector3 GetVelocity()
        {
            return _velocity;
        }

        /// <summary>
        ///     加速度によって移動させる
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