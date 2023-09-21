using System;
using UnityEngine;

namespace Character
{
    public class ButtonAnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetBool("push", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _animator.SetBool("push", false);
            }
        }
    }
}