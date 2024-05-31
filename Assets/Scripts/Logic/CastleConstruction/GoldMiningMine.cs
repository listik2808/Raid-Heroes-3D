using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class GoldMiningMine : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private GoldMine _goldMine;

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
            _goldMine = persistenProgressService.Progress.PlayerData.Building.GoldMine;
            if (_goldMine.IsOpen)
            {
                _isOpen = _goldMine.IsOpen;
                _level = _goldMine.Level;
                _currentVolume = _goldMine.CurrentFullness;
                _convertedTime = _goldMine.TimeText;
                _currentTimerTime = _goldMine.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _goldMine.IsOpen = _isOpen;
            _goldMine.Level = _level;
            _goldMine.CurrentFullness = _currentVolume;
            _goldMine.CurrentTimerTime = _currentTimerTime;
            _goldMine.TimeText = timetext;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
