using UnityEngine;

namespace Enemys
{
    /// <summary>
    ///     Itrapableを起動する
    /// </summary>
    public class Detector : MonoBehaviour
    {
        /// <summary>
        ///     任意のタイミングで起動したいオブジェクト
        /// </summary>
        [SerializeField] private GameObject trapObject;

        private ITrapable trap;

        private void Start()
        {
            trap = trapObject.GetComponent<ITrapable>();
        }

        public void Detect()
        {
            trap.Boot();
        }
    }
}