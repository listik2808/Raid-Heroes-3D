using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenNoPay : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            _closeButton.ButtonOnClic += Close;
            //_canvasGroup.alpha = 0;
        }

        private void OnDisable()
        {
            _closeButton.ButtonOnClic -= Close;
        }

        private void Close()
        {
            //_canvasGroup.alpha = 1;
            gameObject.SetActive(false);
        }
    }
}
