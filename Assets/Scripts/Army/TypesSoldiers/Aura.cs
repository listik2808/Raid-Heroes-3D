using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Aura : Soldier
    {
        [SerializeField] private float _baseValue;
        [SerializeField] private float _maxBaseValue;
        [SerializeField] private float _basePercentageIncomingDamageAbsorption;
        [SerializeField] private float _stepUpgradePercentage;
        [SerializeField] private float _baseDurationAction;

        private float _currentPercentageIncomingDamageAbsorption;
        public float BaseValue => _baseValue;
        public float MaxBaseValue => _maxBaseValue;

        public override void SpecialSkillUpgrade()
        {
            _currentValueSpecAttack = _currentValueSpecAttack + _stepValueUpgrage;
            _currentPercentageIncomingDamageAbsorption = _currentPercentageIncomingDamageAbsorption + _stepUpgradePercentage;
            //_newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseValue;
                _currentPercentageIncomingDamageAbsorption = _basePercentageIncomingDamageAbsorption;
                _currentPercentageIncomingDamageAbsorption = RoundUp(_currentPercentageIncomingDamageAbsorption);
                //_newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
            }
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueAuraSoldierHero)
        {
            _currentValueSpecAttack = _baseValue + (_stepValueUpgrage * valueAuraSoldierHero.GetCurrentSpecialSkill());
            _currentPercentageIncomingDamageAbsorption = _basePercentageIncomingDamageAbsorption + (_stepUpgradePercentage * valueAuraSoldierHero.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueAuraSoldierHero.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueAuraSoldierHero.GetCurrentMeleeSkill());
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueAuraSoldierHero.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.AuraSoldierHero valueAuraSoldierHero = ProgressService.Progress.PlayerData.TypeHero.AuraSoldierHero;
                    SetAbilityValues(valueAuraSoldierHero);
                    SpecialSkillLevelData.LoadStepCurrent(valueAuraSoldierHero.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
        }
    }
}
