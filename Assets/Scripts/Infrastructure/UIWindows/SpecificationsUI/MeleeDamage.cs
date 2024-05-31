using Scripts.StaticData;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public class MeleeDamage : LevelSkillUI
    {
        [SerializeField] private TMP_Text _textMeleeDamage;

        public void FullMeleeDamageIndicator(float currentMeleeDamage, float newMeleeDamage,string sing = null)
        {
            string textDamage = AbbreviationsNumbers.ShortNumber(currentMeleeDamage);
            string textDamage2 = AbbreviationsNumbers.ShortNumber(newMeleeDamage);
            _textMeleeDamage.text = $"{textDamage} <color=green>{sing} {textDamage2}</color>";
        }

        public void FullMeleeDamageIndicator(float currentMeleeDamage)
        {
            string textdamage3 = AbbreviationsNumbers.ShortNumber(currentMeleeDamage);
            _textMeleeDamage.text = $"{textdamage3}";
        }
    }
}
