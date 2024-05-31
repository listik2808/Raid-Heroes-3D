using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.SpecificationsUI
{
    public class RankUI : MonoBehaviour
    {
        public const string HireHero = "Нанять героя";
        public const string RaiseRank = "Поднять ранг";
        public const string TextHiring = "Собери еще ";
        public const string TextPower = "Сила:";
        public const string TextSpecialization = "Специализация: ";
        public const string TextUpRankDefault = "Повысить";
        public const string TextOneRankHero = "Нанять";

        [SerializeField] private TMP_Text _textSpecialization;
        [SerializeField] private TMP_Text _textTypeHero;
        [SerializeField] private Sprite _spriteUpgade;
        [SerializeField] private Sprite _spriteDefault;
        [SerializeField] private List<Image> _rangImageList;
        [SerializeField] private TMP_Text _textPower;
        [SerializeField] private TMP_Text _textHiringCards;
        [SerializeField] private BookmarkButton _buttonAdd;
        [SerializeField] private BookmarkButton _buttonRemove;
        [SerializeField] private BookmarkButton _upRank;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private Image _iconCardBackground;
        [SerializeField] private Image _typeCard;
        [SerializeField] private TMP_Text _textCountCard;
        [SerializeField] private RawImage _iconHeroCard;
        [SerializeField] private Sprite _iconBaseCardBackground;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private GameObject _heroContainer;
        [SerializeField] private Image _infoImpruvment;
        [SerializeField] private TMP_Text _textInfo;
        [SerializeField] private TMP_Text _textButtonUpRank;

        private int _upRangMarker = 0;
        private string _nameSpecizlization;
        private string _name;
        private int _price;
        private PriceLevelHeroCard _priceLevelHeroCard;
        private Soldier _soldier;
        private CameraParent _cameraParent;

        public int Price => _price;
        public TMP_Text TextHiringCards => _textHiringCards;
        public TMP_Text PowerText => _textPower;
        public BookmarkButton ButtonAdd => _buttonAdd;
        public BookmarkButton UpRank => _upRank;
        public GameObject Container => _container;
        public TMP_Text Message => _message;
        public GameObject HeroContainer => _heroContainer;
        public int UpRangMarker => _upRangMarker;

        public void ActivateButtonPriceUpRank(int pricel)
        {
            _upRank.gameObject.SetActive(true);
            _upRank.Button.interactable = true;
            _infoImpruvment.gameObject.SetActive(true);
            _textInfo.text = 1.ToString();
            _upRangMarker = 1;
            //_buttonAdd.gameObject.SetActive(false);
            //_buttonRemove.gameObject.SetActive(false);
            string result = AbbreviationsNumbers.ShortNumber(pricel);
            _textPrice.text = result;
            _container.SetActive(true);
            _textHiringCards.gameObject.SetActive(false);
            _message.gameObject.SetActive(false);
        }

        public void SetPositionSquad(bool result, int count)
        {
            if(count > 1)
            {
                if (result)
                {
                    _buttonAdd.gameObject.SetActive(false);
                    _buttonRemove.gameObject.SetActive(true);
                    _message.gameObject.SetActive(false);
                }
                else
                {
                    _buttonRemove.gameObject.SetActive(false);
                    _buttonAdd.gameObject.SetActive(true);
                    _message.gameObject.SetActive(false);
                }
            }
        }

        public void SetRang(Soldier soldier)
        {
            _soldier = soldier;
            int count = soldier.Rank.CurrentLevelHero;
            if (count > 0)
            {
                for (int i = 0; i < _rangImageList.Count; i++)
                {
                    if (count > 0)
                    {
                        _rangImageList[i].sprite = _spriteUpgade;
                        count--;
                    }
                }
            }
            else
            {
                foreach (var item in _rangImageList)
                {
                    item.sprite = _spriteDefault;
                }
            }
        }

        public void SetPowerHero(float value)
        {
            value = (float)Math.Round(value, 2);
            _textPower.gameObject.SetActive(true);
            string result = AbbreviationsNumbers.ShortNumber(value);
            _textPower.text = TextPower + result;
        }

        public void SetPrice(Soldier soldier,IPersistenProgressService persistenProgressService)
        {
            if(persistenProgressService.Progress.Training.Tutor == false 
                && persistenProgressService.Progress.Training.Finish == false
                && soldier.Rank.MaxCountCard <= soldier.Rank.CurrentCountCard)
            {
                _price = 0;
                _textHiringCards.gameObject.SetActive(false);
                _textButtonUpRank.text = TextOneRankHero;
                ActivateButtonPriceUpRank(_price);
            }
            else
            {
                _priceLevelHeroCard = new PriceLevelHeroCard();
                if (soldier.Rank.MaxCountCard <= soldier.Rank.CurrentCountCard)
                {
                    _price = _priceLevelHeroCard.GetPrice(soldier.Rank.CurrentLevelHero);
                    if (soldier.Rank.CurrentLevelHero == -1)
                    {
                        _textHiringCards.gameObject.SetActive(false);
                        _textButtonUpRank.text = TextOneRankHero;
                        ActivateButtonPriceUpRank(_price);
                    }
                    else
                    {
                        _textButtonUpRank.text = TextUpRankDefault;
                        ActivateButtonPriceUpRank(_price);
                    }
                }
                else
                {
                    DiactivateButtonPrice();
                }
            }
            
        }
        public void DiactivateButtonPrice()
        {
            _upRangMarker = 0;
            if(_upRank != null)
            {
                _upRank.gameObject.SetActive(false);
                _upRank.Button.interactable = false;
            }
            if(_infoImpruvment != null)
                _infoImpruvment.gameObject.SetActive(false);
        }

        public void ShowCountCardHiring()
        {
            _container.SetActive(false);
            _textHiringCards.gameObject.SetActive(true);
            int count = _soldier.Rank.MaxCountCard -_soldier.Rank.CurrentCountCard;
            _textHiringCards.text = TextHiring +" "+ count.ToString() + "  карты этого героя, чтобы нанять его";
        }

        public void SetСard()
        {
            //Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Russian");
            int value = (int)_soldier.DataSoldier.Type;
           // _name = _soldier.HeroTypeId.ToString();
            //_name = Lean.Localization.LeanLocalization.GetTranslationText(_name);
            _name = CustomRuLocalization.GetRuHeroTypeId((int)_soldier.HeroTypeId);
            _textTypeHero.text = _name;
            //_nameSpecizlization = _soldier.Specialty.ToString();
            //_nameSpecizlization = Lean.Localization.LeanLocalization.GetTranslationText(_nameSpecizlization);
            _nameSpecizlization = CustomRuLocalization.GetSpecialization((int)_soldier.Specialty);
            _textSpecialization.text = TextSpecialization + _nameSpecizlization;
            _typeCard.sprite = _soldier.DataSoldier.CardSoldierType.BaseCard.TypeIcons[value];
            _textCountCard.text = _soldier.Rank.CurrentCountCard + "/" + _soldier.Rank.MaxCountCard;
            _iconHeroCard.texture = _cameraParent.RenderTexture;
            if (_soldier.Rank.CurrentLevelHero >= 0)
            {
                _iconCardBackground.sprite = _soldier.DataSoldier.CardSoldierType.BaseCard.TypeSprites[value];
            }
            else
            {
                _iconCardBackground.sprite = _iconBaseCardBackground;
            }
        }

        public void SetTextureHero(CameraParent cameraParent)
        {
            _cameraParent = cameraParent;
            //_cameraParent.gameObject.SetActive(true);
        }

        public void ActivateCamera(CameraParent cameraParent)
        {
            _cameraParent.gameObject.SetActive(true);
        }
    }
}
