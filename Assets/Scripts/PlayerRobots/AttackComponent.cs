using Enemys;
using UnityEngine;

namespace Character
{
    public class AttackComponent : MonoBehaviour
    {
        public void Attack(IHitable target, float AttackPower)
        {
            Debug.Log("attack");
            target.Hitted(AttackPower);
        }
    }
}