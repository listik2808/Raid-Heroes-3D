using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Berserk : Soldier
    {
        [SerializeField] private float _baseMultiplier;
        [SerializeField] private float _baseDurationAction;
        private float _maxBaseDurationAction;
        private float _maxBaseMultiplier;

        private float _currentDurationAction;
        private float _newDurationAction;

        public float BaseMultiplier => _baseMultiplier;
        public float MaxBaseMultiplier => _maxBaseMultiplier;
        public float CurrentDurationAction => _currentDurationAction;
        public float NewDuarationAction => _newDurationAction;
        public float MinDurationAction => _baseDurationAction;
        public float MaxDurationAction => _maxBaseDurationAction;

        public override void BaseData()
        {
            SetBaseData();
            if( _currentValueSpecAttack == 0)
            {
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
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueBerserk)
        {
            _maxBaseMultiplier = GetMaxMotivationMultiplier();
            _maxBaseDurationAction = GetMaxMotivationTime();
            SpecialSkillLevelData.SetStepSkill((int)valueBerserk.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueBerserk.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueBerserk.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueBerserk.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueBerserk.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueBerserk.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SpecAttaclMotivation();
            _newSpecialValue = NewSpecAttackMotivation();
            _currentDurationAction = GetMotimvationTime();
            _newDurationAction = NewMotivationDurationTime();
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueBerserk.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.BerserkHero valueBerserk = ProgressService.Progress.PlayerData.TypeHero.BerserkHero;
                    SetAbilityValues(valueBerserk);
                    //SpecialSkillLevelData.LoadStepCurrent(valueBerserk.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        private void SpecAttaclMotivation()
        {
            var count =1+ GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
               _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
            _currentValueSpecAttack = count;
            SpecialAttackChanges();
        }

        private float GetMaxMotivationMultiplier()
        {
            return 1 + GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue, _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
        }

        private float GetMotimvationTime()
        {
            return GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue
                , _baseDurationAction, 2 * _baseDurationAction, true);
        }

        private float GetMaxMotivationTime()
        {
            return GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue, _baseDurationAction, 2 * _baseDurationAction);
        }

        private float NewSpecAttackMotivation()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count =1+ GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
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
                count =  GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, 2 * _baseDurationAction, true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, 2 * _baseDurationAction, true);
                return count;
            }
            return 0;
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = 1+ GetLevelLinear(cerrentLevel, currentStep, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
            _newDurationAction = GetLevelLinear(cerrentLevel, currentStep, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, 2 * _baseDurationAction, true);
        }
    }
}
