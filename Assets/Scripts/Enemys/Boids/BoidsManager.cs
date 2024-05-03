using System;
using System.Collections.Generic;
using GameLoops;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemys.Boids
{
    /// <summary>
    ///     Boidsクラスを管理
    /// </summary>
    public class BoidsManager : MonoBehaviour, GameLoops.IInitializable, ITickable
    {
        private readonly List<Boid> boidList = new List<Boid>();
        [SerializeField] private GameObject boidPrefab;
        [SerializeField] private BoidParameter _boidParameter;

        [SerializeField] private Transform target;

        [SerializeField] private EnemyManager _enemyManager;

        private void Start()
        {
            _enemyManager.AddBoidManager(this);
        }

        public void Initialize()
        {
            //シード値を指定
            Random.InitState(10);

            for (int i = 0; i < _boidParameter.boidsCount; i++)
            {
                AddBoid();
            }
        }

        public void Tick()
        {
            //動き計算
            ControlBoids();

            foreach (var boid in this.boidList)
            {
                boid.Tick();
            }
        }

        /// <summary>
        /// Boidを生成
        /// </summary>
        public void AddBoid()
        {
            GameObject boid = Instantiate(boidPrefab, transform.position + 10 * Random.onUnitSphere, Random.rotation);
            var boidcomponent = boid.GetComponent<Boid>();
            var mobenemy = boid.GetComponent<MobEnemy>();
            mobenemy.SetEnemyManager(_enemyManager);

            mobenemy.SetBoid(boidcomponent);
            boidcomponent.SetBoidManager(this);

            boidList.Add(boidcomponent);
            boidcomponent.SetBoidParameter(this._boidParameter);
            //初期加速度設定
            Vector3 initAccel = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            boidcomponent.SetAcceleration(boid.transform.forward);
        }

        public void RemoveBoid(Boid boid)
        {
            boidList.Remove(boid);
        }

        /// <summary>
        /// Boidsの加速度を計算、設定
        /// </summary>
        public void ControlBoids()
        {
            foreach (Boid boid in boidList)
            {
                if (!boid.gameObject.activeSelf)
                {
                    continue;
                }

                Vector3 preAccel = boid.GetAcceleration();

                var powerToCenter = CalculateRestrictionSpherePower(boid.transform.position);

                var nearboids = GetNearBoids(boid);
                Vector3 newAccel = Vector3.zero;
                if (nearboids.Count > 0)
                {
                    var separatePower = CalculateSeparatePower(boid, nearboids);
                    var alignmentPower = CalculateAlignmentPower(nearboids);
                    var cohesionPower = CalculateCohesion(boid, nearboids);
                    var toTargetPower = CalculateToTargetPower(boid);

                    newAccel = preAccel + (powerToCenter + separatePower + alignmentPower + cohesionPower +
                                           toTargetPower) * Time.deltaTime;
                }
                else
                {
                    var toTargetPower = CalculateToTargetPower(boid);

                    newAccel = preAccel + (powerToCenter + toTargetPower) * Time.deltaTime;
                }

                boid.SetAcceleration(newAccel.normalized);
            }
        }

        private Vector3 CalculateRestrictionSpherePower(Vector3 boidPosition)
        {
            float radius = _boidParameter.restrictionSphereRadius;
            Vector3 boidsCenter = transform.position;

            Vector3 vectorFromboidToboidcenter = boidsCenter - boidPosition;
            float distance = vectorFromboidToboidcenter.magnitude;
            if (_boidParameter.restrictionStartDistance < distance)
            {
                float powerMulti = (distance - _boidParameter.restrictionStartDistance) /
                                   (_boidParameter.restrictionSphereRadius - _boidParameter.restrictionStartDistance);
                return powerMulti * _boidParameter.powerToBoidsCenter * vectorFromboidToboidcenter;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Boid> GetNearBoids(Boid targetBoid)
        {
            List<Boid> returnNearBoidList = new List<Boid>();
            var prodThresh = Mathf.Cos(70 * Mathf.Deg2Rad);

            foreach (var boid in this.boidList)
            {
                if (boid == targetBoid)
                {
                    continue;
                }


                var boidToOther = boid.transform.position - targetBoid.transform.position;
                var distance_BoidToOther = boidToOther.magnitude;

                if (distance_BoidToOther < _boidParameter.nearboidsDistance)
                {
                    var dot = Vector3.Dot(targetBoid.GetVelocity().normalized, boidToOther.normalized);
                    if (prodThresh < dot)
                    {
                        returnNearBoidList.Add(boid);
                    }
                }
            }

            return returnNearBoidList;
        }

        private Vector3 CalculateSeparatePower(Boid targetBoid, List<Boid> nearBoid)
        {
            Vector3 power = Vector3.zero;

            foreach (var boid in nearBoid)
            {
                power += (targetBoid.transform.position - boid.transform.position).normalized;
            }

            power /= nearBoid.Count;
            return power * _boidParameter.separetePower;
        }

        private Vector3 CalculateAlignmentPower(List<Boid> nearBoid)
        {
            Vector3 alignmentPower = Vector3.zero;
            foreach (var boid in nearBoid)
            {
                alignmentPower += boid.GetVelocity();
            }

            alignmentPower /= nearBoid.Count;
            return alignmentPower * _boidParameter.alignmentPower;
        }

        private Vector3 CalculateCohesion(Boid target, List<Boid> nearBoid)
        {
            var cohesionPower = Vector3.zero;

            var centerNearBoid = Vector3.zero;
            foreach (var boid in nearBoid)
            {
                centerNearBoid += boid.transform.position;
            }

            centerNearBoid /= nearBoid.Count;

            cohesionPower = (centerNearBoid - target.transform.position).normalized;
            return cohesionPower * _boidParameter.cohesionPower;
        }

        private Vector3 CalculateToTargetPower(Boid target)
        {
            var power = (this.target.position - target.transform.position).normalized;
            return power * _boidParameter.toTargetPower;
        }

        /// <summary>
        /// ギズモを表示
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _boidParameter.restrictionSphereRadius);
        }
    }
}