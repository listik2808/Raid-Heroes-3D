using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class WitcherBlue : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _baseRadius;
        //[SerializeField] private float _stepValueUpgrageRadius;
        private float _maxBaseDamageSpecialAttack;
        private float _maxRadius;

        private float _currentRangeAttack;
        private float _newRange;

        public float BaseRadius => _baseRadius;
        public float MaxRadius => _maxRadius;
        public float RadiusAction => _currentRangeAttack;
        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float NewRadius => _newRange;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRadius, 2 * _baseRadius, true);
            DataSoldier.SetRange(_currentRangeAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _newRange = NewSpecRangeAttack(_currentRangeAttack);
            _agent.StoppingDistance = _currentRangeAttack;
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _currentRangeAttack = _baseRadius;
                DataSoldier.SetRange(_currentRangeAttack);
                _newRange = NewSpecRangeAttack(_baseRadius);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }

        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueWitcherBlue)
        {
            SetMaxSpecAttack();
            SetMaxRange();
            SpecialSkillLevelData.SetStepSkill((int)valueWitcherBlue.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueWitcherBlue.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueWitcherBlue.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueWitcherBlue.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueWitcherBlue.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueWitcherBlue.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRadius, 2 * _baseRadius, true);
            DataSoldier.SetRange(_currentRangeAttack);
            _newRange = NewSpecRangeAttack(_baseRadius);
            _agent.StoppingDistance = _currentRangeAttack;
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueWitcherBlue.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.WitcherBlueHero valueWitcherBlue = ProgressService.Progress.PlayerData.TypeHero.WitcherBlueHero;
                    SetAbilityValues(valueWitcherBlue);
                    //SpecialSkillLevelData.LoadStepCurrent(valueWitcherBlue.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        public void SetMaxRange()
        {
            _maxRadius = GetLevelLinear(60, 10, SpecialSkillLevelData.MaxStepValue, _baseRadius, 2 * _baseRadius, true);
        }

        private void SetMaxSpecAttack()
        {
            _maxBaseDamageSpecialAttack = GetMaxSpecAttack();
        }
        private float GetMaxSpecAttack()
        {
            return GetLevelExpCurve(60, 10, 10, _baseDamageSpecialAttack, _stepValueUpgrage);
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = GetLevelExpCurve(cerrentLevel, currentStep, maxStep,
               _baseDamageSpecialAttack, _stepValueUpgrage, true);
            _newRange = GetLevelLinear(cerrentLevel, currentStep, 10, _baseRadius, 2 * _baseRadius, true);
        }
    }
}
