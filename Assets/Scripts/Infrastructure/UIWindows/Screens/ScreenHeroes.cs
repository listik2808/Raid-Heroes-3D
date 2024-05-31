using Scripts.Army.PlayerSquad;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenHeroes : Screen
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
