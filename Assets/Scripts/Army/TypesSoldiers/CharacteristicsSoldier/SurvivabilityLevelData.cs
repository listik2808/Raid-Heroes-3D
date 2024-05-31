using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class SurvivabilityLevelData : LevelDataSkill
    {
        [SerializeField] private float _valueUpHealth = 2;

        public float ValueUpHealth => _valueUpHealth;

        public void SkillSurvivabikityUpgrade(Soldier soldier)
        {
            soldier.SetCurrenHealth(ValueUpHealth);
            soldier.SetNewValueHealth(ValueUpHealth);
        }
    }
}
