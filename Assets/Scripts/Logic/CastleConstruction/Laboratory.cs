using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class Laboratory : ConstructionCastle
    {
        private Lab _lab;

        public override void FillingBar()
        {
            if (_isOpen && _isCalculationsTime && _elepsedTime < 1 && _isFull == false)
            {
                _currentVolume += _countCurrency;
                _countreward += _countCurrency;
                SetFull();
            }
            else if (_isCalculationsTime && _isOpen && _elepsedTime > 0)
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
            _lab = persistenProgressService.Progress.PlayerData.Building.Lab;
            if (_lab.IsOpen)
            {
                _isOpen = _lab.IsOpen;
                _level = _lab.Level;
                _convertedTime = _lab.TimeText;
                _currentTimerTime = _lab.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata, float timer)
        {
            _lab.IsOpen = _isOpen;
            _lab.Level = _level;
            _lab.CurrentTimerTime = _currentTimerTime;
            _lab.TimeText = timedata;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
