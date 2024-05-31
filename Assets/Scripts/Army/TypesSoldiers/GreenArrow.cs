using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class GreenArrow : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _maxBaseDamageSpecialAttack;
        [SerializeField] private float _baseRange;
        private float _maxBaseRange;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float BaseRange => _baseRange;
        public float MaxBaseRange => _maxBaseRange;

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
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            }
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueGreenArrow)
        {
            _currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueGreenArrow.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueGreenArrow.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueGreenArrow.GetCurrentMeleeSkill());
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueGreenArrow.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.GreenArrowHero valueGreenArrow = ProgressService.Progress.PlayerData.TypeHero.GreenArrowHero;
                    SetAbilityValues(valueGreenArrow);
                    //SpecialSkillLevelData.LoadStepCurrent(valueGreenArrow.CurrentStepSpecialAttack);
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
