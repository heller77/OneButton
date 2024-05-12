using System;
using Character.Weapon.CannonBulletExplosions;
using Cysharp.Threading.Tasks;
using Enemys;
using GameManagers;
using GameManagers.AudioManagers;
using R3;
using UnityEngine;

namespace Character.Weapon
{
    /// <summary>
    ///     大砲
    /// </summary>
    public class Cannon : MonoBehaviour
    {
        /// <summary>
        ///     弾を発射する時のエフェクト（煙）
        /// </summary>
        [SerializeField] private ParticleSystem fireEffect;

        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletInstatiatePosition;

        [SerializeField] private GameObject ExplosionEffect;
        [SerializeField] private EnemyManager _enemyManager;

        [SerializeField] private AudioManager _audioManager;

        public async void Attack(IHitable target, float attackPower)
        {
            AudioManager.Instance.PlaySe(SeVariable.CanonSe, transform.position, 0.1f);

            fireEffect.Play();
            var bulletInstance = Instantiate(bullet, bulletInstatiatePosition.position, Quaternion.identity);

            float elapsedTime = 0;
            float duration = 1.0f;
            while (elapsedTime <= duration)
            {
                float t = elapsedTime / duration;
                var nowPosition = bulletInstance.transform.position;
                var diff = target.GetTransform().position - nowPosition;
                if (diff.magnitude < 1.1)
                {
                    Debug.Log("近すぎる");
                    break;
                }

                var newpos = nowPosition + t * diff;
                // Debug.Log(string.Format("diff {0} , new {1}", diff, newpos));
                bulletInstance.transform.position = newpos;

                await UniTask.DelayFrame(1);
                elapsedTime += Time.deltaTime;
            }

            target.Hitted(attackPower);
            GenerateExpolosion(target);

            //スコア追加
            BattleResultManager.GetInstance().AddConsumeBullet();
        }

        private void GenerateExpolosion(IHitable target)
        {
            var cannonExplosion = Instantiate(ExplosionEffect, target.GetTransform().position,
                    Quaternion.LookRotation(target.GetTransform().position - transform.position))
                .GetComponent<CannonBulletExplosion>();
            cannonExplosion.SetEnemyManager(_enemyManager);
            cannonExplosion.Explosion();
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(x => { cannonExplosion.Fade(); });
        }
    }
}