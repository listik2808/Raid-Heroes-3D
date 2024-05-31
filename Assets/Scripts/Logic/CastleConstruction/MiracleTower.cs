using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class MiracleTower : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private MagicTower _magicTower;

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
                PersistenProgressService.Progress.Portals.Add(_currentVolume);
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
            _magicTower = persistenProgressService.Progress.PlayerData.Building.MagicTower;
            if (_magicTower.IsOpen)
            {
                _isOpen = _magicTower.IsOpen;
                _level = _magicTower.Level;
                _currentVolume = _magicTower.CurrentFullness;
                _convertedTime = _magicTower.TimeText;
                _currentTimerTime = _magicTower.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata, float timer)
        {
            _magicTower.IsOpen = _isOpen;
            _magicTower.Level = _level;
            _magicTower.CurrentFullness = _currentVolume;
            _magicTower.CurrentTimerTime = _currentTimerTime;
            _magicTower.TimeText = timedata;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
