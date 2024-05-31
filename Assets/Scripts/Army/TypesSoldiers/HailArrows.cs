using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class HailArrows : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _maxBaseDamageSpecialAttack;
        [SerializeField] private float _baseRadiusSpreadArrows;
        [SerializeField] private float _radiusSpreadArrowsStepUprgade;

        private float _currentRadiusSpreadArrows;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;

        public override void SpecialSkillUpgrade()
        {
            _currentValueSpecAttack = _currentValueSpecAttack + _stepValueUpgrage;
            _currentRadiusSpreadArrows = _currentRadiusSpreadArrows + _radiusSpreadArrowsStepUprgade;
            _currentRadiusSpreadArrows = RoundUp(_currentRadiusSpreadArrows);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueHailArrowsSoldierHero)
        {
            _currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueHailArrowsSoldierHero.GetCurrentSpecialSkill());
            _currentRadiusSpreadArrows = _baseRadiusSpreadArrows + (_radiusSpreadArrowsStepUprgade * valueHailArrowsSoldierHero.GetCurrentSpecialSkill());
            _currentRadiusSpreadArrows = RoundUp(_currentRadiusSpreadArrows);
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueHailArrowsSoldierHero.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueHailArrowsSoldierHero.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            // _currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueHailArrowsSoldierHero.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _currentRadiusSpreadArrows = _baseRadiusSpreadArrows;
                _currentRadiusSpreadArrows = RoundUp(_currentRadiusSpreadArrows);
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            }
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.HailArrowsSoldierHero valueHailArrowsSoldierHero = ProgressService.Progress.PlayerData.TypeHero.HailArrowsSoldierHero;
                    SetAbilityValues(valueHailArrowsSoldierHero);
                    SpecialSkillLevelData.LoadStepCurrent(valueHailArrowsSoldierHero.CurrentStepSpecialAttack);
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
