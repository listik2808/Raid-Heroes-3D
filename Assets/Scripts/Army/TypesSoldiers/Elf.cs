using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Elf : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _baseRange;
        //[SerializeField] private float _distanceStepUprgade;
        private float _maxBaseRange;
        private float _maxBaseDamageSpecialAttack;

        private float _currentRangeAttack;
        private float _newRange;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float BaseRange => _baseRange;
        public float MaxBaseRange => _maxBaseRange;
        public float CurrentRangeAttack => _currentRangeAttack;
        public float NewValueRange => _newRange;

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _currentRangeAttack = _baseRange;
                DataSoldier.SetRange(_currentRangeAttack);
                _newRange = NewSpecRangeAttack(_baseRange);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
            }
        }

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _currentRangeAttack, 2 * _currentRangeAttack, true);
            DataSoldier.SetRange(_currentRangeAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _newRange = NewSpecRangeAttack(_currentRangeAttack);
            _agent.StoppingDistance = _currentRangeAttack;
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueElf)
        {
            SetMaxSpecAttack();
            SetMaxRange();
            SpecialSkillLevelData.SetStepSkill((int)valueElf.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueElf.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueElf.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueElf.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueElf.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueElf.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRange, 2 * _baseRange, true);
            DataSoldier.SetRange(_currentRangeAttack);
            _newRange = NewSpecRangeAttack(_baseRange);
            _agent   .StoppingDistance = _currentRangeAttack;
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueElf.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.ElfHero valueElf = ProgressService.Progress.PlayerData.TypeHero.ElfHero;
                    SetAbilityValues(valueElf);
                    //SpecialSkillLevelData.LoadStepCurrent(valueElf.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        public void SetMaxRange()
        {
            _maxBaseRange = GetLevelLinear(60, 10, SpecialSkillLevelData.MaxStepValue, _baseRange, 2 * _baseRange, true);
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
            _newSpecialValue = GetLevelExpCurve(cerrentLevel, currentStep, SpecialSkillLevelData.MaxStepValue,
               _baseDamageSpecialAttack, _stepValueUpgrage, true);
            _newRange = GetLevelLinear(cerrentLevel, currentStep, 10, _baseRange, 2 * _baseRange, true);
        }
    }
}
