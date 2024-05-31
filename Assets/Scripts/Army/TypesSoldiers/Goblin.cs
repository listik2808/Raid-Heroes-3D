using Scripts.Data.TypeHeroSoldier;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army.TypesSoldiers
{
    public class Goblin : Soldier
    {
        [SerializeField] private float _baseDamageSpecialAttack;
        [SerializeField] private float _maxBaseDamageSpecialAttack;
        [SerializeField] private float _baseRadius;
        [SerializeField] private float _stepValueUpgrageRadius;

        private float _radiusAction;
        private float _newRadius;

        public float RadiusAction => _radiusAction;
        public float BaseDamageSpecialAttack => _baseDamageSpecialAttack;
        public float MaxBaseDamageSpecialAttack => _maxBaseDamageSpecialAttack;
        public float NewRadius => _newRadius;

        public override void SpecialSkillUpgrade()
        {
            _currentValueSpecAttack = _currentValueSpecAttack + _stepValueUpgrage;
            _radiusAction = _radiusAction + _stepValueUpgrageRadius;
            _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
            _newRadius = _radiusAction + _stepValueUpgrageRadius;
            _newRadius = RoundUp(_newRadius);
        }

        public override void BaseData()
        {
            SetBaseData();
            if (_currentValueSpecAttack == 0)
            {
                _currentValueSpecAttack = _baseDamageSpecialAttack;
                _radiusAction = _baseRadius;
               // _newSpeed = _currentSpeed + SpeedSkillLevelData.ValueUpSpeed;
                _newHealth = _currentHealth + SurvivabilityLevelData.ValueUpHealth;
                _newMeleeDamage = _currentMeleeDamage + MeleeDamageLevelData.ValueUpDamage;
                _newSpecialValue = _currentValueSpecAttack + _stepValueUpgrage;
                _newRadius = _radiusAction + _stepValueUpgrageRadius;
            }
        }

        //private void Awake()
        //{
        //    var key = gameObject.name.Substring(0, gameObject.name.Length - "Variant(Clone)".Length - 1);
        //    if (PlayerData.Heroes.ContainsKey(key))
        //        Data = PlayerData.Heroes[key];
        //    else if (PlayerData.Enemies.ContainsKey(key))
        //        Data = PlayerData.Enemies[key];

        //    if (Data == null)
        //        return;

        //    SetAbilityValues(Data);
        //    SpecialSkillLevelData.LoadStepCurrent(Data.CurrentStepSpecialAttack);
        //}

        public void SetAbilityValues(DataLevelSkill valueGoblin)
        {
            _currentValueSpecAttack = _baseDamageSpecialAttack + (_stepValueUpgrage * valueGoblin.GetCurrentSpecialSkill());
            _radiusAction = _baseRadius + (_stepValueUpgrageRadius * valueGoblin.GetCurrentSpecialSkill());
            _currentHealth = DataSoldier.BaseHealthValue + (SurvivabilityLevelData.ValueUpHealth * valueGoblin.GetCurrentHealth());
            _currentMeleeDamage = DataSoldier.BaseMeleeDamage + (MeleeDamageLevelData.ValueUpDamage * valueGoblin.GetCurrentMeleeSkill());
           // _currentSpeed = DataSoldier.BaseSpeedValue + (SpeedSkillLevelData.ValueUpSpeed * valueGoblin.GetCurrentMobilitySkill());
        }

        public override void SetCharacteristics()
        {
            throw new System.NotImplementedException();
        }

        public override void SetNewSkillValueXXX(float cerrentLevel, float currentStep, float maxStep)
        {
            throw new System.NotImplementedException();
        }
    }
}
