using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.SpecificationsUI;
using Scripts.Logic.TaskAchievements;
using System;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class Rank : MonoBehaviour
    {
        [SerializeField] private int _maxLevelHero = 5;
        private int _currentLevelHero = -1;
        private int _maxLevelSkill = 10;
        private int _currentCountCard =0;
        private int _newLevel;
        private int _maxCountCard =5;
        private int _maxStepSkill = 10;
        private Soldier _soldier;
        private RankUI _rankUI;
        public int CurrentCountCard {
            get
            {
                return _currentCountCard;
            }
            set
            {
                if (_currentCountCard != value)
                {
                    _currentCountCard = value;
                    ChangeCountCard?.Invoke();
                }
            }
        }

        public event Action ChangeCountCardRang;
        public event Action UpdateLevelHero;
        public event Action ChangeCountCard;
        public event Action BackgroundChanged;
        public event Action<Soldier> AddCardCount;
        public event Action LevelUp;

        public int MaxLevelSoldier => _maxLevelHero;
        public int CurrentLevelHero => _currentLevelHero;
        public int MaxLevelSkill => _maxLevelSkill;
        public int MaxCountCard => _maxCountCard;
        public int MaxStepSkill => _maxStepSkill;

        public void UpLevelHero(RankUI rankUI,IPersistenProgressService persistenProgressService,HireHeroes hireHeroes)
        {
            _rankUI = rankUI;

            _newLevel = _currentLevelHero;
            _newLevel += 1;
            if (_currentLevelHero == -1)
            {
                //persistenProgressService.Progress.PlayerData.Cards.GetType(_soldier);
                _soldier.OpenHeroCard(true);
                _soldier.DataSoldier.SetCardActivation();
                persistenProgressService.Progress.Achievements.HeroTypeIds.Add(_soldier.HeroTypeId);
                hireHeroes.SetCountHireSoldiers(persistenProgressService);
                _currentLevelHero = _newLevel;
                _maxCountCard += _maxStepSkill;
                BackgroundChanged?.Invoke();
            }

            if (_newLevel <= _maxLevelHero && _newLevel != 0)
            {
                _currentLevelHero = _newLevel;
                _maxLevelSkill += _maxLevelSkill;
                _maxCountCard = (_maxStepSkill * _currentLevelHero)+ _maxStepSkill + CurrentCountCard;
            }

            
            UpdateLevelHero?.Invoke();
            ChangeCountCardRang?.Invoke();
            LevelUp?.Invoke();
            _rankUI.SetRang(_soldier);
            _rankUI.SetPrice(_soldier,persistenProgressService);
            _rankUI.DiactivateButtonPrice();

        }

        public void AddCountCard(int value)
        {
            _currentCountCard += value;
            AddCardCount?.Invoke(_soldier);
            ChangeCountCard?.Invoke();
        }

        public void SetSoldier(Soldier soldier)
        {
            if(_soldier == null)
                _soldier = soldier;
        }

        public void SetLevelHero(int value,int countCard)
        {
            if (value > _currentLevelHero)
            {
                if (value == 0)
                {
                    _currentLevelHero = value;
                    _maxCountCard += _maxStepSkill;
                    _currentCountCard = countCard;
                }
                else
                {
                    _currentCountCard = _maxCountCard + _maxStepSkill;
                    _maxLevelSkill = (value * _maxStepSkill) + _maxStepSkill;
                    _currentLevelHero = 0;
                    for (int i = 0; i < value; i++)
                    {
                        _currentLevelHero += 1;
                        _maxCountCard = (_maxStepSkill * _currentLevelHero) + _maxStepSkill + CurrentCountCard;
                        _currentCountCard = _maxCountCard;
                    }
                    _currentCountCard = countCard;
                }
            }
            ChangeCountCardRang?.Invoke();
        }

        public void SetLevelHero(int level)
        {
            _currentLevelHero = level;
        }

        public void SetMaxCountCard(int maxCardCount)
        {
            _maxCountCard = maxCardCount;
        }
    }
}
