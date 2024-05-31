using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public class Survivability : LevelSkillUI
    {
        [SerializeField] private TMP_Text _textMaxHealth;

        public void FillSurvivabilityIndicators(float currentHeath, float newHealth, string sing = null)
        {
            string healthText = AbbreviationsNumbers.ShortNumber(currentHeath);
            string healtText2 = AbbreviationsNumbers.ShortNumber(newHealth);
            _textMaxHealth.text = $"{healthText} <color=green>{sing} {healtText2}</color>";
        }

        public void FillSurvivabilityIndicators(float currentHeath)
        {
            string healthText3 = AbbreviationsNumbers.ShortNumber(currentHeath);
            _textMaxHealth.text = $"{healthText3}";
        }
    }
}
