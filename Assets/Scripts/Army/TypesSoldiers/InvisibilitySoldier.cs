using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class InvisibilitySoldier : Soldier
    {
        [SerializeField] private float _baseMultiplier;
        [SerializeField] private float _maxBaseMultiplier;
        [SerializeField] private float _minDurationAction;
        [SerializeField] private float _maxDurationAction;
        [SerializeField] private float _stepDurationAction;

        private float _currenDurationAction;
        //private float _newDurationAction;

        //public float NewDurationAction => _newDurationAction;
        public float CurrentDurationAction => _currenDurationAction;
        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_stepValueUpgrage);
            _currentValueSpecAttack = RoundUp(_currentValueSpecAttack);
            //_newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            _currenDurationAction = _currenDurationAction + _stepDurationAction;
            _currenDurationAction = RoundUp(_currenDurationAction);
            //_newDurationAction = _currenDurationAction + _stepDurationAction;
            //_newDurationAction = RoundUp(_newDurationAction);
            _timeSpecialSkill = _currenDurationAction;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(InvisibilitySoldierHero valueInvisibilitySoldierHero)
        {
            _currentValueSpecAttack = _baseMultiplier + (_stepValueUpgrage * valueInvisibilitySoldierHero.GetCurrentSpecialSkill());
            _currentValueSpecAttack = RoundUp(_currentValueSpecAttack);
            _currenDurationAction = _minDurationAction + (_stepDurationAction * valueInvisibilitySoldierHero.GetCurrentSpecialSkill());
            _currenDurationAction = RoundUp(_currenDurationAction);
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueInvisibilitySoldierHero.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueInvisibilitySoldierHero.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueInvisibilitySoldierHero.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseMultiplier;
                //_newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                _currenDurationAction = _minDurationAction;
                _currenDurationAction = RoundUp(_currenDurationAction);
                //_newDurationAction = _currenDurationAction + _stepDurationAction;
                //_newDurationAction = RoundUp(_newDurationAction);
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
            }
            _timeSpecialSkill = _currenDurationAction;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.InvisibilitySoldierHero valueInvisibilitySoldierHero = ProgressService.Progress.PlayerData.TypeHero.InvisibilitySoldierHero;
                    SetAbilityValues(valueInvisibilitySoldierHero);
                    SpecialSkillLevelData.LoadStepCurrent(valueInvisibilitySoldierHero.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
                
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            
        }
    }
}
