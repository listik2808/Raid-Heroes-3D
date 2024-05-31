using Scripts.Infrastructure.UIWindows.ButtonUI;
using System;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected BookmarkButton _bookmark;

        public event Action<Screen> ChangeScreen;
        public BookmarkButton Bookmark => _bookmark;

        //public event Action ResetHero;
        protected abstract void OnEnable();

        protected abstract void OnDisable();

        public void Close()
        {
            //ResetHero?.Invoke();
            _bookmark.OffClic();
            _bookmark.Button.interactable = true;
            _canvas.gameObject.SetActive(false);
        }

        public void Open()
        {
            _canvas.gameObject.SetActive(true);
            _bookmark.Button.interactable = false;
        }

        public void OpenScreen()
        {
            ChangeScreen?.Invoke(this);
        }
    }
}
