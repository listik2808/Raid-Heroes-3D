using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class BlacksmithShop : ConstructionCastle
    {
        [SerializeField] private AudioSource _source;
        private Forge _forge;

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
                if(_source.enabled)
                    _source.Play();
                PersistenProgressService.Progress.Gems.Green.Add(_currentVolume);
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
            _forge = persistenProgressService.Progress.PlayerData.Building.Forge;
            if (_forge.IsOpen)
            {
                _isOpen = _forge.IsOpen;
                _level = _forge.Level;
                _currentVolume = _forge.CurrentFullness;
                _convertedTime = _forge.TimeText;
                _currentTimerTime = _forge.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata, float timer)
        {
            _forge.IsOpen = _isOpen;
            _forge.Level = _level;
            _forge.CurrentFullness = _currentVolume;
            _forge.CurrentTimerTime = _currentTimerTime;
            _forge.TimeText = timedata;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
