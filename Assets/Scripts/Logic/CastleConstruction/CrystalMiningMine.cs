using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class CrystalMiningMine : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private CrystalMine _crystalMine;

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
                PersistenProgressService.Progress.Wallet.Diamonds.Add(_currentVolume);
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
            _crystalMine = persistenProgressService.Progress.PlayerData.Building.CrystalMine;
            if (_crystalMine.IsOpen)
            {
                _isOpen = _crystalMine.IsOpen;
                _level = _crystalMine.Level;
                _currentVolume = _crystalMine.CurrentFullness;
                _convertedTime = _crystalMine.TimeText;
                _currentTimerTime = _crystalMine.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _crystalMine.IsOpen = _isOpen;
            _crystalMine.Level = _level;
            _crystalMine.CurrentFullness = _currentVolume;
            _crystalMine.CurrentTimerTime = _currentTimerTime;
            _crystalMine.TimeText = timetext;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
