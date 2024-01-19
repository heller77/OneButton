using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemys
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<MobEnemy> _mobEnemies = new List<MobEnemy>();
        [SerializeField] private BossEnemy _bossEnemy;


        public void Add(MobEnemy mobEnemy)
        {
            _mobEnemies.Add(mobEnemy);
        }

        /// <summary>
        /// 敵のリストから引数で指定した敵を削除
        /// </summary>
        /// <param name="mobEnemy"></param>
        /// <returns></returns>
        public bool RemoveEnemy(MobEnemy mobEnemy)
        {
            return _mobEnemies.Remove(mobEnemy);
        }

        public void RemoveBoss()
        {
            this._bossEnemy = null;
        }

        /// <summary>
        /// カメラの向きにある
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="cameraDir"></param>
        /// <returns></returns>
        public IHitable GetMostNearEnemyInCameraDirection(Transform origin, Vector3 cameraDir)
        {
            var distanceDict = CaluculateEnemysDistance(origin);
            if (distanceDict.Count != 0)
            {
                foreach (var distance_enemy in distanceDict)
                {
                    //カメラの向いてる向きとカメラから敵の位置のベクトルの内積を計算
                    var dot = Vector3.Dot(Vector3.Normalize(cameraDir),
                        Vector3.Normalize(distance_enemy.Value.GetTransform().position - origin.position));
                    // pi/2　より小さければカメラ内にあるとする
                    if (dot > MathF.Cos(MathF.PI / 2))
                    {
                        return distance_enemy.Value;
                    }
                }

                //todo 意味が分からなかったのでコメントアウト。何もターゲットがいなければnullにすべきでは
                // return distanceDict.FirstOrDefault().Value;
                return null;
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
        public List<IHitable> SearchEnemy(Transform origin, float searchDistance)
        {
            //searchDistance以内に居る敵のリスト
            List<IHitable> returnMobEnemies = new List<IHitable>();

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
        /// 有効な敵との距離を計算し、辞書を返す
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<float, IHitable> CaluculateEnemysDistance(Transform origin)
        {
            SortedDictionary<float, IHitable>　enemyanddistanceDict = new SortedDictionary<float, IHitable>();
            Vector3 originPosition = origin.position;

            //originとの距離がsearchDistance以内の敵を収集
            foreach (var mobEnemy in _mobEnemies)
            {
                float distance = Vector3.Distance(originPosition, mobEnemy.transform.position);
                enemyanddistanceDict.Add(distance, mobEnemy);
            }

            if (_bossEnemy != null)
            {
                float bossdistance = Vector3.Distance(originPosition, _bossEnemy.transform.position);
                if (bossdistance < _bossEnemy.GetBossAttackableRadius())
                {
                    enemyanddistanceDict.Add(bossdistance, _bossEnemy);
                }
            }

            return enemyanddistanceDict;
        }
    }
}