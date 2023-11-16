using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemys
{
    public class EnemyManager : MonoBehaviour
    {
        private List<MobEnemy> _mobEnemies = new List<MobEnemy>();

        public void Add(MobEnemy mobEnemy)
        {
            _mobEnemies.Add(mobEnemy);
        }

        public MobEnemy GetMostNearEnemy(Transform origin)
        /// <summary>
        /// カメラの向きにある
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="cameraDir"></param>
        /// <returns></returns>
        public MobEnemy GetMostNearEnemyInCameraDirection(Transform origin, Vector3 cameraDir)
        {
            var distanceDict = CaluculateEnemysDistance(origin);
            if (distanceDict.Count != 0)
            {
                foreach (var distance_enemy in distanceDict)
                {
                    //カメラの向いてる向きとカメラから敵の位置のベクトルの内積を計算
                    var dot = Vector3.Dot(Vector3.Normalize(cameraDir),
                        Vector3.Normalize(distance_enemy.Value.transform.position - origin.position));
                    // pi/2　より小さければカメラ内にあるとする
                    if (dot > MathF.Cos(MathF.PI / 2))
                    {
                        return distance_enemy.Value;
                    }
                }

                return distanceDict.FirstOrDefault().Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 敵を探す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="searchDistance"></param>
        /// <returns></returns>
        public List<MobEnemy> SearchEnemy(Transform origin, float searchDistance)
        {
            //searchDistance以内に居る敵のリスト
            List<MobEnemy> returnMobEnemies = new List<MobEnemy>();

            var enemyanddistanceDict = CaluculateEnemysDistance(origin);
            foreach (var distance_enemy in enemyanddistanceDict)
            {
                if (distance_enemy.Key > searchDistance)
                {
                    returnMobEnemies.Add(distance_enemy.Value);
                }
            }

            return returnMobEnemies;
        }

        /// <summary>
        /// 敵との距離を計算し、
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<float, MobEnemy> CaluculateEnemysDistance(Transform origin)
        {
            SortedDictionary<float, MobEnemy>　enemyanddistanceDict = new SortedDictionary<float, MobEnemy>();
            Vector3 originPosition = origin.position;

            //originとの距離がsearchDistance以内の敵を収集
            foreach (var mobEnemy in _mobEnemies)
            {
                float distance = Vector3.Distance(originPosition, mobEnemy.transform.position);
                enemyanddistanceDict.Add(distance, mobEnemy);
            }

            return enemyanddistanceDict;
        }
    }
}