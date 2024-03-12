using UnityEngine;

namespace TitleScene.UIs
{
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