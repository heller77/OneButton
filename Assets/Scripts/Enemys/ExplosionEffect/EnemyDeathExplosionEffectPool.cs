using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Enemys.ExplosionEffect
{
    /// <summary>
    /// Enemyが倒れた時のエフェクトを生成する
    /// </summary>
    public class EnemyDeathExplosionEffectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyExplosion;
        [SerializeField] private int poolDefaultCapacity;
        [SerializeField] private int poolMaxSize;

        private static EnemyDeathExplosionEffectPool _instance;

        //シングルトンのインスタンスを返す。（nullの事は考えてないので注意）
        public static EnemyDeathExplosionEffectPool Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _instance = this;
        }

        private IObjectPool<GameObject> _objectPool;

        public void Start()
        {
            _objectPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_enemyExplosion), // プールが空のときに新しいインスタンスを生成する処理
                actionOnGet: target => target.gameObject.SetActive(true), // プールから取り出されたときの処理 
                actionOnRelease: target => target.gameObject.SetActive(false), // プールに戻したときの処理
                actionOnDestroy: target => Destroy(target), // プールがmaxSizeを超えたときの処理
                collectionCheck: true, // 同一インスタンスが登録されていないかチェックするかどうか
                defaultCapacity: poolDefaultCapacity, // デフォルトの容量
                maxSize: poolMaxSize);
        }

        /// <summary>
        /// エフェクトを取得(objectpoolで管理)
        /// </summary>
        public GameObject GetExplosionEffectPool()
        {
            return _objectPool.Get();
        }

        /// <summary>
        /// エフェクトを解放（objectpool）
        /// </summary>
        public void Release(GameObject releaseTarget)
        {
            _objectPool.Release(releaseTarget);
        }
    }
}