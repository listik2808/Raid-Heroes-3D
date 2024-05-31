using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class HeroesLobby : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private HeroesHall _heroesHall;

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
                SetCard(3);
                UpdataMarker();
            }
        }

        public override void LoadData(IPersistenProgressService persistenProgressService)
        {
            _persistenProgressService = persistenProgressService;
            _heroesHall = persistenProgressService.Progress.PlayerData.Building.HeroesHall;
            if (_heroesHall.IsOpen)
            {
                _isOpen = _heroesHall.IsOpen;
                _level = _heroesHall.Level;
                _currentVolume = _heroesHall.CurrentFullness;
                _convertedTime = _heroesHall.TimeText;
                _currentTimerTime = _heroesHall.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timedata, float timer)
        {
            _heroesHall.IsOpen = _isOpen;
            _heroesHall.Level = _level;
            _heroesHall.CurrentFullness = _currentVolume;
            _heroesHall.CurrentTimerTime = _currentTimerTime;
            _heroesHall.TimeText = timedata;
        }

        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }
    }
}
