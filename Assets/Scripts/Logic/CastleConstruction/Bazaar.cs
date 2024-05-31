using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class Bazaar : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private Market _market;

        public override void FillingBar()
        {
            if (_isOpen && _isCalculationsTime && _elepsedTime <= 0.3f && _isFull == false)
            {
                _currentVolume += _countCurrency;
                _countreward += _countCurrency;
                SetFull();
            }
            else if (_isCalculationsTime && _isOpen && _elepsedTime > 0 && _isFull == false)
            {
                _elepsedTime -= Time.deltaTime;
                AccrualTime();
                TranslatingString();
            }
        }

        public override void GetAward()
        {
            _currentVolume = _countreward;
            if (_currentVolume > 0)
            {
                if(_audioSource.enabled)
                    _audioSource.Play();
                _rewardMarker = 0;
                PersistenProgressService.Progress.Wallet.Coins.Add(_currentVolume);
                _currentVolume = 0;
                _countreward = 0;
                if (_isFull)
                {
                    SetFull();
                }
                else
                    TranslatingString();
                UpdataMarker();
            }
        }

        public override void LoadData(IPersistenProgressService persistenProgressService)
        {
            _persistenProgressService = persistenProgressService;
            _market = persistenProgressService.Progress.PlayerData.Building.Market;
            if (_market.IsOpen)
            {
                _isOpen = _market.IsOpen;
                _level = _market.Level;
                _currentVolume = _market.CurrentFullness;
                _convertedTime = _market.TimeText;
                _currentTimerTime = _market.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _market.IsOpen = _isOpen;
            _market.Level = _level;
            _market.CurrentFullness = _currentVolume;
            _market.CurrentTimerTime = _currentTimerTime;
            _market.TimeText = timetext;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
