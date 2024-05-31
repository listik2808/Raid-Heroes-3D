using Assets.Scripts.Currency;
using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.SpecificationsUI;
using Scripts.Logic;
using Scripts.Logic.TaskAchievements;
using Source.Scripts.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenUprageSoldier : MonoBehaviour 
    {
        [SerializeField] private RankUI _rangUI;
        [SerializeField] private SpecialSkill _specialSkill;
        [SerializeField] private Survivability _survivability;
        [SerializeField] private MeleeDamage _meleeDamage;
        //[SerializeField] private SpeedSkill _speedSkill;
        [SerializeField] private BookmarkButton _closeButton;
        [SerializeField] private BookmarkButton _remuveSoldier;
        [SerializeField] private BookmarkButton _addSoldier;
        [SerializeField] private Squad _squad;
        [SerializeField] private Heroes _heroes;
        [SerializeField] protected ScreenNoPay _screenNoPay;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private BookmarkButton _buttonLeft;
        [SerializeField] private BookmarkButton _buttonRight;
        [SerializeField] private ScrollRect _scrollViewSquad;
        [SerializeField] private ScrollRect _scrollViewHeroes;
        [SerializeField] private GameObject _walet;
        [SerializeField] private CanvasGroup _canvasGroupScreenMain;
        [SerializeField] private BookmarkButton _optionButton;
        [SerializeField] private ButtonMultiplier _buttonMultiplier;
        [SerializeField] private MarkersImprovingSoldiers _markersImprovingSoldiers;
        [SerializeField] private HireHeroes _hireHeroes;
        [SerializeField] private MakeAnyImprovementsHeroAchievement _makeAnyImprovementsHeroAchievement;
        [SerializeField] private MakeEvolutionHeroesAchievement _makeEvolutionHeroesAchievement;
        [SerializeField] private UpRankAchievement _upRankAchievement;

        protected IPersistenProgressService _progressService;
        private Coins _coins;
        private Diamonds _diamonds;
        private Soldier _soldier;
        private Soldier _newSoldier;
        private int _index;
        private bool _squadArmy = false;

        private float _stepSpecSkill = 0;
        private float _priceSpecSkill = 0;
        private float _levelSpecScill = 0;
        private float _allStep = 0;
        private float _priceCurrent = 0;
        private float _levelCurrent = 0;

        private float _stepMeleeDamage = 0;
        private float _priceMeleeDamage = 0;
        private float _levelMeleeDamageSkill = 0;
        private float _allStepMeleeDamage = 0;
        private float _priceCurrentMeleeDamage = 0;
        private float _levelCurrentMeleeDamage = 0;

        private float _hpStep = 0;
        private float _priceHpSkill = 0;
        private float _levelHpSkill = 0;
        private float _allStepHp = 0;
        private float _priceCurrentHp = 0;
        private float _levelCurrentHp = 0;

        private List<Soldier> _soldierSquad = new List<Soldier>();
        private List<Soldier> _soldierSquadCastle = new List<Soldier>();

        //public event Action ImprovementsSpecialSkill;

        public List<Soldier> SoldierHero => _soldierSquad;
        public List<Soldier> SoldierSquad => _soldierSquadCastle;

        public MakeAnyImprovementsHeroAchievement MakeAnyImprovementsHeroAchievement => _makeAnyImprovementsHeroAchievement;
        public MakeEvolutionHeroesAchievement MakeEvolutionHeroesAchievement => _makeEvolutionHeroesAchievement;
        public UpRankAchievement UpRankAchievement => _upRankAchievement;

        public HireHeroes Hire => _hireHeroes;
        public float CurrentStepHp => _hpStep;
        public float LevelHpSkill => _levelHpSkill;
        public float AllStepHp => _allStepHp;
        public float PriceCurrentHp => _priceCurrentHp;
        public float LevelCurrentHp => _levelCurrentHp;

        public float CurerntStepMeleeDamage => _stepMeleeDamage;
        public float LevelMeleeDamageSkill => _levelMeleeDamageSkill;
        public float AllStepMeleeDamage => _allStepMeleeDamage;
        public float PriceCurrentMeleeDamage => _priceCurrentMeleeDamage;
        public float LevelCurrentMeleeDamage => _levelCurrentMeleeDamage;

        public float LevelSpecSkill => _levelSpecScill;
        public float CurrentStepSpecSkill => _stepSpecSkill;
        public float LevelCurrent => _levelCurrent;
        public float AllStep => _allStep;
        public float PriceCurrent => _priceCurrent;

        public ButtonMultiplier ButtonMultiplier => _buttonMultiplier;
        public Soldier Soldier => _soldier;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public ScreenNoPay ScreenNoPay => _screenNoPay;
        public IPersistenProgressService ProgressService => _progressService;
        public RankUI RangUI => _rangUI;
        public SpecialSkill SpecialSkill => _specialSkill;
        public Survivability Survivability => _survivability;
        public MeleeDamage MeleeDamage => _meleeDamage;
        //public SpeedSkill SpeedSkill => _speedSkill;
        public Squad Squad => _squad;
        public Heroes Heroes => _heroes;
        public BookmarkButton CloseButton => _closeButton;
        public BookmarkButton RemuveSoldierButton => _remuveSoldier;
        public BookmarkButton AddSoldierButton => _addSoldier;

        private void OnEnable()
        {
            ActivateEndDiactivateButton();
            _optionButton.gameObject.SetActive(false);
            _canvasGroupScreenMain.blocksRaycasts = false;
            _walet.gameObject.SetActive(false);
            _scrollViewHeroes.gameObject.SetActive(false);
            _scrollViewSquad.gameObject.SetActive(false);
            _squad.ChangedSquad += SetSquad;
            _heroes.ChangadSquadCastle += SetSquadCastle;
            _buttonLeft.ButtonOnClic += ChooseNextLeft;
            _buttonRight.ButtonOnClic += ChooseNextRight;
            _closeButton.ButtonOnClic += CloseScreenUpgradeSoldier;
        }

        private void OnDisable()
        {
            _optionButton.gameObject.SetActive(true);
            _canvasGroupScreenMain.blocksRaycasts = true;
            _walet.gameObject.SetActive(true);
            _scrollViewHeroes.gameObject.SetActive(true);
            _scrollViewSquad.gameObject.SetActive(true);
            _squad.ChangedSquad -= SetSquad;
            _heroes.ChangadSquadCastle -= SetSquadCastle;
            _buttonLeft.ButtonOnClic -= ChooseNextLeft;
            _buttonRight.ButtonOnClic -= ChooseNextRight;
            _closeButton.ButtonOnClic -= CloseScreenUpgradeSoldier;
        }

        private void Awake()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        public void FillingBarSpecialSkill(float currentStep,float maxStep , float maxLevelSkill,float currentLevelSkill,float bestLevel,ButtonMultiplier buttonMultiplier)
        {
            _stepSpecSkill = 0;
            _priceSpecSkill = 0;
            _levelSpecScill = 0;
            _allStep = 0;
            _priceCurrent = 0;
            _levelCurrent = 0;

            int upgradeMultiplier = 1;
            if (currentStep == maxStep && currentLevelSkill == maxLevelSkill)
            {
                _specialSkill.ButtonEvolution.gameObject.SetActive(false);
                _specialSkill.ImprovementButton.gameObject.SetActive(false);
                _specialSkill.ActivateText();
            }
            else if(currentStep == maxStep && currentLevelSkill != maxLevelSkill)
            {
                _specialSkill.ButtonEvolution.gameObject.SetActive(true);
                _specialSkill.ImprovementButton.gameObject.SetActive(false);
                _specialSkill.DeactivateText();
                var value = Price.GetUpgradeCostDiamonds(currentLevelSkill, bestLevel);
                if(value > 0)
                {
                    _levelSpecScill = currentLevelSkill + 1;
                    _specialSkill.SetIconEvolutionDiamond();
                    _specialSkill.SetPriceDiamonds(value);
                }
                else 
                {
                    _specialSkill.SetIconEvolutionCoin();
                    var price = Price.GetUpgradeCostCoin(currentLevelSkill,currentStep,maxStep, upgradeMultiplier, true);
                    _specialSkill.SetPriceDiamonds(price);
                }
            }
            else if (currentStep != maxStep)
            {
                _specialSkill.ButtonEvolution.gameObject.SetActive(false);
                _specialSkill.ImprovementButton.gameObject.SetActive (true);
                _specialSkill.DeactivateText();
                if (buttonMultiplier.IsTen || buttonMultiplier.IsMax)
                {
                    _stepSpecSkill = currentStep;
                    _levelSpecScill = currentLevelSkill;
                    TryBuyTenTimes(ref _stepSpecSkill, maxStep, ref _levelSpecScill, maxLevelSkill, bestLevel,
                        _priceSpecSkill, upgradeMultiplier, ref _allStep, ref _priceCurrent, ref _levelCurrent, buttonMultiplier);
                    if (_allStep == 1)
                    {
                        _priceSpecSkill = Price.GetUpgradeCostCoin(currentLevelSkill, _stepSpecSkill, maxStep, upgradeMultiplier, true);
                        _specialSkill.SetPriceCoins(_priceSpecSkill);
                        _specialSkill.StandardText();
                    }
                    else if(_allStep < 1)
                    {
                        _stepSpecSkill += 1;
                        _priceSpecSkill = Price.GetUpgradeCostCoin(currentLevelSkill, _stepSpecSkill, maxStep, upgradeMultiplier, true);
                        _specialSkill.SetPriceCoins(_priceSpecSkill);
                        _specialSkill.StandardText();
                    }
                    else
                    {
                        _specialSkill.SetPriceCoins(_priceCurrent);
                        _specialSkill.SetMultiplierTextUpCountImpruvment((int)_allStep);
                    }
                }
                else if (buttonMultiplier.IsTen == false && buttonMultiplier.IsMax == false)
                {
                    _stepSpecSkill = currentStep +1;
                    _priceSpecSkill = Price.GetUpgradeCostCoin(currentLevelSkill, _stepSpecSkill, maxStep, upgradeMultiplier, true);
                    _specialSkill.SetPriceCoins(_priceSpecSkill);
                    _specialSkill.StandardText();
                }
            }

            _specialSkill.SliderSkill.value = currentStep / maxStep;
        }

        public void FillingBarSurvivability(float currentValue, float maxValue, int maxLevel, int currentLevel, float bestLevel, ButtonMultiplier buttonMultiplier)
        {
            _hpStep = 0;
            _priceHpSkill = 0;
            _levelHpSkill = 0;
            _allStepHp = 0;
            _priceCurrentHp = 0;
            _levelCurrentHp = 0;
            int upgradeMultiplierHp = 2;

            if (currentValue == maxValue && currentLevel == maxLevel)
            {
                _survivability.ButtonEvolution.gameObject.SetActive(false);
                _survivability.ImprovementButton.gameObject.SetActive(false);
                _survivability.ActivateText();
            }
            else if(currentValue == maxValue && currentLevel != maxLevel)
            {
                _survivability.ButtonEvolution.gameObject.SetActive(true);
                _survivability.ImprovementButton.gameObject.SetActive(false);
                _survivability.DeactivateText();
                var value = Price.GetUpgradeCostDiamonds(currentLevel, bestLevel);
                if (value > 0)
                {
                    _levelHpSkill = currentLevel + 1; 
                    _survivability.SetIconEvolutionDiamond();
                    _survivability.SetPriceDiamonds(value);
                }
                else
                {
                    _survivability.SetIconEvolutionCoin();
                    var price = Price.GetUpgradeCostCoin(currentLevel, currentValue, maxValue, 2, true);
                    _survivability.SetPriceDiamonds(price);
                }
            }
            else if(currentValue != maxValue)
            {
                _survivability.ButtonEvolution.gameObject.SetActive(false);
                _survivability.ImprovementButton.gameObject.SetActive(true);
                _survivability.DeactivateText();
                if (buttonMultiplier.IsTen || buttonMultiplier.IsMax)
                {
                    _hpStep = currentValue;
                    _levelHpSkill = currentLevel;
                    TryBuyTenTimes(ref _hpStep, maxValue, ref _levelHpSkill, maxLevel, bestLevel, _priceHpSkill,
                        upgradeMultiplierHp, ref _allStepHp, ref _priceCurrentHp, ref _levelCurrentHp, buttonMultiplier);
                    if (_allStepHp == 1)
                    {
                        _priceHpSkill = Price.GetUpgradeCostCoin(currentLevel, _hpStep, maxValue, upgradeMultiplierHp, true);
                        _survivability.SetPriceCoins(_priceHpSkill);
                        _survivability.StandardText();
                    }
                    else if(_allStepHp < 1)
                    {
                        _hpStep += 1;
                        _priceHpSkill = Price.GetUpgradeCostCoin(currentLevel, _hpStep, maxValue, upgradeMultiplierHp, true);
                        _survivability.SetPriceCoins(_priceHpSkill);
                        _survivability.StandardText();
                    }
                    else
                    {
                        _survivability.SetPriceCoins(_priceCurrentHp);
                        _survivability.SetMultiplierTextUpCountImpruvment((int)_allStepHp);
                    }
                }
                else if (buttonMultiplier.IsTen == false && buttonMultiplier.IsMax == false)
                {
                    _hpStep = currentValue +1;
                    _priceHpSkill = Price.GetUpgradeCostCoin(currentLevel, _hpStep, maxValue, upgradeMultiplierHp, true);
                    _survivability.SetPriceCoins(_priceHpSkill);
                    _survivability.StandardText();
                }
            }

            _survivability.SliderSkill.value = currentValue / maxValue;
        }

        public void FillingBarMeleeDamage(float currentValue,float maxValue, int maxMelleDamageLevel, int currentMelleDamageLevel,float bestLevel, ButtonMultiplier buttonMultiplier)
        {
            _stepMeleeDamage = 0;
            _priceMeleeDamage = 0;
            _levelMeleeDamageSkill = 0;
            _allStepMeleeDamage = 0;
            _priceCurrentMeleeDamage = 0;
            _levelCurrentMeleeDamage = 0;
            int upgradeMultiplierMeleeDamage = 1;

            if (currentValue == maxValue && currentMelleDamageLevel == maxMelleDamageLevel)
            {
                _meleeDamage.ButtonEvolution.gameObject.SetActive(false);
                _meleeDamage.ImprovementButton.gameObject.SetActive(false);
                _meleeDamage.ActivateText();
            }
            else
            {
                if (currentValue == maxValue && currentMelleDamageLevel != maxMelleDamageLevel)
                {
                    _meleeDamage.ButtonEvolution.gameObject.SetActive(true);
                    _meleeDamage.ImprovementButton.gameObject.SetActive(false);
                    _meleeDamage.DeactivateText();
                    var value = Price.GetUpgradeCostDiamonds(currentMelleDamageLevel, bestLevel);
                    if (value > 0)
                    {
                        _levelMeleeDamageSkill = currentMelleDamageLevel + 1;
                        _meleeDamage.SetIconEvolutionDiamond();
                        _meleeDamage.SetPriceDiamonds(value);
                    }
                    else
                    {
                        _meleeDamage.SetIconEvolutionCoin();
                        var price = Price.GetUpgradeCostCoin(currentMelleDamageLevel, currentValue, maxValue, 1, true);
                        _meleeDamage.SetPriceDiamonds(price);
                    }
                }
                else if (currentValue != maxValue)
                {
                    _meleeDamage.ButtonEvolution.gameObject.SetActive(false);
                    _meleeDamage.ImprovementButton.gameObject.SetActive(true);
                    _meleeDamage.DeactivateText();
                    if (buttonMultiplier.IsTen || buttonMultiplier.IsMax)
                    {
                        _stepMeleeDamage = currentValue;
                        _levelMeleeDamageSkill = currentMelleDamageLevel;

                        TryBuyTenTimes(ref _stepMeleeDamage, maxValue, ref _levelMeleeDamageSkill, maxMelleDamageLevel, bestLevel,
                            _priceMeleeDamage, upgradeMultiplierMeleeDamage, ref _allStepMeleeDamage, ref _priceCurrentMeleeDamage,
                            ref _levelCurrentMeleeDamage, buttonMultiplier);

                        if (_allStepMeleeDamage == 1)
                        {
                            _priceMeleeDamage = Price.GetUpgradeCostCoin(currentMelleDamageLevel, _stepMeleeDamage, maxValue, upgradeMultiplierMeleeDamage, true);
                            _meleeDamage.SetPriceCoins(_priceMeleeDamage);
                            _meleeDamage.StandardText();
                        }
                        else if(_allStepMeleeDamage < 1)
                        {
                            _stepMeleeDamage += 1;
                            _priceMeleeDamage = Price.GetUpgradeCostCoin(currentMelleDamageLevel, _stepMeleeDamage, maxValue, upgradeMultiplierMeleeDamage, true);
                            _meleeDamage.SetPriceCoins(_priceMeleeDamage);
                            _meleeDamage.StandardText();
                        }
                        else
                        {
                            _meleeDamage.SetPriceCoins(_priceCurrentMeleeDamage);
                            _meleeDamage.SetMultiplierTextUpCountImpruvment((int)_allStepMeleeDamage);
                        }
                    }
                    else if (buttonMultiplier.IsTen == false && buttonMultiplier.IsMax == false)
                    {
                        _stepMeleeDamage = currentValue +1;
                        _priceMeleeDamage = Price.GetUpgradeCostCoin(currentMelleDamageLevel, _stepMeleeDamage, maxValue, upgradeMultiplierMeleeDamage, true);
                        _meleeDamage.SetPriceCoins(_priceMeleeDamage);
                        _meleeDamage.StandardText();
                    }
                }
            }
            _meleeDamage.SliderSkill.value = currentValue / maxValue;
        }

        //public void FillingBarSpeedSkill(float currentValue,float maxValue, int maxSpeedLevel, int currentSpeedLevel,float bestLevel)
        //{
        //    if (currentValue == maxValue && currentSpeedLevel == maxSpeedLevel)
        //    {
        //        _speedSkill.ButtonEvolution.gameObject.SetActive(false);
        //        _speedSkill.ImprovementButton.gameObject.SetActive(false);
        //    }
        //    else if (currentValue == maxValue && currentSpeedLevel != maxSpeedLevel)
        //    {
        //        _speedSkill.ButtonEvolution.gameObject.SetActive(true);
        //        _speedSkill.ImprovementButton.gameObject.SetActive(false);

        //        var value = Price.GetUpgradeCostDiamonds(currentSpeedLevel, bestLevel);
        //        if (value > 0)
        //        {
        //            _speedSkill.SetIconEvolutionDiamond();
        //            _speedSkill.SetPriceDiamonds(value);
        //        }
        //        else
        //        {
        //            _speedSkill.SetIconEvolutionCoin();
        //            var price = Price.GetUpgradeCostCoin(currentSpeedLevel, currentValue, maxValue, 1, true);
        //            _speedSkill.SetPriceDiamonds(price);
        //        }
        //    }
        //    else if (currentValue != maxValue) 
        //    {
        //        _speedSkill.ButtonEvolution.gameObject.SetActive(false);
        //        _speedSkill.ImprovementButton.gameObject.SetActive(true);
        //        var price = Price.GetUpgradeCostCoin(currentSpeedLevel, currentValue, maxValue, 1, true);
        //        _speedSkill.SetPriceCoins(price);
        //    }

        //    _speedSkill.SliderSkill.value = currentValue / maxValue;
        //}

        public void AddSoldierSquad()
        {
            _soldierSquad = _squad.Soldiers;
            _soldierSquadCastle = _heroes.Soldiers;
            ActivateEndDiactivateButton();
        }

        public void SetSoldier(Soldier soldier)
        {
            _soldier = soldier;
        }

        public void CheckSwitchButtons() => ActivateEndDiactivateButton();

        private void ChooseNextLeft()
        {
            int number;
            bool result;
            result = SearchSoldier(_soldierSquad);
            if (result)
            {
                int maxcount = _soldierSquad.Count;
                if (_index -1 >= 0)
                {
                    Diactivate(_squad.SoldierСards);
                    number = _index - 1;
                    _newSoldier = _soldierSquad[number];
                    ActivateCard(_squad.SoldierСards);
                }
                else if(_index -1 <= -1)
                {
                    Diactivate(_squad.SoldierСards);
                    number = maxcount - 1;
                    _newSoldier = _soldierSquad[number];
                    ActivateCard(_squad.SoldierСards);
                }
            }
            else
            {
                SearchSoldier(_soldierSquadCastle);
                int maxcount = _soldierSquadCastle.Count;
                if (_index - 1 >= 0)
                {
                    Diactivate(_heroes.SoldierСards);
                    number = _index - 1;
                    _newSoldier = _soldierSquadCastle[number];
                    ActivateCard(_heroes.SoldierСards);
                }
                else if (_index - 1 <= -1)
                {
                    Diactivate(_heroes.SoldierСards);
                    number = maxcount - 1;
                    _newSoldier = _soldierSquadCastle[number];
                    ActivateCard(_heroes.SoldierСards);
                }
            }
        }

        private bool SetFindingCard()
        {
            foreach (Soldier squad in _soldierSquad)
            {
                if (squad.HeroTypeId == _soldier.HeroTypeId)
                {
                    return true;
                }
            }
            return false;
        }

        private void ActivateEndDiactivateButton()
        {
            _squadArmy = SetFindingCard();
            if (_squadArmy)
            {
                if (SearchSoldier(_soldierSquad) == true)
                {
                    if (_soldierSquad.Count > 1)
                    {
                        _buttonLeft.gameObject.SetActive(true);
                        _buttonRight.gameObject.SetActive(true);
                    }
                    else
                    {
                        _buttonLeft.gameObject.SetActive(false);
                        _buttonRight.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (SearchSoldier(_soldierSquadCastle) == true)
                {
                    if (_soldierSquadCastle.Count > 1)
                    {
                        _buttonLeft.gameObject.SetActive(true);
                        _buttonRight.gameObject.SetActive(true);
                    }
                    else
                    {
                        _buttonLeft.gameObject.SetActive(false);
                        _buttonRight.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void Diactivate(List<SoldierCard> soldierCards)
        {
            foreach (var item in soldierCards)
            {
                item.SoldierCardViewer.Start();
                item.SoldierCardViewer.CloseUpgradeSoldier();
            }
        }

        private void ActivateCard(List<SoldierCard> soldierCard)
        {
            foreach (SoldierCard i in soldierCard)
            {
                if (i.Soldier.HeroTypeId == _newSoldier.HeroTypeId)
                {
                    i.SetSoliedrMap(_newSoldier);
                    i.SoldierCardViewer.Start();
                    i.SoldierCardViewer.OpenSoldierUpgradeScreen();
                }
            }
        }

        private void ChooseNextRight()
        {
            int number;
            bool result;
            result = SearchSoldier(_soldierSquad);
            if (result)
            {
                int maxcount = _soldierSquad.Count;
                if (_index + 1 <= maxcount -1)
                {
                    Diactivate(_squad.SoldierСards);
                    number = _index + 1;
                    _newSoldier = _soldierSquad[number];
                    ActivateCard(_squad.SoldierСards);
                }
                else if (_index + 1 > maxcount -1)
                {
                    Diactivate(_squad.SoldierСards);
                    number = 0;
                    _newSoldier = _soldierSquad[number];
                    ActivateCard(_squad.SoldierСards);
                }
            }
            else
            {
                SearchSoldier(_soldierSquadCastle);
                int maxcount = _soldierSquadCastle.Count;
                if (_index + 1 <= maxcount -1)
                {
                    Diactivate(_heroes.SoldierСards);
                    number = _index + 1;
                    _newSoldier = _soldierSquadCastle[number];
                    ActivateCard(_heroes.SoldierСards);
                }
                else if (_index + 1 > maxcount -1)
                {
                    Diactivate(_heroes.SoldierСards);
                    number = 0;
                    _newSoldier = _soldierSquadCastle[number];
                    ActivateCard(_heroes.SoldierСards);
                }
            }
        }

        private bool SearchSoldier(List<Soldier> soldiers)
        {
            foreach (Soldier item in soldiers)
            {
                if (item.HeroTypeId == _soldier.HeroTypeId)
                {
                    _index = soldiers.IndexOf(item);
                    return true;
                }
            }
            return false;
        }

        private void SetSquad()
        {
            _soldierSquad = _squad.Soldiers;
        }

        private void SetSquadCastle()
        {
            _soldierSquadCastle = _heroes.Soldiers;
        }

        private void CloseScreenUpgradeSoldier()
        {
            _soldier.DataSoldier.SoldierСard.SoldierCardViewer.CloseUpgradeSoldier();
            _markersImprovingSoldiers.ActivateRecalculation();
        }

        private void TryBuyTenTimes(ref float step, float maxStep,ref float currentLevel, float maxLevel, float bestLevel, float price, int up,ref float allStep,ref float priceCurrent,ref float newLevel,ButtonMultiplier buttonMultiplier)
        {
            price = Price.GetUpgradeCostCoin(currentLevel, step, maxStep, up, true);
            if (CheckingMoneySufficiency(price, _progressService.Progress.Wallet.Coins.Count))
            {
                priceCurrent = price;
                allStep++;
            }
            else
            {
                return;
            }

            for (int i = 0; i < buttonMultiplier.CurrentMultiplier; i++)
            {
                step++;
                if (step == maxStep)
                {
                    if (bestLevel > currentLevel)
                    {
                        if (currentLevel + 1 > maxLevel)
                            break;
                        price += Price.GetUpgradeCostCoin(currentLevel +1, 0, maxStep, up, true);
                        if (CheckingMoneySufficiency(price, _progressService.Progress.Wallet.Coins.Count))
                        {
                            currentLevel++;
                            allStep++;
                            priceCurrent = price;
                            newLevel = currentLevel;
                            step = 0;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (currentLevel == maxLevel)
                    {
                        break;
                    }
                    else if(bestLevel <= currentLevel && currentLevel != maxLevel)
                    {
                        break;
                    }
                }
                else if (step < maxStep)
                {
                    price += Price.GetUpgradeCostCoin(currentLevel, step, maxStep, up, true);
                    if (CheckingMoneySufficiency(price, _progressService.Progress.Wallet.Coins.Count))
                    {
                        allStep++;
                        priceCurrent = price;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private bool CheckingMoneySufficiency(float coins,float money)
        {
            if(money >= coins)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
