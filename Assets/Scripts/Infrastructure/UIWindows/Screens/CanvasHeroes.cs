using Scripts.Army.PlayerSquad;
using System;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class CanvasHeroes : MonoBehaviour
    {
        public event Action Activate;
        public event Action Deactivate;

        private void OnEnable()
        {
            Activate?.Invoke();
        }

        private void OnDisable()
        {
            Deactivate?.Invoke();
        }
    }
}
