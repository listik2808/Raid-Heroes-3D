using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Assassin : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        private float _maxBaseDamageSpecialAttack;
        
        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;

        public override void BaseData()
        {
            SetBaseData();
            if(_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            }
        }
        public override void SpecialSkillUpgrade()
        {
            SetSpecAttack(_baseDamageSpecialAttack);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
           // _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
        }

        private void Awake()
        {
            ProgressService = AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueAssassin)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueAssassin.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueAssassin.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueAssassin.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueAssassin.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueAssassin.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueAssassin.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SetSpecAttack(_baseDamageSpecialAttack);
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            _newSpecialValue = NewSpecAttack(_baseDamageSpecialAttack);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueAssassin.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.AssassinHero valueAssassin = ProgressService.Progress.PlayerData.TypeHero.AssassinHero;
                    SetAbilityValues(valueAssassin);
                    //SpecialSkillLevelData.LoadStepCurrent(valueAssassin.CurrentStepSpecialAttack);
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
