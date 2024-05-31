using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class CanvasRaid : MonoBehaviour
    {
        [SerializeField] private Button _attack;
        public Button Attack => _attack;

        public event Action Open;
        public event Action Close;

        private void OnEnable()
        {
            Open?.Invoke();
        }

        private void OnDisable() 
        {
            Close?.Invoke();
        }
    }
}
