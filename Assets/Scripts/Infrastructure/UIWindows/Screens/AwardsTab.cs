namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class AwardsTab : Tab
    {
        private void OnEnable()
        {
            _bookmarkButton.ButtonOnClic += OpenScreen;
            _exitButtonScreen.ButtonOnClic += CloseScreen;
        }

        private void OnDisable()
        {
            _bookmarkButton.ButtonOnClic -= OpenScreen;
            _exitButtonScreen.ButtonOnClic -= CloseScreen;
        }
    }
}
