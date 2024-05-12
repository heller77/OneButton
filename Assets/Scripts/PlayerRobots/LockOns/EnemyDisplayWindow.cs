using System.Collections.Generic;
using DG.Tweening;
using GameLoops;
using UnityEngine;

namespace Character.LockOns
{
    /// <summary>
    ///     敵を表示するウィンドーを管理
    ///     選択している敵の情報を表示
    /// </summary>
    public class EnemyDisplayWindow : MonoBehaviour, IInitializable
    {
        private static readonly int Enemytexture = Shader.PropertyToID("_enemytexture");
        [SerializeField] private float duration = 0.1f;

        [SerializeField] private Texture mob;
        [SerializeField] private Texture mother;
        [SerializeField] private Material _material;
        [SerializeField] private Dictionary<int, Texture> enemy_textureDic;
        private Vector3 originScale;

        public void Initialize()
        {
            originScale = transform.localScale;
            enemy_textureDic = new Dictionary<int, Texture>
            {
                { (int)EnemyType.mob, mob },

                { (int)EnemyType.mother, mother },
            };
        }

        /// <summary>
        ///     ウィンドーを表示
        ///     enemyTypeに応じて表示するテクスチャ
        /// </summary>
        /// <param name="enemyType"></param>
        public void PopWindow(EnemyType enemyType)
        {
            _material.SetTexture(Enemytexture, enemy_textureDic[(int)enemyType]);
            transform.DOScaleY(0, 0);

            transform.DOScaleY(originScale.y, duration);
        }

        /// <summary>
        ///     ウィンドーを非表示
        /// </summary>
        public void CloseWindow()
        {
            transform.DOScaleY(0, duration);
        }
    }
}