using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class SpeedSkillLevelData : LevelDataSkill
    {
        [SerializeField] private float _valueUpSpeed;

        public float ValueUpSpeed => _valueUpSpeed;

        public void SkillSpeedUpgrade(Soldier soldier)
        {
            soldier.SetSpeed(ValueUpSpeed);
            soldier.SetNewValueSpeed(ValueUpSpeed);
        }
    }
}
