using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class Knight : Soldier
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

        public void SetAbilityValues(DataLevelSkill valueKnight)
        {
            SetMaxSpecAttack();
            SpecialSkillLevelData.SetStepSkill((int)valueKnight.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueKnight.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueKnight.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueKnight.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueKnight.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueKnight.CurrenMeleeLevel);
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
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueKnight.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.KnightHero valueKnight = ProgressService.Progress.PlayerData.TypeHero.KnightHero;
                    SetAbilityValues(valueKnight);
                    //SpecialSkillLevelData.LoadStepCurrent(valueKnight.CurrentStepSpecialAttack);
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
            _newSpecialValue = GetLevelExpCurve(cerrentLevel, currentStep, maxStep,_baseDamageSpecialAttack, _stepValueUpgrage, true);
        }
    }
}
