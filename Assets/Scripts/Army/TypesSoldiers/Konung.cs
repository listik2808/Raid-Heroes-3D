using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Konung : Soldier
    {
        [SerializeField] private float _baseMultiplier;
        [SerializeField] private float _maxBaseMultiplier;
        [SerializeField] private float _baseDurationAction;
        [SerializeField] private float _maxDurationAction;
        [SerializeField] private float _stepDurationAction;

        private float _currenetDuarationAction;
        private float _newDurationAction;

        public float BaseMultiplier => _baseMultiplier;
        public float MaxBaseMultiplier => _maxBaseMultiplier;
        public float BaseDurationAction => _baseDurationAction;
        public float MaxDurationAction => _maxDurationAction;
        public float CurrenDurationAction => _currenetDuarationAction;
        public float NewDurationAction => _newDurationAction;

        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_stepValueUpgrage);
            _currentValueSpecAttack = RoundUp(_currentValueSpecAttack);
            _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            _newSpecialValue = RoundUp(_newSpecialValue);
            _currenetDuarationAction = _currenetDuarationAction + _stepDurationAction;
            _currenetDuarationAction = RoundUp(_currenetDuarationAction);
            _newDurationAction = _currenetDuarationAction + _stepDurationAction;
            _newDurationAction = RoundUp(_newDurationAction);
            _timeSpecialSkill = _currenetDuarationAction;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseMultiplier;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                _currenetDuarationAction = _baseDurationAction;
                _newDurationAction = _currenetDuarationAction + _stepDurationAction;
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
            }
            _timeSpecialSkill = _currenetDuarationAction;
            _timeSpecialSkill = RoundUp(_timeSpecialSkill);
        }

        public void SetAbilityValues(DataLevelSkill valueKonung)
        {
            _currentValueSpecAttack = _baseMultiplier + (_stepValueUpgrage * valueKonung.GetCurrentSpecialSkill());
            _currenetDuarationAction = _baseDurationAction + (_stepDurationAction * valueKonung.GetCurrentSpecialSkill());
            _currenetDuarationAction = RoundUp(_currenetDuarationAction);
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueKonung.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueKonung.GetCurrentMeleeSkill());
            DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueKonung.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.KonungHero valueBarbarian = ProgressService.Progress.PlayerData.TypeHero.KonungHero;
                    SetAbilityValues(valueBarbarian);
                    SpecialSkillLevelData.LoadStepCurrent(valueBarbarian.CurrentStepSpecialAttack);
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
