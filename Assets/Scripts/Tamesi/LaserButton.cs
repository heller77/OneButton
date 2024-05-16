using Character.Weapon.Lasers;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Tamesi
{
    public class LaserButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Laser _laser;
        [SerializeField] private Transform target;

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => { _laser.LaunchLaser(target, 1); });
        }
    }
}