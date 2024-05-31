using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class BookmarkButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioSource _audioSource;

        public Button Button => _button;

        public event Action ButtonOnClic;
        public event Action ButtonOffClic;

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
            if(_audioSource != null && _audioSource.enabled == true)
            {
                _audioSource.Play();
                Invoke("Click", 0f);
            }
            else
            {
                ButtonOnClic?.Invoke();
            }
        }

        public void Click()
        {
            ButtonOnClic?.Invoke();
        }

        public void OffClic()
        {
            ButtonOffClic?.Invoke();
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }
    }
}