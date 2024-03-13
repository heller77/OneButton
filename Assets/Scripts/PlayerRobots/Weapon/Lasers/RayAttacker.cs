using Enemys;
using UnityEngine;

namespace Character.Weapon.Lasers
{
    public class RayAttacker
    {
        public static void RayAttack(Transform origin, Transform end, float damage)
        {
            var dir = end.position - origin.position;
            Ray ray = new Ray(origin.position, dir);
            var hits = Physics.RaycastAll(ray);
            foreach (var raycastHit in hits)
            {
                Debug.Log(raycastHit.collider.gameObject.name + " にレイがヒットした");
                if (raycastHit.collider.gameObject.TryGetComponent(out IHitable hit))
                {
                    hit.Hitted(damage);
                }
            }
        }
    }
}