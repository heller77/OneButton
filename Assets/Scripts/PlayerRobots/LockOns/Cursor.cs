using UnityEngine;

namespace Character.LockOns
{
    /// <summary>
    /// ロボットに乗った時に
    /// </summary>
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private GameObject cursorGameObject;
        [SerializeField] private Animator _animator;
        private static readonly int Decide = Animator.StringToHash("decide");
        private static readonly int Selectmode = Animator.StringToHash("selectmode");

        /// <summary>
        /// カーソルを表示する
        /// </summary>
        public void Display()
        {
            cursorGameObject.SetActive(true);
        }

        /// <summary>
        /// カーソルを非表示にする
        /// </summary>
        public void Hide()
        {
            cursorGameObject.SetActive(false);
        }

        /// <summary>
        /// targetにカーソルを移動する
        /// </summary>
        /// <param name="target">カーソルを合わせる対象</param>
        public void MoveToTarget(Transform target)
        {
            this.cursorGameObject.transform.position = target.position;
        }

        /// <summary>
        /// カーソルを敵を選択する時の見た目にする
        /// </summary>
        public void ChangeVisualizeToSelectMode()
        {
            _animator.SetTrigger(Selectmode);
        }

        /// <summary>
        /// カーソルを敵を決定状態の見た目ににする
        /// </summary>
        public void ChangeVisualizeToDecisionMode()
        {
            _animator.SetTrigger(Decide);
        }

        /// <summary>
        /// カメラをずっと見てる
        /// </summary>
        private void BillBoard()
        {
            var cameraposition = UnityEngine.Camera.main.transform.position;
            cameraposition.y = transform.position.y;
            transform.LookAt(cameraposition);
        }

        private void Update()
        {
            BillBoard();
        }
    }
}