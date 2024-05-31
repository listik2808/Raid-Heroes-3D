using Scripts.Infrastructure.UIWindows.Screens;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class RedirectButtonShop : MonoBehaviour
    {
        [SerializeField] private ButtonCloseScreen _buttonCloseScreen;
        [SerializeField] private BookmarkButton _bookmarksButtonCoins;
        [SerializeField] private BookmarkButton _bookmarksButtonDiamond;
        [SerializeField] private BookmarkButton _bookmarksButtonStars;
        [SerializeField] private ScreenShop _screenShop;
        [SerializeField] private BookmarkButton _bookmarksCoins;
        [SerializeField] private BookmarkButton _bookmarksDiamond;
        [SerializeField] private BookmarkButton _bookmarksStars;

        public void Subscription()
        {
            _bookmarksButtonCoins.ButtonOnClic += OpenScreenShopCoins;
            _bookmarksButtonDiamond.ButtonOnClic += OpenScreenShopDiamond;
            _bookmarksButtonStars.ButtonOnClic += OpenScreenShopStars;
            _bookmarksCoins.ButtonOnClic += OpenScreenShopCoins;
            _bookmarksDiamond.ButtonOnClic += OpenScreenShopDiamond;
            _bookmarksStars.ButtonOnClic += OpenScreenShopStars;
        }

        public void Unsubscribe()
        {
            _bookmarksButtonCoins.ButtonOnClic -= OpenScreenShopCoins;
            _bookmarksButtonDiamond.ButtonOnClic -= OpenScreenShopDiamond;
            _bookmarksButtonStars.ButtonOnClic -= OpenScreenShopStars;
            _bookmarksCoins.ButtonOnClic -= OpenScreenShopCoins;
            _bookmarksDiamond.ButtonOnClic -= OpenScreenShopDiamond;
            _bookmarksStars.ButtonOnClic -= OpenScreenShopStars;
        }

        private void OpenScreenShopCoins()
        {
            _buttonCloseScreen.CloseScreen();
            _screenShop.WalletGold.Click();
        }

        private void OpenScreenShopDiamond()
        {
            _buttonCloseScreen.CloseScreen();
            _screenShop.WalletCrystal.Click();
        }

        private void OpenScreenShopStars()
        {
            _buttonCloseScreen.CloseScreen();
            _screenShop.WalletStars.Click();
        }
    }
}