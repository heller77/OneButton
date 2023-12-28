using UnityEngine;
using UnityEngine.Serialization;

namespace Enemys.Boids
{
    [CreateAssetMenu(menuName = "Boid/Param")]
    public class BoidParameter : ScriptableObject
    {
        public int boidsCount;

        public float minSpeed;
        public float maxSpeed;

        public float nearboidsDistance;

        /// <summary>
        /// 行動を制限する球の半径
        /// </summary>
        public float restrictionSphereRadius;

        public float restrictionStartDistance;

        /// <summary>
        /// Boidsの中心へと向かう力（遠いほど強くなるように計算される）
        /// restrictionStartDistanceより
        /// </summary>
        public float powerToBoidsCenter;

        public float separetePower;
        public float alignmentPower;
        public float cohesionPower;

        public float toTargetPower;
    }
}