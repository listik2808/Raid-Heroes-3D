using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.Logic;
using Scripts.Logic.TaskAchievements;
using Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public abstract class ExchangingCrystalsGold : MonoBehaviour
    {
        public const string Gold = " Монет";
        [SerializeField] protected FightNumber _flightNumber;
        [SerializeField] protected BookmarkButton _rewardButton;
        [SerializeField] protected BookmarkButton _buttonClose;
        [SerializeField] protected BookmarkButton _buttonOpenCanvas;
        [SerializeField] protected BookmarkButton _buttonOpenCanvasInfo;
        [SerializeField] protected int _price;
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected TMP_Text _textRewardCanvas;
        [SerializeField] protected TMP_Text _textRewardCard;
        [SerializeField] protected ScreenNoPay _screenNoPay;
        [SerializeField] protected BuyGoodsStoreAchievement BuyGoodsStoreAchievement;

        protected float _reward;
        private int _multiplicatator = 1;
        protected IPersistenProgressService _progressService;

        private void Start()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        protected void ExchangeSetS()
        {
            _reward = Price.GetLastPVERewardGold(_flightNumber,_multiplicatator);
            string result = AbbreviationsNumbers.ShortNumber(_reward);
            float resultValue = AbbreviationsNumbers.Value;
            int rounding = (int)Math.Round(resultValue);
            string roundingChar = AbbreviationsNumbers.Chars[AbbreviationsNumbers.Number];
            _textRewardCanvas.text = rounding.ToString() + roundingChar;
            _textRewardCard.text = rounding.ToString() + roundingChar + Gold;
        }

        protected void ExchangeSetM()
        {
            _reward = Price.GetLastPVERewardGold(_flightNumber, 15 * _multiplicatator);
            string result = AbbreviationsNumbers.ShortNumber(_reward);
            float resultValue = AbbreviationsNumbers.Value;
            int rounding = (int)Math.Round(resultValue);
            string roundingChar = AbbreviationsNumbers.Chars[AbbreviationsNumbers.Number];
            _textRewardCanvas.text = rounding.ToString() + roundingChar;
            _textRewardCard.text = rounding.ToString() + roundingChar + Gold;
        }

        protected void ExchangeSetL()
        {
            _reward = Price.GetLastPVERewardGold(_flightNumber, 200 * _multiplicatator);
            string result = AbbreviationsNumbers.ShortNumber(_reward);
            float resultValue = AbbreviationsNumbers.Value;
            int rounding = (int)Math.Round(resultValue);
            string roundingChar = AbbreviationsNumbers.Chars[AbbreviationsNumbers.Number];
            _textRewardCanvas.text = rounding.ToString() + roundingChar;
            _textRewardCard.text = rounding.ToString() + roundingChar + Gold;
        }

        protected void TryExchangeCurrency()
        {
            if (_progressService.Progress.Wallet.Diamonds.Count >= _price)
            {
                BuyGoodsStoreAchievement.RecordPurchase();
                _progressService.Progress.Wallet.Diamonds.Reduce(_price);
                _progressService.Progress.Wallet.Coins.Add(_reward);
            }
            else
            {
                _screenNoPay.gameObject.SetActive(true);
            }
        }

        public abstract void OpenCanvas();

        protected void CloseCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}