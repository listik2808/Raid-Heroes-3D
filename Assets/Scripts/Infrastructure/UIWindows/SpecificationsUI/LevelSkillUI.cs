using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public abstract class LevelSkillUI : MonoBehaviour
    {
        public const string TextLevel = "Уровень";
        public const string TextMaxLevelSkill = "Повысь героя, чтобы улучшить навык";
        public const string TextInpruvment = "УЛУЧШИТЬ";

        [SerializeField] protected Image _imageSkill;
        [SerializeField] protected BookmarkButton _improvementButton;
        [SerializeField] protected BookmarkButton _buttonEvolution;
        [SerializeField] protected TMP_Text _textPriceImprovement;
        [SerializeField] protected TMP_Text _textPriceEvolution;
        [SerializeField] protected TMP_Text _textLevelSkill;
        [SerializeField] protected Slider _sliderSkill;
        [SerializeField] protected Sprite _spriteDiamond;
        [SerializeField] protected Sprite _spriteCoin;
        [SerializeField] protected Image _imageEvolutionIcon;
        [SerializeField] protected TMP_Text _textLevelMax;
        [SerializeField] private TMP_Text _textButtonImpruvment;
        [SerializeField] private TMP_Text _textButtonEvolution;
        [SerializeField] protected Image InfoImpruvmentSpecAttack;
        [SerializeField] protected TMP_Text InfoTextImpruvmentSpecAttack;
        [SerializeField] protected Image InfoEvolutionSpecAttack;
        [SerializeField] protected TMP_Text InfoTextEvolutionSpecAttack;
        private float _currentDiamondPrice = 0;
        private float _currentCoinPrice = 0;

        public float CurrentDiamondPrice => _currentDiamondPrice;
        public float CurrentCoinPrice => _currentCoinPrice;
        public BookmarkButton ImprovementButton => _improvementButton;
        public BookmarkButton ButtonEvolution => _buttonEvolution;
        public Slider SliderSkill => _sliderSkill;
        public Image IconSkill => _imageSkill;

        public void ActivateMarkerCoinImpruvment(int value)
        {
            InfoImpruvmentSpecAttack.gameObject.SetActive(true);
            InfoTextImpruvmentSpecAttack.text = value.ToString();
        }

        public void DiactivateMarkerCoinImpruvment()
        {
            InfoImpruvmentSpecAttack.gameObject.SetActive(false);
        }

        public void ActivateMarkerDiamondEvolution(int value)
        {
            InfoEvolutionSpecAttack.gameObject.SetActive(true);
            InfoTextEvolutionSpecAttack.text = value.ToString();
        }

        public void DiactivateMarkerDiamondEvolution()
        {
            InfoEvolutionSpecAttack.gameObject.SetActive(false);
        }

        public void ActivateText()
        {
            _textLevelMax.text = TextMaxLevelSkill;
            _textLevelMax.gameObject.SetActive(true);
        }

        public void DeactivateText()
        {
            _textLevelMax.gameObject.SetActive(false);
        }

        public void SetValueLevelEvolution(float value)
        {
            _textLevelSkill.text = TextLevel + value.ToString();
        }

        public void SetPriceCoins( float value)
        {
            string result = AbbreviationsNumbers.ShortNumber(value);
            _textPriceImprovement.text = result;
            _currentCoinPrice = value;
        }

        public void SetPriceDiamonds( float value)
        {
            _textPriceEvolution.text = value.ToString();
            _currentDiamondPrice = value;
        }

        public void SetIconEvolutionCoin()
        {
            _imageEvolutionIcon.sprite = _spriteCoin;
        }

        public void SetIconEvolutionDiamond()
        {
            _imageEvolutionIcon.sprite = _spriteDiamond;
        }

        public void SetMultiplierTextUpCountImpruvment(int value)
        {
            if(value > 1)
                _textButtonImpruvment.text = TextInpruvment + " " + "x"+value;
        }

        public void SetMultiplierTextUpCountEvolution(int value)
        {
            if(value > 1)
                _textButtonEvolution.text = TextInpruvment + " " + value;
        }

        public void StandardText()
        {
            _textButtonImpruvment.text = TextInpruvment;
        }
    }
}
