using UnityEngine;

namespace Character.LockOns
{
    /// <summary>
    /// 敵を選択するカーソル
    /// </summary>
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private GameObject cursorGameObject;
        [SerializeField] private Animator _animator;
        private static readonly int Decide = Animator.StringToHash("decide");
        private static readonly int Selectmode = Animator.StringToHash("selectmode");

        [SerializeField] private EnemyDisplayWindow enemyDisplayWindowGameObject;

        /// <summary>
        /// カーソルを表示する
        /// </summary>
        public void Display()
        {
            cursorGameObject.SetActive(true);
        }

        public void DisplayInfo(EnemyType enemyType)
        {
            enemyDisplayWindowGameObject.PopWindow(enemyType);
        }

        /// <summary>
        /// カーソルを非表示にする
        /// </summary>
        public void Hide()
        {
            cursorGameObject.SetActive(false);
        }

        public void HideInfo()
        {
            enemyDisplayWindowGameObject.CloseWindow();
        }

        public void Move(Vector3 position)
        {
            this.cursorGameObject.transform.position = position;
        }

        /// <summary>
        /// カーソルを敵を選択する時の見た目にする
        /// </summary>
        public void ChangeVisualizeToSelectMode()
        {
            _animator.SetTrigger(Selectmode);
            _animator.ResetTrigger(Decide);
        }

        /// <summary>
        /// カーソルを敵を決定状態の見た目ににする
        /// </summary>
        public void ChangeVisualizeToDecisionMode()
        {
            _animator.SetTrigger(Decide);
            _animator.ResetTrigger(Selectmode);
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