using System;
using UnityEngine;

namespace Utils
{
    public class BillPort : MonoBehaviour
    {
        private void Update()
        {
            this.transform.LookAt(Camera.main.transform.position);
        }
    }
}