using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Breaking : Soldier
    {

        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _maxBaseDamageSpecialAttack;
        [SerializeField] private float _durationSpecialSkill;

        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float DurationSpecialSkill => _durationSpecialSkill;

        public override void SpecialSkillUpgrade()
        {
            _currentValueSpecAttack += _stepValueUpgrage;
            //_newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueBreakingSoldierHero)
        {
            _currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueBreakingSoldierHero.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueBreakingSoldierHero.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueBreakingSoldierHero.GetCurrentMeleeSkill());
            //DataSoldier.EnemyHealth.SetHp();
            // _currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueBreakingSoldierHero.GetCurrentMobilitySkill());
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
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
                    Data.TypeHeroSoldier.BreakingSoldierHero valueBreakingSoldierHero = ProgressService.Progress.PlayerData.TypeHero.BreakingSoldierHero;
                    SetAbilityValues(valueBreakingSoldierHero);
                    SpecialSkillLevelData.LoadStepCurrent(valueBreakingSoldierHero.CurrentStepSpecialAttack);
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
