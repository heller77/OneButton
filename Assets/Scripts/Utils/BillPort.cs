using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     ビルポートさせたいオブジェクトにつける
    /// </summary>
    public class BillPort : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}