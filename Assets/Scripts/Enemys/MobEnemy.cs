using UnityEngine;

namespace Enemys
{
    public class MobEnemy : MonoBehaviour, ITarget
    {
        public Vector3 GetPosition()
        {
            return this.transform.position;
        }
    }
}