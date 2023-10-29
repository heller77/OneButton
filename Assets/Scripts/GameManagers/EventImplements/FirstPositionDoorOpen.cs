using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameManagers.EventImplements
{
    public class FirstPositionDoorOpen : MonoBehaviour
    {
        public async UniTask OpenDoor()
        {
            Debug.Log("open");
        }
    }
}