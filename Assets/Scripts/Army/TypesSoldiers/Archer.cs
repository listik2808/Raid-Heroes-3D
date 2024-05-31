using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Archer : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _baseRange;
        //[SerializeField] private float _distanceStepUprgade;
        private float _maxBaseDamageSpecialAttack;
        private float _maxBaseRange;

        private float _currentRangeAttack;
        private float _newRange;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float BaseRange => _baseRange;
        public float MaxBaseRange => _maxBaseRange;
        public float CurrentRangeAttack => _currentRangeAttack;
        public float NewValueRange => _newRange;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill,SpecialSkillLevelData.CurrentStepSkill,SpecialSkillLevelData.MaxStepValue,
                _baseRange,2 * _baseRange, true);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _newRange = NewSpecRangeAttack(_baseRange);
            DataSoldier.SetRange(_currentRangeAttack);
            _agent.StoppingDistance = _currentRangeAttack;
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueArcherHero)
        {
            SetMaxSpecAttack();
            SetMaxRange();
            SpecialSkillLevelData.SetStepSkill((int)valueArcherHero.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueArcherHero.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueArcherHero.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueArcherHero.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueArcherHero.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueArcherHero.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _currentRangeAttack = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRange, 2 * _baseRange, true);
            DataSoldier.SetRange(_currentRangeAttack);
            _agent.StoppingDistance = _currentRangeAttack;
            _newRange = NewSpecRangeAttack(_baseRange);
            SetBarHp();
            //---//
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueArcherHero.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _currentRangeAttack = _baseRange;
                _agent.StoppingDistance = _currentRangeAttack;
                DataSoldier.SetRange(_currentRangeAttack);
                _newRange = NewSpecRangeAttack(_baseRange);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
            }
        }

        public override void SetCharacteristics()
        {
            if(SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.ArcherHero valueArcherHero = ProgressService.Progress.PlayerData.TypeHero.ArcherHero;
                    SetAbilityValues(valueArcherHero);
                    //SpecialSkillLevelData.LoadStepCurrent(valueArcherHero.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        public void SetMaxRange()
        {
            _maxBaseRange = GetLevelLinear(60,10, SpecialSkillLevelData.MaxStepValue,_baseRange, 2 * _baseRange, true);
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
            _newRange = GetLevelLinear(cerrentLevel, currentStep, 10, _baseRange, 2 * _baseRange, true);
        }
    }
}
