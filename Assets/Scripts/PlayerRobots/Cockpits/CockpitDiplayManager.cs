﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.CockpitButtons
{
    public class CockpitDiplayManager : MonoBehaviour
    {
        [SerializeField] private GameObject niceQuad;
        [SerializeField] private float niceColorMaxIntesity = 10;
        [SerializeField] private float niceColorMinIntesity = 0;

        private Material niceQuadMaterial;

        /// <summary>
        /// selecttargetとattackのfadetime
        /// </summary>
        [SerializeField] private float fadeStartTime = 0.1f;

        [SerializeField] private float fadeEndTime = 0.1f;

        [SerializeField] private GameObject selectTargetQuad;
        private Material selectTargetMaterial;

        [SerializeField] private GameObject attackQuad;
        private Material attackMaterial;

        [SerializeField] private float niceDisplayTime = 0.4f;

        private float fadeMinValue = 0.1f;

        private void Start()
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