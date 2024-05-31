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
    public class Commander : Soldier
    {
        [SerializeField] private float _baseMultiplier;
        [SerializeField] private float _minDurationAction;
        //[SerializeField] private float _stepDurationAction;
        private float _maxDurationAction;
        private float _maxBaseMultiplier;

        private float _currentDurationAction;
        private float _newDurationAction;

        public float BaseMultiplier => _baseMultiplier;
        public float MaxMultiplier => _maxBaseMultiplier;
        public float MinDurationAction => _minDurationAction;
        public float MaxDurationAction => _maxDurationAction;
        public float CurrentDurationAction => _currentDurationAction;
        public float NewDuarationAction => _newDurationAction;

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseMultiplier;
                _newSpecialValue = NewSpecAttackMotivation();
                _currentDurationAction = GetMotimvationTime();
                _newDurationAction = NewMotivationDurationTime();
                SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
                SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            }
            _timeSpecialSkill = _currentDurationAction;
        }

        public override void SpecialSkillUpgrade()
        {
            SpecAttaclMotivation();
            _newSpecialValue = NewSpecAttackMotivation();
            _currentDurationAction = GetMotimvationTime();
            _newDurationAction = NewMotivationDurationTime();
            _timeSpecialSkill = _currentDurationAction;
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueCommander)
        {
            _maxBaseMultiplier = GetMaxMotivationMultiplier();
            _maxDurationAction = GetMaxMotivationTime();
            SpecialSkillLevelData.SetStepSkill((int)valueCommander.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueCommander.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueCommander.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueCommander.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueCommander.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueCommander.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SpecAttaclMotivation();
            _currentDurationAction = GetMotimvationTime();
            _newDurationAction = NewMotivationDurationTime();
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            SetBarHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueCommander.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.CommanderHero valueCommander = ProgressService.Progress.PlayerData.TypeHero.CommanderHero;
                    SetAbilityValues(valueCommander);
                    //SpecialSkillLevelData.LoadStepCurrent(valueCommander.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }
        private void SpecAttaclMotivation()
        {
            var count = 1+ GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
               _baseMultiplier -1,2 * (_baseMultiplier -1),true);
            _currentValueSpecAttack = count;
            SpecialAttackChanges();
        }

        private float GetMaxMotivationMultiplier()
        {
            return 1 + GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue,_baseMultiplier -1, 2 * (_baseMultiplier -1),true);
        }

        private float GetMotimvationTime()
        {
            return GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue
                ,_minDurationAction, 2 * _minDurationAction,true);
        }

        private float GetMaxMotivationTime()
        {
            return GetLevelLinear(60,SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue,_minDurationAction, 2 * _minDurationAction);
        }

        private float NewSpecAttackMotivation()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count = 1+ GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier -1, 2 * (_baseMultiplier -1),true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill +1, 0, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1),true);
                return count;
            }
            return 0;
        }

        private float NewMotivationDurationTime()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _minDurationAction, 2 * _minDurationAction, true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _minDurationAction, 2 * _minDurationAction, true);
                return count;
            }
            return 0;
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = 1+ GetLevelLinear(cerrentLevel, currentStep, maxStep,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
            _newDurationAction = GetLevelLinear(cerrentLevel, currentStep, maxStep,
                _minDurationAction, 2 * _minDurationAction, true);
        }
    }
}
