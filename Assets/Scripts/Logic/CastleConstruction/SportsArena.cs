using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class SportsArena : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private Arena _arena;

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
                PersistenProgressService.Progress.Wallet.Stars.Add(_currentVolume);
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
            _arena = persistenProgressService.Progress.PlayerData.Building.Arena;
            if (_arena.IsOpen)
            {
                _isOpen = _arena.IsOpen;
                _level = _arena.Level;
                _currentVolume = _arena.CurrentFullness;
                _convertedTime = _arena.TimeText;
                _currentTimerTime = _arena.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _arena.IsOpen = _isOpen;
            _arena.Level = _level;
            _arena.CurrentFullness = _currentVolume;
            _arena.CurrentTimerTime = _currentTimerTime;
            _arena.TimeText = timetext;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
