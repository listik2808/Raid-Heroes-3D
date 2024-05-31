using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Subzero : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _freezingTime;
        private float _maxBaseDamageSpecialAttack;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float FreezingTime => _freezingTime;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }
            _timeSpecialSkill = _freezingTime;
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueSubzero)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueSubzero.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueSubzero.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueSubzero.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueSubzero.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueSubzero.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueSubzero.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            SetBarHp();
            //_currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueSubzero.GetCurrentSpecialSkill());
            //_currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueSubzero.GetCurrentHealth());
            //_currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueSubzero.GetCurrentMeleeSkill());
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueSubzero.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.SubzeroHero valueSubzero = ProgressService.Progress.PlayerData.TypeHero.SubzeroHero;
                    SetAbilityValues(valueSubzero);
                   //SpecialSkillLevelData.LoadStepCurrent(valueSubzero.CurrentStepSpecialAttack);
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

        private float GetFreezeTime()
        {
            float s = GetS(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue) / GetMaxS();
            return 2 * _stepValueUpgrage - BaseDamageSpecialAttack - 2 * (_stepValueUpgrage - BaseDamageSpecialAttack) / (1 + s);
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = GetLevelExpCurve(cerrentLevel, currentStep, maxStep,
               _baseDamageSpecialAttack, _stepValueUpgrage, true);
        }
    }
}
