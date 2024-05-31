using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Barbarian : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _durationSpecialSkill;
        private float _maxBaseDamageSpecialAttack;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float DurationSpecialSkill => _durationSpecialSkill;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueBarbarian)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueBarbarian.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueBarbarian.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueBarbarian.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueBarbarian.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueBarbarian.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueBarbarian.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _timeSpecialSkill = _durationSpecialSkill;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
            SetBarHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueBarbarian.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }
            _timeSpecialSkill = _durationSpecialSkill;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    BarbarianHero valueBarbarian = ProgressService.Progress.PlayerData.TypeHero.BarbarianHero;
                    SetAbilityValues(valueBarbarian);
                    //SpecialSkillLevelData.LoadStepCurrent(valueBarbarian.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
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
        }
    }
}
