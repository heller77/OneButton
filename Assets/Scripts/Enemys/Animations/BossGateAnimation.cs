using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Enemys.Animations
{
    /// <summary>
    /// ボスのゲート
    /// </summary>
    public class BossGateAnimation : MonoBehaviour
    {
        [SerializeField] private Transform GateTransform;
        [SerializeField] private float gateOpenAnimTime = 1.0f;
        [SerializeField] private Material gateMaterial;
        [SerializeField] private Transform gateTransform;

        [SerializeField] private float gatematerial_normalMove = 0.3f;
        [SerializeField] private float gate_break_time = 0.3f;

        [SerializeField] private GameObject boss;

        private Vector3 gatesize;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            gatesize = GateTransform.localScale;
            this.GateTransform.localScale = Vector3.zero;
            boss.SetActive(false);

            //マテリアルのパラメータ初期化
            this.gateMaterial.SetFloat("_NormalMove", 0);
            gateMaterial.SetVector("_gatePos", this.GateTransform.position);
            gateMaterial.SetFloat("_rotation", 0.0f);
        }

        public void GateOpen()
        {
            GateOpenAsync();
        }

        public async UniTask GateOpenAsync()
        {
            await GateTransform.DOScale(gatesize, gateOpenAnimTime);
            boss.SetActive(true);

            await this.gateMaterial.DOFloat(gatematerial_normalMove, "_NormalMove", gate_break_time);
            gateMaterial.SetFloat("_rotation", 100.0f);
        }
    }
}