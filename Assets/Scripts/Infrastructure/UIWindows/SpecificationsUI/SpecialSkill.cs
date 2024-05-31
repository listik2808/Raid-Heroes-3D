using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public class SpecialSkill : LevelSkillUI
    {
        [SerializeField] private TMP_Text _textTypeSpecAttack;
        [SerializeField] private TMP_Text _impact1;
        [SerializeField] private TMP_Text _impact2;
        private int _rechargeDuration;
        private string _nameAttack;

        public TMP_Text TextTypeSpeceAttack => _textTypeSpecAttack;
        public TMP_Text Impact1 => _impact1;
        public TMP_Text Impact2 => _impact2;

        public void SetTypeAttack(StaticData.SpecialAttack monsterTypeId)
        {
            //Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Russian");
            //_nameAttack = monsterTypeId.ToString();
            //_nameAttack = Lean.Localization.LeanLocalization.GetTranslationText(_nameAttack);
            _nameAttack = CustomRuLocalization.GetSpecAttack((int)monsterTypeId);
            _textTypeSpecAttack.text = _nameAttack;
        }

        public void SetFirstSkillValue(string firstSkillValueText, float currenValueSpecAttack, float newSpecialDamage ,string singl =null,string proc = null)
        {
            string value = AbbreviationsNumbers.ShortNumber(currenValueSpecAttack);
            string value2 = AbbreviationsNumbers.ShortNumber(newSpecialDamage);
            _impact1.text = $"{firstSkillValueText}  {value} {proc}<color=green>{singl} {value2}</color>";
        }

        public void SetFirstSkillValue(string firstSkillValueText, float currenValueSpecAttack ,string proc = null)
        {
            string result = AbbreviationsNumbers.ShortNumber(currenValueSpecAttack); 
            _impact1.text = $"{firstSkillValueText}  {result} {proc}";
        }

        public void SetSecondSkillValue(string secondSkillValueText, float durationRecoverySpecAttack, float newValueRange, string singl, string textSec = null)
        {
            string result1 = AbbreviationsNumbers.ShortNumber(durationRecoverySpecAttack);
            string result2 = AbbreviationsNumbers.ShortNumber(newValueRange);
            _impact2.text = $"{secondSkillValueText}  {result1} <color=green>{singl} {result2}</color> {textSec}";
        }

        public void SetSecondSkillValue(string secondSkillValueText, float durationRecoverySpecAttack, string textSec = null)
        {
            string result3 = AbbreviationsNumbers.ShortNumber(durationRecoverySpecAttack);
            _impact2.text = $"{secondSkillValueText} {result3} {textSec}";
        }

        public void SetSpecialSkillIcon(Sprite iconSpecAttack)
        {
            _imageSkill.sprite = iconSpecAttack;
        }
    }
}
