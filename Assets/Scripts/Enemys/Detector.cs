using System;
using UnityEngine;

namespace Enemys
{
    public class Detector : MonoBehaviour
    {
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