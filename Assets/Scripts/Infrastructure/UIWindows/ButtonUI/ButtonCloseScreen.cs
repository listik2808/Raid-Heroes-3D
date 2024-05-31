using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class ButtonCloseScreen : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private BookmarkButton _bookmarkButton;

        private void OnEnable()
        {
            _button.onClick.AddListener(CloseScreen);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(CloseScreen);
        }

        public void SetButton(BookmarkButton bookmarkButton)
        {
            _bookmarkButton = bookmarkButton;
        }

        public void CloseScreen()
        {
            _bookmarkButton.Click();
        }
    }
}