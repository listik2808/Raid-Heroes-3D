using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Viking : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _durationSpecialSkill;
        private float _maxBaseDamageSpecialAttack;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float RestoringSpecialSkill => _durationSpecialSkill;

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
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }
            _timeSpecialSkill = _durationSpecialSkill;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueViking)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueViking.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueViking.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueViking.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueViking.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueViking.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueViking.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _timeSpecialSkill = _durationSpecialSkill;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueViking.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.VikingHero valueViking = ProgressService.Progress.PlayerData.TypeHero.VikingHero;
                    SetAbilityValues(valueViking);
                    //SpecialSkillLevelData.LoadStepCurrent(valueViking.CurrentStepSpecialAttack);
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
