using System;
using System.Collections.Generic;
using System.Linq;
using Enemys.Boids;
using GameLoops;
using R3;
using UnityEngine;

namespace Enemys
{
    public class EnemyManager : MonoBehaviour, ITickable
    {
        [SerializeField] private List<MobEnemy> _mobEnemies = new List<MobEnemy>();
        [SerializeField] private BossEnemy _bossEnemy;

        private Subject<Unit> _enemyDestroy = new Subject<Unit>();
        [SerializeField] private List<BoidsManager> _boidsManager = new List<BoidsManager>();

        public void StartMoveEnemys()
        {
            foreach (var manager in _boidsManager)
            {
                manager.Initialize();
            }
        }

        public void Tick()
        {
            MoveBoid();
        }

        public void MoveBoid()
        {
            foreach (var manager in _boidsManager)
            {
                manager.Tick();
            }
        }

        public void AddBoidManager(BoidsManager boidsManager)
        {
            _boidsManager.Add(boidsManager);
        }

        public Observable<Unit> enemyDestroy
        {
            get { return this._enemyDestroy; }
        }

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
            _enemyDestroy.OnNext(Unit.Default);
            return _mobEnemies.Remove(mobEnemy);
        }

        public void RemoveBoss()
        {
            this._bossEnemy = null;
        }

        public void RemoveAllEnemy()
        {
            // foreach (var mobEnemy in _mobEnemies)
            // {
            //     mobEnemy.DontDisplay();
            // }

            _mobEnemies.Clear();
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
                if (distance_enemy.Key < searchDistance)
                {
                    returnMobEnemies.Add(distance_enemy.Value);
                }
            }

            return returnMobEnemies;
        }

        /// <summary>
        /// カメラにtargetが映っているかを判定
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool JudgeTargetisInCamera(Camera camera, IHitable target)
        {
            // オブジェクトのワールド座標をビューポート座標に変換
            Vector3 viewportPoint = camera.WorldToViewportPoint(target.GetTransform().position);

            //カメラの縦方向の下限
            float camerayMin = 0.3f;
            //カメラに写っているかどうか（camerayMinでカメラの下の方はコックピットなので制限）
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= camerayMin && viewportPoint.y <= 1 && viewportPoint.z >= 0)
            {
                return true;
            }

            return false;
        }

        public List<IHitable> SearchEnemyInCamera(Transform origin, float searchDistance, Camera camera)
        {
            //searchDistance以内に居る敵のリスト
            List<IHitable> returnMobEnemies = new List<IHitable>();

            var enemyanddistanceDict = CaluculateEnemysDistance(origin);
            foreach (var distance_enemy in enemyanddistanceDict)
            {
                if (distance_enemy.Key < searchDistance)
                {
                    if (JudgeTargetisInCamera(camera, distance_enemy.Value))
                    {
                        returnMobEnemies.Add(distance_enemy.Value);
                    }
                }
            }

            return returnMobEnemies;
        }

        /// <summary>
        /// 有効な敵との距離を計算し、辞書を返す
        /// </summary>
        /// <returns></returns>
        private SortedDictionary<float, IHitable> CaluculateEnemysDistance(Transform origin)
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

        public IHitable GetBossEnemy()
        {
            return this._bossEnemy;
        }
    }
}