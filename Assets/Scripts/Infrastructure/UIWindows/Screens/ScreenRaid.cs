using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenRaid : Screen
    {
        protected override void OnEnable()
        {
            _bookmark.ButtonOnClic += OpenScreen;
        }

        protected override void OnDisable()
        {
            _bookmark.ButtonOnClic -= OpenScreen;
        }
    }
}
