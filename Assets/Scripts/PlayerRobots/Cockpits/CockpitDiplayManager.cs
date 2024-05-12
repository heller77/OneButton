using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Character.CockpitButtons
{
    /// <summary>
    ///     コックピットにあるディスプレイの管理
    /// </summary>
    public class CockpitDiplayManager : MonoBehaviour
    {
        [SerializeField] private GameObject niceQuad;
        [SerializeField] private float niceColorMaxIntesity = 10;
        [SerializeField] private float niceColorMinIntesity;

        /// <summary>
        ///     selecttargetとattackのfadetime
        /// </summary>
        [SerializeField] private float fadeStartTime = 0.1f;

        [SerializeField] private float fadeEndTime = 0.1f;

        [SerializeField] private GameObject selectTargetQuad;

        [SerializeField] private GameObject attackQuad;

        [SerializeField] private float niceDisplayTime = 0.4f;

        private readonly float fadeMinValue = 0.1f;
        private Material attackMaterial;

        private Material niceQuadMaterial;
        private Material selectTargetMaterial;

        private void Awake()
        {
            niceQuadMaterial = niceQuad.GetComponent<MeshRenderer>().material;
            selectTargetMaterial = selectTargetQuad.GetComponent<Renderer>().material;
            attackMaterial = attackQuad.GetComponent<Renderer>().material;
        }

        public void ShowSelect()
        {
            attackMaterial.DOFade(fadeMinValue, fadeEndTime);
            selectTargetMaterial.DOFade(1.0f, fadeStartTime);
        }

        public void ShowAttack()
        {
            selectTargetMaterial.DOFade(fadeMinValue, fadeEndTime);
            attackMaterial.DOFade(1.0f, fadeStartTime);
        }

        public async UniTask DisplayNice()
        {
            niceQuadMaterial.SetFloat("_intensity", niceColorMaxIntesity);
            await UniTask.Delay(TimeSpan.FromSeconds(niceDisplayTime));
            niceQuadMaterial.SetFloat("_intensity", niceColorMinIntesity);
        }

        public void AllHide()
        {
            selectTargetMaterial.DOFade(fadeMinValue, 0);
            attackMaterial.DOFade(fadeMinValue, 0);
            niceQuadMaterial.SetFloat("_intensity", niceColorMinIntesity);
        }
    }
}