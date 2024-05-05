using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorials
{
    public class TutorialUiManager : MonoBehaviour
    {
        [SerializeField] private RectTransform loguiParent;
        [SerializeField] private GameObject loguiPrefab;

        [SerializeField] private Texture _textimageSpaceclickforselect;
        [SerializeField] private Texture _textimageSpaceclickforattack;

        private static readonly int DisplayValue = Shader.PropertyToID("_displayValue");

        private TutorialLogUI nowDisplayMissionUI;

        private void GenerateTutorialUI()
        {
            if (this.nowDisplayMissionUI != null)
            {
                // var pos = nowtarget.gameObject.GetComponent<RectTransform>().position;
                // pos.y -= 1;
                // nowtarget.gameObject.GetComponent<RectTransform>().position = pos;
                nowDisplayMissionUI.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150, 0.4f);
            }

            GameObject logui = Instantiate(loguiPrefab) as GameObject;
            logui.transform.SetParent(this.loguiParent, false);
            logui.SetActive(true);

            nowDisplayMissionUI = logui.GetComponent<TutorialLogUI>();
            nowDisplayMissionUI.Initialize();
        }

        /// <summary>
        /// 敵を選択するのにスペースキーをクリックするUIを表示
        /// </summary>
        public void DisplaySpaceKeyForSelectEnemy()
        {
            GenerateTutorialUI();
            nowDisplayMissionUI.CheckMark.material.SetFloat(DisplayValue, 0);
            nowDisplayMissionUI.TextImage.texture = this._textimageSpaceclickforselect;
            nowDisplayMissionUI.TextImage.material.SetFloat(DisplayValue, 1);
        }

        /// <summary>
        /// 敵を選択出来たら
        /// </summary>
        public void ExecuteFirstSpaceClickForSelectEnemy()
        {
            nowDisplayMissionUI.CheckMark.material.SetFloat(DisplayValue, 1);
            //ミッションクリアしたので、uiをgracePeriod秒後に非表示
            var target = nowDisplayMissionUI;
            float gracePeriod = 0.7f;
            Observable.Timer(TimeSpan.FromSeconds(gracePeriod)).Subscribe(_ =>
            {
                target.gameObject.GetComponent<RectTransform>().DOAnchorPosX(1000, 0.4f);
            });
        }

        /// <summary>
        /// 敵を攻撃するのにスペースキーをクリックするUIを表示
        /// </summary>
        public void DisplaySpaceKeyForAttackEnemy()
        {
            GenerateTutorialUI();
            nowDisplayMissionUI.CheckMark.material.SetFloat(DisplayValue, 0);
            nowDisplayMissionUI.TextImage.texture = this._textimageSpaceclickforattack;
            nowDisplayMissionUI.TextImage.material.SetFloat(DisplayValue, 1);
        }

        /// <summary>
        /// 敵を攻撃出来たら
        /// </summary>
        public void ExecuteSpaceClickForAttackEnemy()
        {
            nowDisplayMissionUI.CheckMark.material.SetFloat(DisplayValue, 1);
            //ミッションクリアしたので、uiをgracePeriod秒後に非表示
            var target = nowDisplayMissionUI;
            float gracePeriod = 2.0f;
            Observable.Timer(TimeSpan.FromSeconds(gracePeriod)).Subscribe(_ =>
            {
                target.gameObject.GetComponent<RectTransform>().DOAnchorPosX(1000, 0.4f);
            });
        }
    }
}