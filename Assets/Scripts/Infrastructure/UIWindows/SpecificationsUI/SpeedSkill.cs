using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public class SpeedSkill : LevelSkillUI
    {
        [SerializeField] private TMP_Text _textSpeed;

        public void SetSkillIndicatorsSpeed(float currentSpeed, float newSpeed,string sing = null)
        {
            _textSpeed.text = $"{currentSpeed} <color=green>{sing} {newSpeed}</color>";
        }

        public void SetSkillIndicatorsSpeed(float currentSpeed)
        {
            _textSpeed.text = $"{currentSpeed}";
        }
    }
}
