using Scripts.Infrastructure.UIWindows.ButtonUI;
using System;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public abstract class Tab : MonoBehaviour
    {
        [SerializeField] protected BookmarkButton _bookmarkButton;
        [SerializeField] protected BookmarkButton _exitButtonScreen;
        [SerializeField] protected Canvas _canvas;

        public event Action SetCurrencyValue;
        protected void OpenScreen()
        {
            SetCurrencyValue?.Invoke();
            _canvas.gameObject.SetActive(true);
        }

        protected void CloseScreen()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
