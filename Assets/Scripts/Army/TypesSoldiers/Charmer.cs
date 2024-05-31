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
    public class Charmer : Soldier
    {
        [SerializeField] private float _baseDurationAction;
        private float _maxBaseDurationAction;

        private Soldier _soldierEnemy;

        public float BaseDurationAction => _baseDurationAction;
        public float MaxBaseDurationAction => _maxBaseDurationAction;

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDurationAction;
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = SetNewScpecAttackHypno();
            }
            _timeSpecialSkill = _currentValueSpecAttack;
        }

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttackHypno();
            _newSpecialValue = SetNewScpecAttackHypno();
            _timeSpecialSkill = _currentValueSpecAttack;
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueCharmer)
        {
            SetSpecAttackHypno();
            GetMaxTimeHypno();
            SpecialSkillLevelData.SetStepSkill((int)valueCharmer.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueCharmer.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueCharmer.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueCharmer.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueCharmer.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueCharmer.CurrenMeleeLevel);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newSpecialValue = SetNewScpecAttackHypno();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueCharmer.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.CharmerHero valueCharmer = ProgressService.Progress.PlayerData.TypeHero.CharmerHero;
                    SetAbilityValues(valueCharmer);
                    //SpecialSkillLevelData.LoadStepCurrent(valueCharmer.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        private void SetSpecAttackHypno()
        {
            float count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, _stepValueUpgrage, true);
            _currentValueSpecAttack = count;
        }

        private float SetNewScpecAttackHypno()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, _stepValueUpgrage, true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, _stepValueUpgrage, true);
                return count;
            }

            return 0;
        }

        private void GetMaxTimeHypno()
        {
            float count = GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue,
                _baseDurationAction, _stepValueUpgrage, true);
            _maxBaseDurationAction = count;
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = GetLevelLinear(cerrentLevel, currentStep, maxStep,
                _baseDurationAction, _stepValueUpgrage, true);
        }
    }
}
