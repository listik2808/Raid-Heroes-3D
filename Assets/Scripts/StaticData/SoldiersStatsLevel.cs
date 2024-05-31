using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using System;
using UnityEngine;

namespace Scripts.StaticData
{
    public class SoldiersStatsLevel : MonoBehaviour
    {
        private float _bestSurvivalRateLevel=0;
        private float _bestMeleerateLevel=0;
        private float _bestLevelSpecialSkill = 0;
        private float _bestLevelMobility = 0;
        private float _currenSurvivabilityLevel =1;
        private float _currenMeleeLevel =1;
        private float _currenSpecialAttack = 1;
        private float _currenSpeedLevel = 1;
        private float _maxLevelStatsHero = 60;

        public float MaxLevelStatsHero => _maxLevelStatsHero;
        public float BestSurvivalRateLevel => _bestSurvivalRateLevel;
        public float BestMeleerateLevel => _bestMeleerateLevel;
        public float BestLevelSpecialSkill => _bestLevelSpecialSkill;
        public float BestLevelMobility => _bestLevelMobility;

        public event Action UpdateCurrentLevelSkill;
        public event Action UpdateBestLevelSkill;

        public float MaxLevelParam(Rank rank)
        {
            _maxLevelStatsHero = (rank.MaxLevelSoldier + 1) * 10;
            return _maxLevelStatsHero;
        }

        public int GetMaxLevelOrCurrentLevel(int levelRank)
        {
            return (levelRank + 1) * 10;
        }

        public float CurrentSurvivabilityLevel
        {
            get => _currenSurvivabilityLevel;
            set
            {
                if(_currenSurvivabilityLevel != value)
                {
                    _currenSurvivabilityLevel = value;
                    UpdateCurrentLevelSkill?.Invoke();
                    SetBestSurvivalRateLevel();
                }
            }
        }
        public float CurrentMeleelevel
        {
            get => _currenMeleeLevel;
            set
            {
                if(_currenMeleeLevel != value)
                {
                    _currenMeleeLevel = value;
                    UpdateCurrentLevelSkill?.Invoke();
                    SetBestMeleelevel();
                }
            }
        }
        public float CurrentLevelSpecialSkill
        {
            get => _currenSpecialAttack;
            set
            {
                if(_currenSpecialAttack != value)
                {
                    _currenSpecialAttack = value;
                    UpdateCurrentLevelSkill?.Invoke();
                    SetBestLevelSpecialSkill();
                }
            }
        }
        public float CurrentSpeedLevel
        {
            get => _currenSpeedLevel;
            set
            {
                if(_currenSpeedLevel != value)
                {
                    _currenSpeedLevel = value;
                    UpdateCurrentLevelSkill?.Invoke();
                    SetBestMobilityLevel();
                }
            }
        }

        public void SetBestSurvivalRateLevel()
        {
            if (_currenSurvivabilityLevel > _bestSurvivalRateLevel)
            {
                _bestSurvivalRateLevel = _currenSurvivabilityLevel;
                UpdateBestLevelSkill?.Invoke();
            }
        }

        public void SetBestMeleelevel()
        {
            if (_currenMeleeLevel > _bestMeleerateLevel)
            {
                _bestMeleerateLevel = _currenMeleeLevel;
                UpdateBestLevelSkill?.Invoke();
            }
        }

        public void SetBestLevelSpecialSkill()
        {
            if (_currenSpecialAttack > _bestLevelSpecialSkill)
            {
                _bestLevelSpecialSkill = _currenSpecialAttack;
                UpdateBestLevelSkill?.Invoke();
            }
        }

        public void SetBestMobilityLevel()
        {
            if (_currenSpeedLevel > _bestLevelMobility)
            {
                _bestLevelMobility = _currenSpeedLevel;
                UpdateBestLevelSkill?.Invoke();
            }
        }

        public void AddEnemyLevelSpecialSkill( int value)
        {
            _currenSpecialAttack += value;
        }

        public void AddEnemyMeleeLevel( int value)
        {
            _currenMeleeLevel += value;
        }

        public void AddenemyHealthLevel( int value)
        {
            _currenSurvivabilityLevel += value;
        }

        public void SetMeleeLevel(int value)
        {
            if (value > 0)
                _currenMeleeLevel = value;
        }

        public void SetHealthLevel(int value)
        {
            if ( value >0)
                _currenSurvivabilityLevel = value;
        }

        public void SetSpecAttac(int value)
        {
            if (value > 0)
                _currenSpecialAttack = value;
        }

        public void ResetLevel()
        {
            _currenMeleeLevel = 0;
            _currenSpecialAttack = 0;
            _currenSurvivabilityLevel = 0;
        }
    }
}
