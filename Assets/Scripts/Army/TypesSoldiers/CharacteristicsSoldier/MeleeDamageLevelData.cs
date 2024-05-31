using System;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class MeleeDamageLevelData : LevelDataSkill
    {
       [SerializeField] private float _valueUpDamage = 1;

        public float ValueUpDamage => _valueUpDamage;

        public void SkillMeleeDamageUpgrade(Soldier soldier)
        {
            soldier.SetMeleeDamage(ValueUpDamage);
            soldier.SetNewValueMeleedamage(ValueUpDamage);
        }
    }
}
