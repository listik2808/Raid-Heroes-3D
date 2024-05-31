using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenTroops : Screen
    {
        [SerializeField] private BookmarkButton _closeButton;

        protected override void OnEnable()
        {
            _bookmark.ButtonOnClic += OpenScreen;
            _closeButton.ButtonOnClic += Close;
        }

        protected override void OnDisable()
        {
            _bookmark.ButtonOnClic -= OpenScreen;
            _closeButton.ButtonOnClic += Close;
        }
    }
}
