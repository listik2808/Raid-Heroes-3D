using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.BattleTactics;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.StaticData;
using System;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers
{
    public abstract class Soldier : MonoBehaviour
    {
        
        public HeroType TypeSoldier;
        public HeroTypeId HeroTypeId;
        public SpecialAttack SpecialAttack;
        public Specialty Specialty;
        [SerializeField] private Rank _rank;
        [SerializeField] private Power _power;
        [SerializeField] private DataSoldier _dataSoldier;
        [SerializeField] private SpecialSkillLevelData _specialSkillLevelData;
        [SerializeField] private SurvivabilityLevelData _survivabilityLevelData;
        [SerializeField] private MeleeDamageLevelData _meleeDamageLevelData;
        //[SerializeField] private SpeedSkillLevelData _speedSkillLevelData;
        [SerializeField] private SoldiersStatsLevel _soldiersStatsLevel;
        [SerializeField] protected float _stepValueUpgrage;
        [SerializeField] protected float _durationRecoverySpecAttack;
        [SerializeField] protected Sprite _iconSpecAttack;
        [SerializeField] protected AnimationSwitch _animationSwitch;
        [SerializeField] protected AIAgentBase _agent;
        public MonsterTypeId MonsterTypeId;
        protected IPersistenProgressService ProgressService;
        protected float _maxHealth;
        protected float _currentMeleeDamage;
        protected float _currentHealth;
        protected float _currentSpeed;
        protected float _speedValueCurrent;
        protected float _currentMeleeRechargeTimeBattlefield;
        protected float _currentRechargeTimeSpecialSkillBattlefield;
        protected float _timeSpecialSkill;
        protected float _currentValueSpecAttack = 0;
        protected float _underHypnosisTime;

        protected float _newHealth;
        protected float _newSpeed;
        protected float _newMeleeDamage;
        protected float _newSpecialValue;

        private PlayerCell Cell;

        private bool _installedInCell = false;
        protected bool _mineSoldier;
        protected bool _enemySoldier;
        protected bool _unitOpened = false;

        public AIAgentBase Agent =>_agent;
        public float StepValueUpgrade => _stepValueUpgrage;
        public Power Power => _power;
        public SoldiersStatsLevel SoldiersStatsLevel => _soldiersStatsLevel;
        // public SpeedSkillLevelData SpeedSkillLevelData => _speedSkillLevelData;
        public MeleeDamageLevelData MeleeDamageLevelData => _meleeDamageLevelData;
        public SurvivabilityLevelData SurvivabilityLevelData => _survivabilityLevelData;
        public SpecialSkillLevelData SpecialSkillLevelData => _specialSkillLevelData;
        public DataSoldier DataSoldier => _dataSoldier;
        public Rank Rank => _rank;
        public AnimationSwitch AnimationSwitch => _animationSwitch;
        public Sprite IconSpecAttack => _iconSpecAttack;
        public PlayerCell PlayerCell => Cell;
        public float TimeSpecialSkill => _timeSpecialSkill;
        public float DurationRecoverySpecAttack => _durationRecoverySpecAttack;
        public float CurrenValueSpecAttack => _currentValueSpecAttack;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public float CurrentMeleeDamage => _currentMeleeDamage;
        // public float CurrentSpeed => _currentSpeed;
        public float NewHealth => _newHealth;
        // public float NewSpeed => _newSpeed;
        public float NewMeleeDamage => _newMeleeDamage;
        public float NewSpecialDamage => _newSpecialValue;
        public bool InstalledInCell => _installedInCell;
        public bool UnitOpened => _unitOpened;

        public event Action ChangedHp;
        public event Action ChangedDamage;
        public event Action ChangedSpecAttack;
        public event Action ChangedHpBar;

        public abstract void SpecialSkillUpgrade();

        public abstract void BaseData();

        public abstract void SetCharacteristics();

        public abstract void SetNewSkillValueXXX(float cerrentLevel,float currentStep,float maxStep);
        public void SpecialAttackChanges()
        {
            ChangedSpecAttack?.Invoke();
        }

        public void TutorMelldamage(float value)
        {
            _currentMeleeDamage = value;
            _currentValueSpecAttack = value;
        }

        public void SetBaseData()
        {
            //_currentSpeed = DataSoldier.BaseSpeedValue;
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage;
            _currentHealth = DataSoldier.BaseHealthValue;
            ChangedSpecAttack?.Invoke();
            ChangedHp?.Invoke();
            ChangedDamage?.Invoke();
        }

        public void SetBarHp()
        {
            ChangedHpBar?.Invoke();
        }
        public void TakeDamage(float damage)
        {
            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
            }
            else
            {
                _currentHealth = 0;
            }
        }

        public void SetCell(PlayerCell playerCell)
        {
            Cell = playerCell;
        }

        public void SetInstalled() =>
            _installedInCell = true;
        public void DiactivateInstalled() =>
            _installedInCell = false;

        public void OpenHeroCard(bool value)
        {
            _unitOpened = value;
        }

        public float GetMaxHp()
        {
            SoldiersStatsLevel.MaxLevelParam(Rank);
            return GetLevelExpCurve(SoldiersStatsLevel.MaxLevelStatsHero, 10, 10, _currentHealth, _survivabilityLevelData.ValueUpHealth);
        }
        public float GetMaxDamage()
        {
            SoldiersStatsLevel.MaxLevelParam(Rank);
            return GetLevelExpCurve(SoldiersStatsLevel.MaxLevelStatsHero, 10, 10, _dataSoldier.BaseMeleeDamage, _meleeDamageLevelData.ValueUpDamage);
        }

        public void SetSpecAttack(float value)
        {
            var count = GetLevelExpCurve(_soldiersStatsLevel.CurrentLevelSpecialSkill, _specialSkillLevelData.CurrentStepSkill, _specialSkillLevelData.MaxStepValue,
               value , _stepValueUpgrage,true);
            _currentValueSpecAttack = count;
            ChangedSpecAttack?.Invoke();
        }

        public void SetCurrenHealth(float value)
        {
            SoldiersStatsLevel.MaxLevelParam(Rank);
            var count = GetLevelExpCurve(_soldiersStatsLevel.CurrentSurvivabilityLevel, _survivabilityLevelData.CurrentStepSkill, _survivabilityLevelData.MaxStepValue,
                _dataSoldier.BaseHealthValue, value,true);
            _currentHealth = count;
            ChangedHp?.Invoke();
        }

        public void SetMeleeDamage(float value)
        {
            var count = GetLevelExpCurve(_soldiersStatsLevel.CurrentMeleelevel, _meleeDamageLevelData.CurrentStepSkill,_meleeDamageLevelData.MaxStepValue,
                _dataSoldier.BaseMeleeDamage,value,true);
            _currentMeleeDamage = count;
            ChangedDamage?.Invoke();
        }

        public float SetNewValueHealth(float value)
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (_survivabilityLevelData.CurrentStepSkill < _survivabilityLevelData.MaxStepValue)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentSurvivabilityLevel, _survivabilityLevelData.CurrentStepSkill +1, _survivabilityLevelData.MaxStepValue,
                    _dataSoldier.BaseHealthValue, value, true);
                _newHealth = count;
                return _newHealth;
            }
            else if (_soldiersStatsLevel.CurrentSurvivabilityLevel < _soldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentSurvivabilityLevel +1, 0, _survivabilityLevelData.MaxStepValue,
                    _dataSoldier.BaseHealthValue, value, true);
                _newHealth = count;
                return _newHealth;
            }
            return 0;
        }

        public void SetNewHpMultiplier(float currentSurvabilityLevel,float currentStep)
        {
            _newHealth = GetLevelExpCurve(currentSurvabilityLevel, currentStep, _survivabilityLevelData.MaxStepValue,
                    _dataSoldier.BaseHealthValue, SurvivabilityLevelData.ValueUpHealth, true);
        }

        public void SetNewMeleeDamageMultiplier(float currentMeleedamageLevel,float currentStep)
        {
            _newMeleeDamage = GetLevelExpCurve(currentMeleedamageLevel, currentStep, _meleeDamageLevelData.MaxStepValue,
                    _dataSoldier.BaseMeleeDamage, MeleeDamageLevelData.ValueUpDamage, true);
        }

        public float NewSpecAttack(float value)
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (_specialSkillLevelData.CurrentStepSkill < _specialSkillLevelData.MaxStepValue)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentLevelSpecialSkill, _specialSkillLevelData.CurrentStepSkill + 1, _specialSkillLevelData.MaxStepValue,
                value, _stepValueUpgrage, true);
                return count;
            }
            else if (_soldiersStatsLevel.CurrentLevelSpecialSkill < _soldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, _specialSkillLevelData.MaxStepValue,
                value, _stepValueUpgrage, true);
                return count;
            }

            return 0;
        }
        public float NewSpecRangeAttack(float value)
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (_specialSkillLevelData.CurrentStepSkill < _specialSkillLevelData.MaxStepValue)
            {
                count = GetLevelLinear(_soldiersStatsLevel.CurrentLevelSpecialSkill, _specialSkillLevelData.CurrentStepSkill + 1, 10, value, 2 * value, true);
                return count;
            }
            else if (_soldiersStatsLevel.CurrentLevelSpecialSkill < _soldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelLinear(_soldiersStatsLevel.CurrentLevelSpecialSkill + 1, 0, 10,value, 2 * value, true);
                return count;
            }

            return 0;
        }

        public float SetNewValueMeleedamage(float value)
        {
            float count;
            SoldiersStatsLevel.MaxLevelParam(Rank);
            if (_meleeDamageLevelData.CurrentStepSkill < _meleeDamageLevelData.MaxStepValue)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentMeleelevel, _meleeDamageLevelData.CurrentStepSkill +1, _meleeDamageLevelData.MaxStepValue,
                    _dataSoldier.BaseMeleeDamage, value, true);
                _newMeleeDamage = count;
                return _newMeleeDamage;
            }
            else if (_soldiersStatsLevel.CurrentMeleelevel < _soldiersStatsLevel.MaxLevelStatsHero)
            {
                count = GetLevelExpCurve(_soldiersStatsLevel.CurrentMeleelevel + 1, 0, _meleeDamageLevelData.MaxStepValue, _dataSoldier.BaseMeleeDamage, value, true);
                _newMeleeDamage = count;
                return _newMeleeDamage;
            }

            return 0;
        }

        public float GetMaxHpMaxLevel()
        {
            return (_rank.CurrentLevelHero + 1) * 10;
        }

        public void SetNewValueSpeed(float value)
        {
            _newSpeed = _currentSpeed + value;
            _newSpeed = RoundUp(_newSpeed);
        }
        public void SetSpeed(float value)
        {
            _currentSpeed += value;
            _currentSpeed = (float)Math.Round((double)_currentSpeed, 1);
        }

        //public void SetMineSoldier(bool value)
        //{
        //    _mineSoldier = value;
        //}

        //public void SetEnemySoldier(bool value)
        //{
        //    _enemySoldier = value;
        //}

        //public void SetHipnososTime(float volue)
        //{
        //    _underHypnosisTime += volue;
        //}

        //public void ResetHipnosis()
        //{
        //    _underHypnosisTime = 0;
        //    _mineSoldier = false;
        //    _enemySoldier = true;
        //}
        public float GetLevelExpCurve(float level, float step, float maxStep, float c0, float a, bool round = false)
        {
            var result = c0 * (float)Math.Exp(a * GetS(level, step, maxStep));
            if (round)
                return (float)Math.Round(result,2);

            return result;
        }

        public float RoundUp(float value, int coefficient = 1)
        {
            return (float)Math.Round((double)value, coefficient);
        }

        public float GetLevelLinear(float level,float step,float maxStep,float a,float b,bool round = false)
        {
            return GetLinear(a, b, GetS(level, step, maxStep), GetMaxS(), round);
        }

        public float GetLinear(float startValue,float finishValue,float step, float maxStep,bool round)
        {
            var result = startValue + (finishValue - startValue) * step/maxStep;
            if(round)
                return (float)Math.Round(result,2);
            return result;
        }

        public float GetS(float level, float step, float maxStep, float lowCoef = 0.25f)
        {
            return ((level - 1) * (maxStep + 1) + step) * lowCoef;
        }
        public float GetMaxS(float lowCoeff = 0.25f)
        {
            return GetS(60, 10, 10, lowCoeff);
        }
    }
}
