using System;
using Cysharp.Threading.Tasks;
using Enemys;
using GameManagers.SeManagers;
using UnityEngine;

namespace Character.Weapon
{
    public class NormalGun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletInstancePosition;

        [SerializeField] private AudioManager _audioManager;

        /// <summary>
        /// 
        /// </summary>
        public async void Attack(IHitable target, float attackPower)
        {
            _audioManager.PlaySe(SeVariable.normalbulletFireSE, this.transform.position, 0.05f);
            var bulletInstance = Instantiate(bulletPrefab, bulletInstancePosition.position, Quaternion.identity);

            float elapsedTime = 0;

            //弾を飛ばす
            float duration = 1.0f;
            while (elapsedTime <= duration)
            {
                //ターゲットが攻撃出来る状態でないならば、
                if (!target.isHitable())
                {
                    VanishBullet(bulletInstance);
                    return;
                }

                Vector3 targetPos = target.GetTransform().position;


                float t = elapsedTime / duration;
                var nowPosition = bulletInstance.transform.position;
                var diff = targetPos - nowPosition;
                if (diff.magnitude < 1.1)
                {
                    Debug.Log("近すぎる");
                    break;
                }

                var newpos = nowPosition + t * diff;
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