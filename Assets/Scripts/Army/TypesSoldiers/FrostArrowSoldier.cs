using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class FrostArrowSoldier : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _maxBaseDamageSpecialAttack;
        [SerializeField] private float _baseRange;
        [SerializeField] private float _maxBaseRange;
        [SerializeField] private float _distanceStepUprgade;
        [SerializeField] private float _durationSpecialSkill;

        private float _currentRangeAttack;
        private float _newRange;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_stepValueUpgrage);
            _currentRangeAttack = _currentRangeAttack + _distanceStepUprgade;
            _currentRangeAttack = RoundUp(_currentRangeAttack);
            _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            _timeSpecialSkill = _durationSpecialSkill;
            //_newRange = _currentRangeAttack + _distanceStepUprgade;
            //_newRange = RoundUp(_newRange);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(FrostArrowSoldierHero valueFrostArrowSoldierHero)
        {
            _currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueFrostArrowSoldierHero.GetCurrentSpecialSkill());
            _currentRangeAttack = _baseRange + (_distanceStepUprgade * valueFrostArrowSoldierHero.GetCurrentSpecialSkill());
            _currentRangeAttack = RoundUp(_currentRangeAttack);
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueFrostArrowSoldierHero.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueFrostArrowSoldierHero.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            // _currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueFrostArrowSoldierHero.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _currentRangeAttack = _baseRange;
                _newRange = _currentRangeAttack + _distanceStepUprgade;
                _newRange = RoundUp(_newRange);
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            }
            _timeSpecialSkill = _durationSpecialSkill;
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.FrostArrowSoldierHero valueFrostArrowSoldierHero = ProgressService.Progress.PlayerData.TypeHero.FrostArrowSoldierHero;
                    SetAbilityValues(valueFrostArrowSoldierHero);
                    SpecialSkillLevelData.LoadStepCurrent(valueFrostArrowSoldierHero.CurrentStepSpecialAttack);
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
