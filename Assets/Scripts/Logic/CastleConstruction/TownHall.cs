using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class TownHall : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;

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
                {
                    TranslatingString();
                }
                UpdataMarker();
            }
        }

        public override void LoadData(IPersistenProgressService persistenProgressService)
        {
            _persistenProgressService = persistenProgressService;
            if (persistenProgressService.Progress.PlayerData.Building.Castle.IsOpen)
            {
                _isOpen = persistenProgressService.Progress.PlayerData.Building.Castle.IsOpen;
                _level = persistenProgressService.Progress.PlayerData.Building.Castle.Level;
                _currentVolume = persistenProgressService.Progress.PlayerData.Building.Castle.CurrentFullness;
                _convertedTime = persistenProgressService.Progress.PlayerData.Building.Castle.TimeText;
                _currentTimerTime = persistenProgressService.Progress.PlayerData.Building.Castle.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata,float timer)
        {
            PersistenProgressService.Progress.PlayerData.Building.Castle.IsOpen = _isOpen;
            PersistenProgressService.Progress.PlayerData.Building.Castle.Level = _level;
            PersistenProgressService.Progress.PlayerData.Building.Castle.CurrentFullness = _currentVolume;
            PersistenProgressService.Progress.PlayerData.Building.Castle.CurrentTimerTime = timer;
            PersistenProgressService.Progress.PlayerData.Building.Castle.TimeText = timedata;
           
        }

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
    }
}
