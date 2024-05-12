using UnityEngine;
using UnityEngine.UI;

namespace Tutorials
{
    /// <summary>
    /// インゲームで操作などを説明するチュートリアルUI
    /// </summary>
    public class TutorialLogUI : MonoBehaviour
    {
        /// <summary>
        /// チェックマーク（Image）
        /// </summary>
        [SerializeField] private RawImage _chckMark;

        public RawImage CheckMark
        {
            get => _chckMark;
        }

        /// <summary>
        /// 操作を説明するテキスト画像
        /// </summary>
        [SerializeField] private RawImage _textImage;

        public RawImage TextImage
        {
            get => _textImage;
        }

        public RectTransform TutorialUIParent
        {
            get => _tutorialUIParent;
        }

        /// <summary>
        /// チェックマークとかの親
        /// </summary>
        [SerializeField] private RectTransform _tutorialUIParent;

        public void Initialize()
        {
            var textimagemat = TextImage.material;
            TextImage.material = new Material(textimagemat);

            var checkmat = CheckMark.material;
            CheckMark.material = new Material(checkmat);
        }
    }
}