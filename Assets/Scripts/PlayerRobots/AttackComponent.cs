using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemys;
using UnityEngine;

namespace Character
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletFirePosition;


        public void Attack(IHitable target, float AttackPower)
        {
            Debug.Log("attack");
            target.Hitted(AttackPower);
        }


        /// <summary>
        /// 弾を撃つ
        /// </summary>
        public void FireBullet(Transform target)
        {
            var bullet = Instantiate(bulletPrefab, bulletFirePosition.position, Quaternion.identity);
            //memo : ここtargetが動くとあたっていないように見えるので、修正する
            bullet.transform.DOMove(target.position, 0.6f);
            MoveBulletToTarget(bullet.transform, target, 10.5f);
        }

        private async UniTask MoveBulletToTarget(Transform bullet, Transform targetTransform, float duration)
        {
            float time = 0;
            var firstPos = bullet.position;
            while (time <= duration)
            {
                //0-1の
                float t = time / duration;
                var diff = bullet.position - targetTransform.position;
                var newpos = bullet.position + t * diff;
                bullet.position = newpos;

                await UniTask.DelayFrame(1);
                time += Time.deltaTime;
            }
        }
    }
}