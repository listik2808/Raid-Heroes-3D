using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Poisoner : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _baseRadius;
        private float _maxBaseDamageSpecialAttack;
        private float _maxBaseRange;
        private float _radiusAction;
        private float _newRadius;

        public float RadiusAction => _radiusAction;
        public float MaxRadius => _maxBaseRange;
        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float NewRadius => _newRadius;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _radiusAction = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRadius, 2 * _baseRadius, true);
            DataSoldier.SetRange(_radiusAction);
            _newRadius = NewSpecRangeAttack(_baseRadius);
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _radiusAction = _baseRadius;
                DataSoldier.SetRange(_radiusAction);
                _newRadius = NewSpecRangeAttack(_baseRadius);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valuePoisoner)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valuePoisoner.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valuePoisoner.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valuePoisoner.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valuePoisoner.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valuePoisoner.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valuePoisoner.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            _radiusAction = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
                _baseRadius, 2 * _baseRadius, true);
            DataSoldier.SetRange(_radiusAction);
            _newRadius = NewSpecRangeAttack(_baseRadius);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valuePoisoner.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.PoisonerHero valuePoisoner = ProgressService.Progress.PlayerData.TypeHero.PoisonerHero;
                    SetAbilityValues(valuePoisoner);
                    //SpecialSkillLevelData.LoadStepCurrent(valuePoisoner.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        public void SetMaxRange()
        {
            _maxBaseRange = GetLevelLinear(60, 10, SpecialSkillLevelData.MaxStepValue, _baseRadius, 2 * _baseRadius, true);
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
            _newRadius = GetLevelLinear(cerrentLevel, currentStep, 10, _baseRadius, 2 * _baseRadius, true);
        }
    }
}
