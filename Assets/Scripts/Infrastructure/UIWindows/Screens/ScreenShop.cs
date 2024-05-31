using Scripts.Infrastructure.UIWindows.ButtonUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenShop : Screen
    {
        //[SerializeField] private AudioSource _audioSource;
        [SerializeField] private BookmarkButton _buttonSet;
        [SerializeField] private BookmarkButton _buttonCoin;
        [SerializeField] private BookmarkButton _buttonCrystals;
        [SerializeField] private BookmarkButton _buttonStars;
        [SerializeField] private BookmarkButton _buttonOpenFlag;
        [SerializeField] private ResourceSetsShop _resorceSets;
        [SerializeField] private StarsSreenShop _starScreen;
        [SerializeField] private CrystalScreenShop _crystalScreen;
        [SerializeField] private CoinsScreenShop _coinsScreen;
        [SerializeField] private FlageScreenShop _flageScreen;
        [SerializeField] private BookmarkButton _walletButtonGold;
        [SerializeField] private BookmarkButton _walletButtonCrystal;
        [SerializeField] private BookmarkButton _walletbuttonStars;
        [SerializeField] private BookmarkButton _walletGold;
        [SerializeField] private BookmarkButton _walletCrystal;
        [SerializeField] private BookmarkButton _walletStars;
        public BookmarkButton WalletGold => _walletButtonGold;
        public BookmarkButton WalletCrystal => _walletButtonCrystal;
        public BookmarkButton WalletStars => _walletbuttonStars;


        protected override void OnEnable()
        {
            _bookmark.ButtonOnClic += OpenScreen;
            _buttonStars.ButtonOnClic += OpenStarScreen;
            _buttonCrystals.ButtonOnClic += OpenCrystalScreen;
            _buttonCoin.ButtonOnClic += OpenCoinScreen;
            _buttonSet.ButtonOnClic += OpenScrollRect;
            _buttonOpenFlag.ButtonOnClic += OpenFlageScreen;
            _walletButtonGold.ButtonOnClic += OpenScreen;//OpenCoinScreen;
            _walletGold.ButtonOnClic += OpenScreen;//OpenCoinScreen;
            _walletButtonCrystal.ButtonOnClic += OpenScreen;//OpenCrystalScreen;
            _walletCrystal.ButtonOnClic += OpenScreen;//OpenCrystalScreen;
            _walletbuttonStars.ButtonOnClic += OpenScreen;//OpenStarScreen;
            _walletStars.ButtonOnClic += OpenScreen;//OpenStarScreen;
        }

        protected override void OnDisable()
        {
            _bookmark.ButtonOnClic -= OpenScreen;
            _buttonStars.ButtonOnClic -= OpenStarScreen;
            _buttonCrystals.ButtonOnClic -= OpenCrystalScreen;
            _buttonCoin.ButtonOnClic -= OpenCoinScreen;
            _buttonSet.ButtonOnClic -= OpenScrollRect;
            _buttonOpenFlag.ButtonOnClic -= OpenFlageScreen;
            _walletButtonGold.ButtonOnClic -= OpenScreen;//OpenCoinScreen;
            _walletGold.ButtonOnClic -= OpenScreen;//OpenCoinScreen;
            _walletButtonCrystal.ButtonOnClic -= OpenScreen; //OpenCrystalScreen;
            _walletCrystal.ButtonOnClic -= OpenScreen; //OpenCrystalScreen;
            _walletbuttonStars.ButtonOnClic -= OpenScreen; //OpenStarScreen;
            _walletStars.ButtonOnClic -= OpenScreen;//OpenStarScreen;
        }

        //private bool _flagAvailable = false;

        //private void Start()
        //{
        //    if (_flagAvailable)
        //    {
        //        _buttonOpenFlag.gameObject.SetActive(true);
        //    }
        //}

        private void OpenStarScreen()
        {
            Diactivate();
            if (_canvas.gameObject.activeSelf == false)
                OpenScreen();
            Activate(_starScreen.gameObject);
            //_buttonStars.Button.Select();
        }

        private void OpenCrystalScreen()
        {
            Diactivate();
            if(_canvas.gameObject.activeSelf ==false)
               OpenScreen();
            Activate(_crystalScreen.gameObject);
            //_buttonCrystals.Button.Select();
        }

        private void OpenCoinScreen()
        {
            Diactivate();
            if (_canvas.gameObject.activeSelf == false)
                OpenScreen();
            Activate(_coinsScreen.gameObject);
           // _buttonCoin.Button.Select();
        }

        private void OpenScrollRect()
        {
            Diactivate();
            Activate(_resorceSets.gameObject);
        }

        private void OpenFlageScreen()
        {
            Diactivate();
            Activate(_flageScreen.gameObject);
        }

        private void Diactivate()
        {
            _starScreen.gameObject.SetActive(false);
            _crystalScreen.gameObject.SetActive(false);
            _coinsScreen.gameObject.SetActive(false);
            _resorceSets.gameObject.SetActive(false);
            _flageScreen.gameObject.SetActive(false);
        }

        private void Activate(GameObject screen)
        {
            screen.SetActive(true);
        }
    }
}
