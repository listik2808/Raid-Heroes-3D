using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class OpenPawer : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _bookmarkButton;
        [SerializeField] private BookmarkButton _close;
        [SerializeField] private GameObject _screenPower;

        private void OnEnable()
        {
            _bookmarkButton.Button.onClick.AddListener(Open);
            _close.Button.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _bookmarkButton.Button.onClick.RemoveListener(Open);
            _close.Button.onClick.RemoveListener(Close);
        }

        private void Open()
        {
            _screenPower.SetActive(true);
        }

        private void Close()
        {
            _screenPower.SetActive(false);
        }
    }
}
