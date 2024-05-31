using Scripts.Data.TypeHeroSoldier;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Army.TypesSoldiers
{
    public class King : Soldier
    {
        [SerializeField] private float _baseMultiplier;
        [SerializeField] private float _minDurationAction;
        //[SerializeField] private float _stepDuarationAction;
        private float _maxBaseMultiplier;
        private float _maxDurationAction;

        private float _currentDurationAction;
        private float _newDuarationAction;

        public float BaseMultiplier => _baseMultiplier;
        public float MaxBaseMultiplier => _maxBaseMultiplier;
        public float MinDurationAction => _minDurationAction;
        public float MaxDurationAction => _maxDurationAction;
        public float NewDurationAction => _newDuarationAction;

        public float CurrentDuationAction => _currentDurationAction;

        public override void SpecialSkillUpgrade()
        {
            SpecAttaclMotivation();
            _newSpecialValue = NewSpecAttackMotivation();
            _currentDurationAction = GetMotimvationTime();
            _newDuarationAction = NewMotivationDurationTime();
            _timeSpecialSkill = _currentDurationAction;
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                //_newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _currentValueSpecAttack = _baseMultiplier;
                _newSpecialValue = NewSpecAttackMotivation();
                _currentDurationAction = GetMotimvationTime();
                _newDuarationAction = NewMotivationDurationTime();
                SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
                SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
                _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
                _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            }
            _timeSpecialSkill = _currentDurationAction;
        }

        private void Awake()
        {
            ProgressService =AllServices.Container.Single<IPersistenProgressService>();
            SetCharacteristics();
        }

        public void SetAbilityValues(DataLevelSkill valueKing)
        {
            _maxBaseMultiplier = GetMaxMotivationMultiplier();
            _maxDurationAction = GetMaxMotivationTime();
            SpecialSkillLevelData.SetStepSkill((int)valueKing.CurrentStepSpecialAttack);
            SurvivabilityLevelData.SetStepSkill((int)valueKing.CurrentStepSurvivability);
            MeleeDamageLevelData.SetStepSkill((int)valueKing.CurrentStepMelee);
            SoldiersStatsLevel.SetSpecAttac((int)valueKing.CurrenSpecialAttackLevel);
            SoldiersStatsLevel.SetHealthLevel((int)valueKing.CurrenSurvivabilityLevel);
            SoldiersStatsLevel.SetMeleeLevel((int)valueKing.CurrenMeleeLevel);
            SetMeleeDamage(MeleeDamageLevelData.ValueUpDamage);
            SetCurrenHealth(SurvivabilityLevelData.ValueUpHealth);
            SpecAttaclMotivation();
            _currentDurationAction = GetMotimvationTime();
            _newDuarationAction = NewMotivationDurationTime();
            _newMeleeDamage = SetNewValueMeleedamage(MeleeDamageLevelData.ValueUpDamage);
            _newHealth = SetNewValueHealth(SurvivabilityLevelData.ValueUpHealth);
            SetBarHp();
            //DataSoldier.EnemyHealth.SetHp();
            //_currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueKing.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            if (SceneManager.GetActiveScene().name != "SandBox")
            {
                if (TypeSoldier == HeroType.Hero)
                {
                    Data.TypeHeroSoldier.KingHero valueKing = ProgressService.Progress.PlayerData.TypeHero.KingHero;
                    SetAbilityValues(valueKing);
                    //SpecialSkillLevelData.LoadStepCurrent(valueKing.CurrentStepSpecialAttack);
                }
                else
                {
                    BaseData();
                }
            }
        }

        private void SpecAttaclMotivation()
        {
            var count = 1 + GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue,
               _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
            _currentValueSpecAttack = count;
            SpecialAttackChanges();
        }

        private float GetMaxMotivationMultiplier()
        {
            return 1 + GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue, _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
        }

        private float GetMotimvationTime()
        {
            return GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill, SpecialSkillLevelData.MaxStepValue
                , _minDurationAction, 2 * _minDurationAction, true);
        }

        private float GetMaxMotivationTime()
        {
            return GetLevelLinear(60, SpecialSkillLevelData.MaxStepValue, SpecialSkillLevelData.MaxStepValue, _minDurationAction, 2 * _minDurationAction);
        }

        private float NewSpecAttackMotivation()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count = 1 + GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
                return count;
            }
            return 0;
        }

        private float NewMotivationDurationTime()
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (SpecialSkillLevelData.CurrentStepSkill < SpecialSkillLevelData.MaxStepValue)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill, SpecialSkillLevelData.CurrentStepSkill + 1, SpecialSkillLevelData.MaxStepValue,
                _minDurationAction, 2 * _minDurationAction, true);
                return count;
            }
            else if (SoldiersStatsLevel.CurrentLevelSpecialSkill < SoldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(SoldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, SpecialSkillLevelData.MaxStepValue,
                _minDurationAction, 2 * _minDurationAction, true);
                return count;
            }
            return 0;
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            _newSpecialValue = 1 + GetLevelLinear(cerrentLevel, currentStep, maxStep,
                _baseMultiplier - 1, 2 * (_baseMultiplier - 1), true);
            _newDuarationAction = GetLevelLinear(cerrentLevel, currentStep, maxStep,
                _minDurationAction, 2 * _minDurationAction, true);
        }
    }
}
