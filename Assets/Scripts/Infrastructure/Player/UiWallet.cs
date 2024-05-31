using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.Player
{
    public class UiWallet : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private TMP_Text _diamondText;
        [SerializeField] private TMP_Text _stars;
        [SerializeField] private TMP_Text _greenGem;
        [SerializeField] private TMP_Text _portalStone;
        [SerializeField] private TMP_Text _coinUpgradeText;
        [SerializeField] private TMP_Text _diamondUpgradeText;
        [SerializeField] private TMP_Text _starsUpgradeText;
        private IPersistenProgressService _progressService;

        private float _countCoins = 0;
        private float _countStone = 0;
        private float _countGreenGem = 0;
        private float _countStarts = 0;
        private float _countDaimond = 0;
        private string _textCoins;
        private string _textStone;
        private string _textGreenGem;
        private string _textStars;
        private string _textDaimond;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            _progressService.Progress.Wallet.Coins.ShowValue -= ShowCoins;
            _progressService.Progress.Wallet.Diamonds.ShowValue -= ShowDiamond;
            _progressService.Progress.Wallet.Stars.ShowValue -= ShowStarts;
            _progressService.Progress.Gems.Green.ShowValue -= ShowGreenGem;
            _progressService.Progress.Portals.ShowValue -= ShowPortalStone;
        }

        public void Construct(IPersistenProgressService progressService)
        {
            _progressService = progressService;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            _progressService.Progress.Wallet.Coins.Count = _progressService.Progress.Wallet.Coins.Count;
            _progressService.Progress.Wallet.Diamonds.Count = _progressService.Progress.Wallet.Diamonds.Count;
            _progressService.Progress.Wallet.Stars.Count = _progressService.Progress.Wallet.Stars.Count;
            _progressService.Progress.Gems.Green.Count = _progressService.Progress.Gems.Green.Count;
            _progressService.Progress.Portals.Count = _progressService.Progress.Portals.Count;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _progressService.Progress.Wallet.Coins.ShowValue += ShowCoins;
            _progressService.Progress.Wallet.Diamonds.ShowValue += ShowDiamond;
            _progressService.Progress.Wallet.Stars.ShowValue += ShowStarts;
            _progressService.Progress.Gems.Green.ShowValue += ShowGreenGem;
            _progressService.Progress.Portals.ShowValue += ShowPortalStone;
            ShowCoins(_progressService.Progress.Wallet.Coins.Count);
            ShowDiamond(_progressService.Progress.Wallet.Diamonds.Count);
            ShowStarts(_progressService.Progress.Wallet.Stars.Count);
        }

        private void ShowCoins(float value)
        {
            _countCoins = (float)Math.Round(value);
            _textCoins = AbbreviationsNumbers.ShortNumber(_countCoins);
            _coinText.text = _textCoins;
            _coinUpgradeText.text = _textCoins;
        }

        private void ShowDiamond(float value)
        {
            _countDaimond = (float)Math.Round(value);
            _textDaimond = AbbreviationsNumbers.ShortNumber(_countDaimond);
            _diamondText.text = _textDaimond;
            _diamondUpgradeText.text = _textDaimond;
        }

        private void ShowStarts(float value)
        {
            _countStarts = (float)Math.Round(value);
            _textStars = AbbreviationsNumbers.ShortNumber(_countStarts);
            _stars.text = _textStars;
            _starsUpgradeText.text = _textStars;
        }

        private void ShowGreenGem(float value)
        {
            _countGreenGem = (float)Math.Round(value);
            _textGreenGem = AbbreviationsNumbers.ShortNumber(_countGreenGem);
            _greenGem.text = _textGreenGem;
        }

        private void ShowPortalStone(float value)
        {
            _countStone = (float)Math.Round(value);
            _textStone = AbbreviationsNumbers.ShortNumber(_countStone);
            _portalStone.text = _textStone;
        }
    }
}