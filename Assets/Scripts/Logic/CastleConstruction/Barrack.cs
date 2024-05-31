using Scripts.Army.TypesSoldiers;
using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class Barrack : ConstructionCastle
    {
        [SerializeField] private AudioSource _audioSource;
        private Barracks _barracks;

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

        //private List<Soldier>_soldierSimple = new List<Soldier>();

        public override void GetAward()
        {
            _rewardMarker = 0;
            _currentVolume = _countreward;
            if (_currentVolume > 0)
            {
                if(_audioSource.enabled)
                    _audioSource.Play();
                //SetListSoldiers();
                SetCard(1);
                UpdataMarker();
            }
        }

        public override void LoadData(IPersistenProgressService persistenProgressService)
        {
            _persistenProgressService = persistenProgressService;
            _barracks = persistenProgressService.Progress.PlayerData.Building.Barracks;
            if (_barracks.IsOpen)
            {
                _isOpen = _barracks.IsOpen;
                _level = _barracks.Level;
                _currentVolume = _barracks.CurrentFullness;
                _convertedTime = _barracks.TimeText;
                _currentTimerTime = _barracks.CurrentTimerTime;
                CalculateTime();
            }
        }

        public override void SavaData(string timetext, float _currentTimerTime)
        {
            _barracks.IsOpen = _isOpen;
            _barracks.Level = _level;
            _barracks.CurrentFullness = _currentVolume;
            _barracks.CurrentTimerTime = _currentTimerTime;
            _barracks.TimeText = timetext;
        }
        
        private void Update()
        {
            if (_isOpen == false)
                return;

            FillingBar();
        }

        //private void SetListSoldiers()
        //{
        //    foreach (Soldier item in PlayerData.Cards.Soldiers)
        //    {
        //        if(item.DataSoldier.Type == CardType.Simple)
        //        {
        //            _soldierSimple.Add(item);
        //        }
        //    }
        //}
    }
}
