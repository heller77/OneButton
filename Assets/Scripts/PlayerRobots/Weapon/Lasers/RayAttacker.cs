using System;
using Enemys;
using UnityEngine;

namespace Character.Weapon.Lasers
{
    public class RayAttacker
    {
        private static float radius = 10.0f;

        public static void RayAttack(Transform origin, Transform end, float damage)
        {
            var dir = end.position - origin.position;
            Ray ray = new Ray(origin.position, dir);
            Debug.DrawRay(ray.origin, ray.direction * dir.magnitude, Color.yellow, 10.0f);
            var hits = Physics.RaycastAll(ray);
            foreach (var raycastHit in hits)
            {
                if (raycastHit.collider.gameObject.TryGetComponent(out IHitable hit))
                {
                    hit.Hitted(damage);
                }
            }

            // 円周上の4つのサンプリング点から攻撃を行う
            for (int i = 0; i < 4; i++)
            {
                // 角度
                float angle = Mathf.Deg2Rad * (90 * i);
                // 発射点
                Vector3 samplePoint =
                    origin.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                // サンプリング点からの方向を計算
                Vector3 auxiliary_dir = (end.position - samplePoint);
                // 新しいレイを作成
                Ray auxiliary_ray = new Ray(samplePoint, dir);
                // レイキャストを実行し、ヒットしたすべてのオブジェクトを取得
                var auxiliary_hits = Physics.RaycastAll(ray);
                Debug.DrawRay(auxiliary_ray.origin, auxiliary_ray.direction * auxiliary_dir.magnitude, Color.red,
                    10.0f);

                foreach (var hit in auxiliary_hits)
                {
                    // ヒットしたオブジェクトに対して攻撃を実行
                    if (hit.collider.gameObject.TryGetComponent(out IHitable hittable))
                    {
                        hittable.Hitted(damage);
                    }
                }
            }
        }
    }
}