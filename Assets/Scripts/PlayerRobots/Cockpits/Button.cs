using System;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Character.CockpitButtons
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private Animator buttonAnimatpr;

        private Subject<Unit> pushAnimationSubject = new Subject<Unit>();
        private bool isPushAnimation = false;

        private void Start()
        {
            var trigger = buttonAnimatpr.GetBehaviour<ObservableStateMachineTrigger>();
            var a = trigger.OnStateEnterAsObservable().Where(x => x.StateInfo.IsName("push")).Subscribe(x =>
            {
                this.pushAnimationSubject.OnNext(Unit.Default);
            }).AddTo(this);
        }

        public void PowerOn()
        {
            buttonAnimatpr.SetBool("poweron", true);
        }

        public async UniTask Push()
        {
            if (isPushAnimation)
            {
                return;
            }

            buttonAnimatpr.SetBool("push", true);
            isPushAnimation = true;
            await pushAnimationSubject.FirstAsync();
            isPushAnimation = false;
            buttonAnimatpr.SetBool("push", false);
        }
    }
}