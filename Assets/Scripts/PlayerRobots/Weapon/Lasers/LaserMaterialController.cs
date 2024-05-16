using UnityEngine;

namespace Character.Weapon.Lasers
{
    /// <summary>
    ///     レーザの見た目を管理
    /// </summary>
    public class LaserMaterialController
    {
        private readonly Material laserMaterial;

        public LaserMaterialController(Material laserMaterial)
        {
            this.laserMaterial = laserMaterial;
        }

        /// <summary>
        ///     レーザの太さを変える
        /// </summary>
        public void SetLaserExpandValue(float expandValue)
        {
            laserMaterial.SetFloat("_LaserExpandValue", expandValue);
        }
    }
}