using UnityEngine;

namespace TitleScene.UIs
{
    /// <summary>
    ///     ライセンス表示を管理
    /// </summary>
    public class DisplayLiscense : MonoBehaviour
    {
        [SerializeField] private GameObject displayGameObjcet;

        public void Display()
        {
            displayGameObjcet.SetActive(true);
        }

        public void Hide()
        {
            displayGameObjcet.SetActive(false);
        }
    }
}