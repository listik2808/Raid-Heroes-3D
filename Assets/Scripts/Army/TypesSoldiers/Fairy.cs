using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Fairy : Soldier
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
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueFairy)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueFairy.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueFairy.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueFairy.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueFairy.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueFairy.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueFairy.CurrenMeleeLevel);
            SetSpecAttack(_baseValue);
            _newSpecialValue = NewSpecAttack(_baseValue);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueFairy.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    FairyHero valueFairy = ProgressService.Progress.PlayerData.TypeHero.FairyHero;
                    SetAbilityValues(valueFairy);
                    //SpecialSkillLevelData.LoadStepCurrent(valueFairy.CurrentStepSpecialAttack);
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
