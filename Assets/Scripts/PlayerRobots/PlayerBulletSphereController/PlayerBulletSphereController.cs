using System.Collections.Generic;
using UnityEngine;

namespace Character.PlayerBulletController
{
    /// <summary>
    ///     弾のチャージの率を表す球の管理
    /// </summary>
    public class PlayerBulletSphereController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> sphereList = new List<GameObject>();
        private int nowDisplaySphereNum;

        public void AddBall()
        {
            sphereList[nowDisplaySphereNum].SetActive(true);
            nowDisplaySphereNum++;
        }

        public void HideAllSphere()
        {
            foreach (var sphere in sphereList)
            {
                sphere.SetActive(false);
            }
        }
    }
}