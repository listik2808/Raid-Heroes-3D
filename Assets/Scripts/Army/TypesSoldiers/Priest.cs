using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Priest : Soldier
    {
        [SerializeField] private float _baseValueDamageSkeleta;
        [SerializeField] private float _currentHealthSkeleta;
        [SerializeField] private float _stepValuerHealthSkeleta;
        [SerializeField] private float _maxHpSkelet;
        [SerializeField] private float _maxDamageSkelet;
        [SerializeField] private float _baseHealthSkelet;
        private float _newHealthSkelata;

        public float BaseValueDamageSkeleta => _baseValueDamageSkeleta;
        public float CurrentHealthSkeleta => _currentHealthSkeleta;
        public float MaxHpSkelet => _maxHpSkelet;
        public float MaxDamageSkelet => _maxDamageSkelet;
        public float NewHealthSkeleta => _newHealthSkelata;
        public float BaseHealthSkelet => _baseHealthSkelet;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_stepValueUpgrage);
            _currentHealthSkeleta = _currentHealthSkeleta + _stepValuerHealthSkeleta;
            _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            _newHealthSkelata = _currentHealthSkeleta + _stepValuerHealthSkeleta;
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseValueDamageSkeleta;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                _currentHealthSkeleta = _currentHealthSkeleta + _stepValuerHealthSkeleta;
                _newHealthSkelata = _currentHealthSkeleta + _stepValuerHealthSkeleta;
               // _newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
            }
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valuePriest)
        {
            _currentValueSpecAttack = _baseValueDamageSkeleta + (_stepValueUpgrage * valuePriest.GetCurrentSpecialSkill());
            _currentHealthSkeleta = _currentHealthSkeleta + (_stepValuerHealthSkeleta * valuePriest.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valuePriest.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valuePriest.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valuePriest.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.PriestHero valuePriest = ProgressService.Progress.PlayerData.TypeHero.PriestHero;
                    SetAbilityValues(valuePriest);
                    SpecialSkillLevelData.LoadStepCurrent(valuePriest.CurrentStepSpecialAttack);
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
