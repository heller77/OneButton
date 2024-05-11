using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForDebug
{
    /// <summary>
    /// デバッグ用の機能
    /// テキストを特定オブジェクトの上に表示
    /// </summary>
    public class DisplayTextInEditor : MonoBehaviour
    {
        [SerializeField] protected string displayText = "";
        [SerializeField] private Color textColor = Color.gray;
        [SerializeField] private Vector3 testPositionOffset = Vector3.zero;

#if UNITY_EDITOR

        protected virtual string DecideDisplayText()
        {
            return this.displayText;
        }

        private void OnDrawGizmos()
        {
            //テキストの設定
            var guiStyle = new GUIStyle
                { fontSize = 20, normal = { textColor = textColor }, alignment = TextAnchor.UpperCenter };

            //名前をシーンビュー上に表示
            Handles.Label(transform.position + testPositionOffset, this.DecideDisplayText(), guiStyle);
        }

#endif
    }
}