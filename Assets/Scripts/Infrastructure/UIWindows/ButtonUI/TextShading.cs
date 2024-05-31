using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class TextShading : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _button;
        [SerializeField] private TMP_Text _text;
        private Color _currentColor;

        private void OnEnable()
        {
            _button.ButtonOnClic += SetTransparency;
            _button.ButtonOffClic += ResetTransparency;
        }

        private void OnDisable()
        {
            _button.ButtonOnClic -= SetTransparency;
            _button.ButtonOffClic -= ResetTransparency;
        }

        private void Start()
        {
            if (_button.Button.interactable)
            {
                ResetTransparency();
            }
            else
            {
                SetTransparency();
            }
        }

        public void SetTransparency()
        {
            _currentColor = new Color32(255, 255, 204, 255);
            _text.color = _currentColor;
        }

        private void ResetTransparency()
        {
            _currentColor = new Color32(153, 102, 51, 128);
            _text.color = _currentColor;
        }
    }
}