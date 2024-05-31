using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Princess : Soldier
    {
        [SerializeField] private float _baseSpecialAttack;
        [SerializeField] private float _maxBaseSpecialAttack;

        public float BaseSpecialAttack => _baseSpecialAttack;
        public float MaxBaseSpecialAttack => _maxBaseSpecialAttack;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_stepValueUpgrage);
            _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseSpecialAttack;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
            }
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }
        public void SetAbilityValues(DataLevelSkill valuePrincess)
        {
            _currentValueSpecAttack = _baseSpecialAttack + (_stepValueUpgrage * valuePrincess.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valuePrincess.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage +(MeleeDamageLevelData.ValueUpDamage * valuePrincess.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valuePrincess.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.PrincessHero valuePrincess = ProgressService.Progress.PlayerData.TypeHero.PrincessHero;
                    SetAbilityValues(valuePrincess);
                    SpecialSkillLevelData.LoadStepCurrent(valuePrincess.CurrentStepSpecialAttack);
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
