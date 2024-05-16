using UnityEngine;

namespace Character
{
    /// <summary>
    ///     コックピットのボタンのアニメーションを管理
    /// </summary>
    public class ButtonAnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private MeshRenderer _meshRenderer;

        [SerializeField] private Material nonPushedMaterial;

        [SerializeField] private Material pushedMaterial;
        [SerializeField] private Light buttonLight;

        private void Update()
        {
            //todo : touch操作に対応できてないので、inputsystemを使う様に
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("push", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("push", false);
            }
        }

        public void ChangeMaterial_WhenPushing()
        {
            _meshRenderer.material = pushedMaterial;
        }

        public void ChangeMaterial_WhenNoPushing()
        {
            _meshRenderer.material = nonPushedMaterial;
        }
    }
}