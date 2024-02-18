using System;
using Cysharp.Threading.Tasks;
using Enemys;
using UnityEngine;

namespace Character.Weapon
{
    public class NormalGun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletInstancePosition;

        /// <summary>
        /// 
        /// </summary>
        public async void Attack(IHitable target, float attackPower)
        {
            var bulletInstance = Instantiate(bulletPrefab, bulletInstancePosition.position, Quaternion.identity);

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
            VanishBullet(bulletInstance);
        }

        private void VanishBullet(GameObject bullet)
        {
            bullet.SetActive(false);
        }
    }
}