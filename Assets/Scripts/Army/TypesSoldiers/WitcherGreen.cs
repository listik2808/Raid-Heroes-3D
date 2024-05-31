using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class WitcherGreen : Soldier
    {
        [SerializeField] private float _baseValue;
        private float _maxBaseValue;

        public float BaseValue => _baseValue;
        public float MaxBaseValue => _maxBaseValue;

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseValue;
                _newSpecialValue = NewSpecAttack(_baseValue);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            }
        }

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseValue);
            _newSpecialValue = NewSpecAttack(_baseValue);
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueWitcherGreen)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueWitcherGreen.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueWitcherGreen.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueWitcherGreen.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueWitcherGreen.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueWitcherGreen.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueWitcherGreen.CurrenMeleeLevel);
            SetSpecAttack(_baseValue);
            _newSpecialValue = NewSpecAttack(_baseValue);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueWitcherGreen.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.WitcherGreenHero valueWitcherGreen = ProgressService.Progress.PlayerData.TypeHero.WitcherGreenHero;
                    SetAbilityValues(valueWitcherGreen);
                   // SpecialSkillLevelData.LoadStepCurrent(valueWitcherGreen.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        private void SetMaxSpecAttack()
        {
            _maxBaseValue = GetMaxSpecAttack();
        }
        private float GetMaxSpecAttack()
        {
            return GetLevelExpCurve(60, 10, 10, _baseValue, _stepValueUpgrage);
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = GetLevelExpCurve(cerrentLevel, currentStep, maxStep,
               _baseValue, _stepValueUpgrage, true);
        }
    }
}
