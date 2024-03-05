using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Character.LockOns
{
    public class EnemyDisplayWindow : MonoBehaviour
    {
        private Vector3 originScale;
        [SerializeField] private float duration = 0.1f;

        [SerializeField] private Texture mob;
        [SerializeField] private Texture mother;
        [SerializeField] private Material _material;
        [SerializeField] private Dictionary<int, Texture> enemy_textureDic;
        private static readonly int Enemytexture = Shader.PropertyToID("_enemytexture");

        private void Start()
        {
            originScale = this.transform.localScale;
            enemy_textureDic = new Dictionary<int, Texture>()
            {
                { (int)EnemyType.mob, mob },

                { (int)EnemyType.mother, mother },
            };
        }

        public void PopWindow(EnemyType enemyType)
        {
            _material.SetTexture(Enemytexture, this.enemy_textureDic[(int)enemyType]);
            transform.DOScaleY(0, 0);

            transform.DOScaleY(originScale.y, duration);
        }

        public void CloseWindow()
        {
            transform.DOScaleY(0, duration);
        }
    }
}