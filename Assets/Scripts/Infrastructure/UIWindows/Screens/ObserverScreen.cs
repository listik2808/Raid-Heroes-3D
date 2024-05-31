using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ObserverScreen : MonoBehaviour
    {
        [SerializeField] private List<Screen> _screenList = new List<Screen>();
        [SerializeField] private Button _button;
        [SerializeField] private AudioSource _audioSource;

        private Screen _screenActive;

        private void OnEnable()
        {
            foreach (Screen screen in _screenList)
            {
                screen.ChangeScreen += SetActiveScreen;
            }
        }

        private void OnDisable()
        {
            foreach (Screen screen in _screenList)
            {
                screen.ChangeScreen -= SetActiveScreen;
            }
        }

        private void Awake()
        {
            SetActiveScreen(_screenList[3]);
            _button.interactable = false;
        }

        private void SetActiveScreen(Screen screen)
        {
            _screenActive?.Close();
            _screenActive = screen;
            _screenActive.Open();
            _audioSource.Play();
        }
    }
}
