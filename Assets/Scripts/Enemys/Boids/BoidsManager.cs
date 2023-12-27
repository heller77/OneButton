using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemys.Boids
{
    /// <summary>
    ///     Boidsクラスを管理
    /// </summary>
    public class BoidsManager : MonoBehaviour
    {
        private readonly List<Boid> boidList = new List<Boid>();
        [SerializeField] private GameObject boidPrefab;
        [SerializeField] private BoidParameter _boidParameter;

        private void Start()
        {
            for (int i = 0; i < _boidParameter.boidsCount; i++)
            {
                AddBoid();
            }
        }

        private void Update()
        {
            ControlBoids();
        }

        /// <summary>
        /// Boidを生成
        /// </summary>
        public void AddBoid()
        {
            GameObject boid = Instantiate(boidPrefab, 10 * Random.onUnitSphere, Random.rotation);
            var boidcomponent = boid.GetComponent<Boid>();
            boidList.Add(boidcomponent);
            boidcomponent.SetBoidParameter(this._boidParameter);
            //初期加速度設定
            Vector3 initAccel = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            boidcomponent.SetAcceleration(boid.transform.forward);
        }

        /// <summary>
        /// Boidsの加速度を計算、設定
        /// </summary>
        public void ControlBoids()
        {
            foreach (Boid boid in boidList)
            {
                Vector3 preAccel = boid.GetAcceleration();

                var powerToCenter = CalculateRestrictionSpherePower(boid.transform.position);

                var nearboids = GetNearBoids(boid);
                if (nearboids.Count > 0)
                {
                    var separatePower = CalculateSeparatePower(boid, nearboids);
                    var alignmentPower = CalculateAlignmentPower(nearboids);
                    var cohesionPower = CalculateCohesion(boid, nearboids);


                    Vector3 newAccel = preAccel + powerToCenter + separatePower + alignmentPower + cohesionPower;
                    boid.SetAcceleration(newAccel);
                }
                else
                {
                    Vector3 newAccel = preAccel + powerToCenter;
                    boid.SetAcceleration(newAccel);
                }
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

            cohesionPower = (centerNearBoid - target.transform.position);
            return cohesionPower * _boidParameter.cohesionPower;
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