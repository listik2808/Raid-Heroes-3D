using Scripts.Logic.CastleConstruction;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class ButtonOpenBulletinBoard : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ConstructionCastle _construction;
        [SerializeField] private AudioSource _audioSource;

        public event Action<ConstructionCastle> ClicButtoncastle;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClicButton);
        }
        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClicButton);
        }

        public void ClicButton()
        {
            ClicButtoncastle?.Invoke(_construction);
            _audioSource.Play();
        }
    }
}