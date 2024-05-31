using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.SpecificationsUI;
using Scripts.StaticData;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenSkill : MonoBehaviour
    {
        public const string ValueCooldown = "Время востановления";
        public const string TextSpecialSkill = "Особый навык";
        public const string BaseSkill = "Базовый навык";
        public const string BaseValueText = "Базовое значение";
        public const string TextMaxValue = "Макс. значение";
        public const string TextArtifact = "(без учета артефактов)";
        public const string TextSurvivability = "Живучесть";
        public const string TextMeleeDamage = "Урон в Секунду";

        [SerializeField] private TMP_Text _textSkillSpec;
        [SerializeField] private Image _imageSkill;
        [SerializeField] private TMP_Text _textSkillMassage;
        [SerializeField] private TMP_Text _baseDamadeValue;
        [SerializeField] private TMP_Text _baseMaxDamageValue;
        [SerializeField] private TMP_Text _secondBasicValue;
        [SerializeField] private TMP_Text _secondMaxValue;
        [SerializeField] private TMP_Text _textCooldown;
        [SerializeField] private TMP_Text _textCooldownMeleeDamage;
        [SerializeField] private BookmarkButton _close;
        [SerializeField] private BookmarkButton _left;
        [SerializeField] private BookmarkButton _right;
        [SerializeField] private GameObject _container;
        [SerializeField] private BookmarkButton _buttonSurwabilityIcon;
        [SerializeField] private BookmarkButton _buttonSurwabilityBackground;
        [SerializeField] private BookmarkButton _buttonMeleeDamageIcon;
        [SerializeField] private BookmarkButton _buttonMeleeDamageBackground;
        [SerializeField] private BookmarkButton _buttonSpecialSkillIcon;
        [SerializeField] private BookmarkButton _buttonSpecialSkillBackground;

        private string _text;
        private string _valueDamage;
        private float _baseMaxHp;
        private float _baseMaxDamaeg;

        public GameObject Container => _container;
        public BookmarkButton ButtonSurwabilityIcon => _buttonSurwabilityIcon;
        public BookmarkButton ButtonSurwabilityBackground => _buttonSurwabilityBackground;
        public BookmarkButton ButtonMeleeDamageIcon => _buttonMeleeDamageIcon;
        public BookmarkButton ButtonMeleeDamageBackground => _buttonMeleeDamageBackground;
        public BookmarkButton ButtonSpecialSkillIcon => _buttonSpecialSkillIcon;
        public BookmarkButton ButtonSpecialSkillBackground => _buttonSpecialSkillBackground;
        public BookmarkButton ButtonLeft => _left;
        public BookmarkButton ButtonRight => _right;

        private void OnEnable()
        {
            _close.Button.onClick.AddListener(CloseScreen);
            _close.ButtonOnClic += CloseScreen;
        }

        private void OnDisable()
        {
            _close.Button?.onClick.RemoveListener(CloseScreen);
            _close.ButtonOnClic -= CloseScreen;
        }

        public void SetSurwability(Sprite icon,Soldier soldier)
        {
            _imageSkill.sprite = icon;
            _textSkillSpec.text = TextSurvivability;
            _textSkillMassage.text = BaseSkill;
            _baseDamadeValue.text = BaseValueText +" "+soldier.DataSoldier.BaseHealthValue;
            _baseMaxHp = soldier.GetMaxHp();
            _valueDamage = AbbreviationsNumbers.ShortNumber(_baseMaxHp);
            _baseMaxDamageValue.text = TextMaxValue +" "+_valueDamage+"\n"+TextArtifact;
            _textCooldown.gameObject.SetActive(false);
            _secondBasicValue.gameObject.SetActive(false);
            _secondMaxValue.gameObject.SetActive(false);
            _textCooldownMeleeDamage.gameObject.SetActive(false);
        }

        public void SetMeleeDamage(Sprite icon,Soldier soldier,string textSec)
        {
            _imageSkill.sprite = icon;
            _textSkillSpec.text = TextMeleeDamage;
            _textSkillMassage.text = BaseSkill;
            _baseDamadeValue.text = BaseValueText + " " + soldier.DataSoldier.BaseMeleeDamage;
            _baseMaxDamaeg = soldier.GetMaxDamage();
            _valueDamage = AbbreviationsNumbers.ShortNumber(_baseMaxDamaeg);
            _baseMaxDamageValue.text = TextMaxValue + " " + _valueDamage + "\n" + TextArtifact;
            _textCooldownMeleeDamage.text = ValueCooldown+" "+soldier.DataSoldier.DurationRecoveryMeleeDamage + textSec;
            _secondBasicValue.gameObject.SetActive(false);
            _secondMaxValue.gameObject.SetActive(false);
            _textCooldown.gameObject.SetActive(false);
            _textCooldownMeleeDamage.gameObject.SetActive(true);
        }

        public void SetSpecialSkill(Soldier soldier,float baseDamage,string baseDamageText,float baseMaxDamage,string maxDamageText,float valueCooldown,string textSec,
            string valueSecond = null,string baseTextSecond = null,string secondValueMax = null,string maxTextValue = null)
        {
            _textCooldown.gameObject.SetActive(true);
            _textCooldownMeleeDamage.gameObject.SetActive(false);
            _imageSkill.sprite = soldier.IconSpecAttack;
            _text = soldier.SpecialAttack.ToString();
            _text = Lean.Localization.LeanLocalization.GetTranslationText(_text);
            _textSkillSpec.text = _text;
            _textSkillMassage.text = TextSpecialSkill;
            _valueDamage = AbbreviationsNumbers.ShortNumber(baseMaxDamage);
            _baseDamadeValue.text = baseDamageText +" "+baseDamage;
            _baseMaxDamageValue.text = maxDamageText + " "+_valueDamage + "\n"+ TextArtifact;
           
            SetPositionText(soldier,valueCooldown,textSec ,valueSecond, baseTextSecond, secondValueMax, maxTextValue);
        }

        private void CloseScreen()
        {
            _container.gameObject.SetActive(false);
        }

        private void SetPositionText(Soldier soldier,float valueCooldown,string textSec ,string valueSecond = null,string baseTextSecond = null,string secondValueMax = null,string maxTextValue = null)
        {
            if (valueSecond != null)
            {
                _secondBasicValue.gameObject.SetActive(true);
                if(soldier.HeroTypeId == HeroTypeId.Berserk)
                {
                    _secondBasicValue.text = baseTextSecond + " " + valueSecond +" "+ textSec;
                }
                else 
                {
                    _secondBasicValue.text = baseTextSecond + " " + valueSecond;
                }
            }
            else
            {
                _secondBasicValue.gameObject.SetActive(false);
            }
            if (secondValueMax != null)
            {
                _secondMaxValue.gameObject.SetActive(true);
                if (soldier.HeroTypeId == HeroTypeId.Berserk)
                {
                    _secondMaxValue.text = maxTextValue + " " + secondValueMax +" "+ textSec;
                }
                else
                {
                    _secondMaxValue.text = maxTextValue + " " + secondValueMax;
                }
                    
            }
            else
            {
                _secondMaxValue.gameObject.SetActive(false);
            }
            if (_secondBasicValue.gameObject.activeInHierarchy == false && _secondMaxValue.gameObject.activeInHierarchy == false)
            {
                _textCooldownMeleeDamage.gameObject.SetActive(true);
                _textCooldown.gameObject.SetActive(false);
                _textCooldownMeleeDamage.text = ValueCooldown + " " + valueCooldown + textSec;
            }
            else
            {
                _textCooldownMeleeDamage.gameObject.SetActive(false);
                _textCooldown.gameObject.SetActive(true);
                _textCooldown.text = ValueCooldown + " " + valueCooldown + textSec;
            }
        }
    }
}
