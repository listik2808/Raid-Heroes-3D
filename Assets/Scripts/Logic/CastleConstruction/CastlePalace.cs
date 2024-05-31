using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class CastlePalace : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private Palace _palace;

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
            _rewardMarker = 0;
            _currentVolume = _countreward;
            if (_currentVolume > 0)
            {
                if(_audioSource.enabled)
                    _audioSource.Play();
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
            _palace = persistenProgressService.Progress.PlayerData.Building.Palace;
            if (_palace.IsOpen)
            {
                _isOpen = _palace.IsOpen;
                _level = _palace.Level;
                _currentVolume = _palace.CurrentFullness;
                _convertedTime = _palace.TimeText;
                _currentTimerTime = _palace.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata, float timer)
        {
            _palace.IsOpen = _isOpen;
            _palace.Level = _level;
            _palace.CurrentFullness = _currentVolume;
            _palace.CurrentTimerTime = timer;
            _palace.TimeText = timedata;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
