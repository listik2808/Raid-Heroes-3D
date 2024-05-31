
namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenArena : Screen
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
