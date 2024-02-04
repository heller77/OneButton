using Cysharp.Threading.Tasks;
using Enemys;
using Unity.Mathematics;
using UnityEngine;

namespace Character.Weapon
{
    public class Cannon : MonoBehaviour
    {
        /// <summary>
        /// 弾を発射する時のエフェクト（煙）
        /// </summary>
        [SerializeField] private ParticleSystem fireEffect;

        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletInstatiatePosition;

        [SerializeField] private GameObject ExplosionEffect;

        public async void Attack(IHitable target, float attackPower)
        {
            fireEffect.Play();
            var bulletInstance = Instantiate(bullet, bulletInstatiatePosition.position, Quaternion.identity);

            float elapsedTime = 0;
            float duration = 1.0f;
            while (elapsedTime <= duration)
            {
                Debug.Log(target.GetTransform().position);
                float t = elapsedTime / duration;
                var nowPosition = bulletInstance.transform.position;
                var diff = target.GetTransform().position - nowPosition;
                if (diff.magnitude < 1.1)
                {
                    Debug.Log("近すぎる");
                    break;
                }

                var newpos = nowPosition + t * diff;
                Debug.Log(string.Format("diff {0} , new {1}", diff, newpos));
                bulletInstance.transform.position = newpos;

                await UniTask.DelayFrame(1);
                elapsedTime += Time.deltaTime;
            }

            target.Hitted(attackPower);
            GenerateExpolosion(target);
        }

        private void GenerateExpolosion(IHitable target)
        {
            Instantiate(ExplosionEffect, target.GetTransform().position,
                Quaternion.LookRotation(target.GetTransform().position - this.transform.position));
        }
    }
}