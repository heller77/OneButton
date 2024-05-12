using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// ビルポートさせたいオブジェクトにつける
    /// </summary>
    public class BillPort : MonoBehaviour
    {
        private void Update()
        {
            this.transform.LookAt(Camera.main.transform.position);
        }
    }
}