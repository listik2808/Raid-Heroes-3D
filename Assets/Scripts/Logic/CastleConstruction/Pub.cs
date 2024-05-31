using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class Pub : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private Tavern _tavern;

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
                SetCard(2);
                UpdataMarker();
            }
        }

        public override void LoadData(IPersistenProgressService persistenProgressService)
        {
            _persistenProgressService = persistenProgressService;
            _tavern = persistenProgressService.Progress.PlayerData.Building.Tavern;
            if (_tavern.IsOpen)
            {
                _isOpen = _tavern.IsOpen;
                _level = _tavern.Level;
                _currentVolume = _tavern.CurrentFullness;
                _convertedTime = _tavern.TimeText;
                _currentTimerTime = _tavern.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _tavern.IsOpen = _isOpen;
            _tavern.Level = _level;
            _tavern.CurrentFullness = _currentVolume;
            _tavern.CurrentTimerTime = _currentTimerTime;
            _tavern.TimeText = timetext;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
