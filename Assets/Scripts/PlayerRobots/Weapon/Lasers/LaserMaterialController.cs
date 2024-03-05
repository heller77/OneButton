using System;
using UnityEngine;

namespace Character.Weapon.Lasers
{
    public class LaserMaterialController
    {
        private Material laserMaterial;

        public LaserMaterialController(Material laserMaterial)
        {
            this.laserMaterial = laserMaterial;
        }

        /// <summary>
        /// レーザの太さを変える
        /// </summary>
        public void SetLaserExpandValue(float expandValue)
        {
            Debug.Log($"lasermaterial _LaserExpandValue  set {expandValue}");
            laserMaterial.SetFloat("_LaserExpandValue", expandValue);
        }
    }
}