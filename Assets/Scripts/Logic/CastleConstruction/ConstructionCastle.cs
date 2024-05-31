using Scripts.Army.AllCadsHeroes;
using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scripts.Logic.CastleConstruction
{
    public abstract class ConstructionCastle : MonoBehaviour
    {
        public const string Text1 = "Одержать еще ";
        public const string Text2 = "побед в рейдах";

        [SerializeField] private Sprite _sprite;
        [SerializeField] protected TypeBuilding typeBuilding;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] protected TMP_Text _textLevel;
        [SerializeField] private Image _imageLevelIcon;
        [SerializeField] protected ScreenNoPay _screenNoPay;
        [SerializeField] protected int _generationTime;
        [SerializeField] protected int _numberBattlesFought;
        [SerializeField] protected ConstructionCastle _constructionCastle;
        [SerializeField] protected CurrencyType _currencyType;
        [SerializeField] protected float _costConstruction;
        [SerializeField] protected float _basicCostImprovement;
        [SerializeField] protected float _coefficientPriceUpgrade;
        [SerializeField] protected float _incomCoefficent;
        [SerializeField] protected float _incomCoefficentUpgrade;
        [SerializeField] protected BookmarkButton _takeIncome;
        [SerializeField] protected BookmarkButton _upgradeButton;
        [SerializeField] protected BookmarkButton _buttonBuild;
        [SerializeField] protected TMP_Text _textPricePay;
        [SerializeField] protected TMP_Text _priceUpgrade;
        [SerializeField] protected TMP_Text _textSliderTakeIncome;
        [SerializeField] protected Slider _slider;
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected TMP_Text _textCastlBuild;
        [SerializeField] protected PassiveIncome _passiveIncome;
        [SerializeField] protected MainScreen _mainScreen;
        [SerializeField] protected ScreenCardShow _screenCardShow;
        [SerializeField] protected ListAllHeroes _allHeroes;
        [SerializeField] private Squad _squad;
        [SerializeField] private Heroes _heroes;
        [SerializeField] private TMP_Text _allTextCard;

        [SerializeField] private Image _infoImageTakeIncome;
        [SerializeField] private TMP_Text _textTakeIncome;
        [SerializeField] private Image _infoUpgrage;
        [SerializeField] private TMP_Text _textUpgrage;
        [SerializeField] private Image _infoBattonBuild;
        [SerializeField] private TMP_Text _textButtonBuild;

        protected IPersistenProgressService _persistenProgressService;
        private ISaveLoadService _saveLoadService;
        private List<Soldier> _soldierTypeCard = new List<Soldier>();
        protected float _elepsedTime = 0;
        protected float _second = 0;
        protected float _currentСostСonstruction;
        protected float _countCurrency;
        protected float _maxVolume;
        protected float _currentVolume;
        protected float _currentTimerTime;
        private float _newCurrency;
        protected int _currentBattles;
        protected float _countreward;
        protected int _level =0;
        protected bool _isOpen = false;
        protected bool _isFull = false;
       // protected bool _isOpenShowWindow = true;
        protected bool _isCalculationsTime = false;
        protected string _convertedTime;
        protected string _currentTime;
        protected string _hours;
        protected string _minutes;
        protected string _seconds;
        protected string _ch = ":";
        protected int _rewardMarker = 0;
        private int _improvementsMarker = 0;
        private int _payMarker = 0;
        private int _allMarker; 

        public ISaveLoadService SaveLoadService => _saveLoadService;
        public IPersistenProgressService PersistenProgressService => _persistenProgressService;
        public float CurrentСostСonstruction => _currentСostСonstruction;
        public ConstructionCastle Castle => _constructionCastle;
        public int NumberBattlesFought => _numberBattlesFought;
        public Sprite Sprite => _sprite;
        public float NewCurrency => _newCurrency;
        public bool IsOpen => _isOpen;
        public int Level => _level;
        public float IncomCoefficent => _incomCoefficent;
        public float IncomCoefficentUpgrade => _incomCoefficentUpgrade;
        public float CountCurrency => _countCurrency;
        public float CostConstruction => _costConstruction;
        public float BasicCostImprovement => _basicCostImprovement;
        public float CoefficientPriceUpgrade => _coefficientPriceUpgrade;
        public int GenerationTime => _generationTime;
        public TMP_Text Name => _nameText;
        public CurrencyType CurrencyType => _currencyType;
        public TypeBuilding Building => typeBuilding;

        public event Action TryBuilding;
        public event Action MarketChanged;
        public event Action OpenBuilding;
        public event Action ImpruvementBuilding;

        public int CurrentBattles
        {
            get
            {
                return _currentBattles;
            }
            set
            {
                if(_currentBattles != value)
                {
                    _currentBattles = value;
                }
                if (_isOpen == false)
                    SetNumberBattles(_currentBattles);
            }
        }

        private void OnEnable()
        {
            if(_persistenProgressService == null)
                _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _takeIncome.ButtonOnClic += GetAward;
            LoadData(_persistenProgressService);
            _buttonBuild.ButtonOnClic += TryPay;
            _upgradeButton.ButtonOnClic += TryBuyImprovements;
            CurrentBattles = _mainScreen.BattleScene.FlightNumber.CurrentNumber;
            if (_constructionCastle != null && _isOpen == false)
            {
                _constructionCastle.TryBuilding += Restart;
            }
        }

        private void OnDisable()
        {
            //_isCalculationsTime = false;
            _takeIncome.ButtonOnClic -= GetAward;
            _buttonBuild.ButtonOnClic -= TryPay;
            _upgradeButton.ButtonOnClic -= TryBuyImprovements;
            if (_constructionCastle != null && _isOpen == false)
            {
                _constructionCastle.TryBuilding -= Restart;
            }
            SaveLoadService.SaveProgress();
        }

        public abstract void FillingBar();
        public abstract void GetAward();
        public abstract void SavaData(string timetext, float _currentTimerTime);
        public abstract void LoadData(IPersistenProgressService persistenProgressService);

        public void DiactivateCardShoeWindow()
        {
            //_isOpenShowWindow = false;
            _screenCardShow.gameObject.SetActive(false);
            if (_currentVolume == 0)
            {
                if (_isFull)
                {
                    SetFull();
                }
                else
                {
                    TranslatingString();
                }

            }
            else
            {
                GetAward();
            }
        }

        public void Reset()
        {
            ResetLevel();
        }

        public void TryBuyImprovements()
        {
            if (_persistenProgressService.Progress.Wallet.Coins.Count >= _currentСostСonstruction)
            {
                SaveLoadService.SaveProgress();
                _upgradeButton.Button.interactable = false;
                _persistenProgressService.Progress.Wallet.Coins.Reduce(_currentСostСonstruction);
                UpgradeBuilding();
                GetCostConstruction();
                ShowNewUpgradePrice();
                LevelUp();
                _persistenProgressService.Progress.Achievements.AllValueImpruvementBuilding++;
                ImpruvementBuilding?.Invoke();
                NewUpgrageBuilding();
                TranslatingString();
                SetFull();
                MarketChanged?.Invoke();
            }
            else
            {
                _screenNoPay.gameObject.SetActive(true);
            }
            _upgradeButton.Button.interactable = true;
        }

        public void UpdataMarker()
        {
            MarketChanged?.Invoke();
        }

        public void TryPay()
        {
            if (_persistenProgressService.Progress.Wallet.Coins.Count >= _currentСostСonstruction)
            {
                SaveLoadService.SaveProgress();
                _persistenProgressService.Progress.Wallet.Coins.Reduce(_currentСostСonstruction);
                UpgradeBuilding();
                LevelUp();
                OpenUpgrade();
                GetCostConstruction();
                UpdataMarker();
                ShowNewUpgradePrice();
                TranslatingString();
                NewUpgrageBuilding();
            }
            else
            {
                _screenNoPay.gameObject.SetActive(true);
            }
            _upgradeButton.Button.interactable = true;
        }

        public int GetActiveMarker()
        {
            _allMarker = _improvementsMarker + _rewardMarker + _payMarker;
            return _allMarker;
        }

        public void PossiblePurchase(IPersistenProgressService progressService)
        {
            CurrentBattles = _mainScreen.BattleScene.FlightNumber.CurrentNumber;
            GetCostConstruction();
            if (IsOpen)
            {
                if(progressService.Progress.Wallet.Coins.Count >= _currentСostСonstruction)
                {
                    _textUpgrage.text = 1.ToString();
                    _improvementsMarker = 1;
                    _payMarker = 0;
                    if (_infoUpgrage.gameObject.activeInHierarchy == false)
                    {
                        _infoUpgrage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    _textUpgrage.text = 1.ToString();
                    _improvementsMarker = 0;
                    _payMarker = 0;
                    if (_infoUpgrage.gameObject.activeInHierarchy == true)
                    {
                        _infoUpgrage.gameObject.SetActive(false);
                    }
                }
            }
            else if(IsOpen == false)
            {
                if (_numberBattlesFought <= _currentBattles || _numberBattlesFought <= _persistenProgressService.Progress.OptionData.WonRaids)
                {
                    if (_constructionCastle != null && _constructionCastle.IsOpen || _constructionCastle == null)
                    {
                        if (progressService.Progress.Wallet.Coins.Count >= _currentСostСonstruction)
                        {
                            _payMarker = 1;
                            _textButtonBuild.text = 1.ToString();
                            if (_infoBattonBuild.gameObject.activeInHierarchy == false)
                            {
                                _infoBattonBuild.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            _payMarker = 0;
                            if (_infoBattonBuild.gameObject.activeInHierarchy == true)
                            {
                                _infoBattonBuild.gameObject.SetActive(false);
                                _textButtonBuild.text = 1.ToString();
                            }
                        }
                    }
                    else
                    {
                        _payMarker = 0;
                        if (_infoBattonBuild.gameObject.activeInHierarchy == true)
                        {
                            _infoBattonBuild.gameObject.SetActive(false);
                            _textButtonBuild.text = 1.ToString();
                        }
                    }
                }
                else
                {
                    _infoBattonBuild.gameObject.SetActive(false);
                    _textButtonBuild.text = 1.ToString();
                    _payMarker = 0;
                }
            }
        }

        protected void ResetLevel()
        {
            _level = 1;
            _elepsedTime = 0;
            _second = 0;
            CurrentBuilding();
            GetCostConstruction();
            SavaData(_convertedTime, _currentTimerTime);
        }

        protected void SetNumberBattles(int number)
        {
            string textResult;
            if (_isOpen == false)
            {
                if (_persistenProgressService == null)
                    _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
                if (_numberBattlesFought > _persistenProgressService.Progress.OptionData.WonRaids)
                {
                    var value = _numberBattlesFought - _persistenProgressService.Progress.OptionData.WonRaids;
                    _text.text = Text1 + value +" "+ Text2;
                    _textCastlBuild.gameObject.SetActive(false);
                }
                else
                {
                    if (_constructionCastle != null)
                    {
                        if (_constructionCastle.IsOpen)
                        {
                            _text.gameObject.SetActive(false);
                            _buttonBuild.gameObject.SetActive(true);
                            GetCostConstruction();
                            _costConstruction = MathF.Round(_costConstruction);
                            textResult = AbbreviationsNumbers.ShortNumber(_costConstruction);
                            _textPricePay.text = textResult;
                            _textCastlBuild.gameObject.SetActive(false);
                        }
                        else
                        {
                            _text.gameObject.SetActive(false);
                            _textCastlBuild.gameObject.SetActive(true);
                            _textCastlBuild.text = "Для открытия покупки требуеться купить " + _constructionCastle.Name.text;
                        }
                    }
                    else if (_constructionCastle == null)
                    {
                        _text.gameObject.SetActive(false);
                        _buttonBuild.gameObject.SetActive(true);
                        GetCostConstruction();
                        _costConstruction = MathF.Round(_costConstruction);
                        textResult = AbbreviationsNumbers.ShortNumber(_costConstruction);
                        _textPricePay.text = textResult;
                        _textCastlBuild.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (CurrencyType == CurrencyType.EpicCard)
                {
                    SetListTypeHeroCard(CardType.Epic);
                }
                else if(CurrencyType == CurrencyType.RareCard)
                {
                    SetListTypeHeroCard(CardType.Rare);
                }
                else if(CurrencyType == CurrencyType.RegularCard)
                {
                    SetListTypeHeroCard(CardType.Simple);
                }
            }
        }

        protected void GetCostConstruction()
        {
            _currentСostСonstruction = _passiveIncome.GetBuildingCost(this);
        }

        protected void ShowNewUpgradePrice()
        {
            _currentСostСonstruction = MathF.Round(_currentСostСonstruction);
            string resultText = AbbreviationsNumbers.ShortNumber(_currentСostСonstruction);
            _priceUpgrade.text = resultText;
        }

        protected void TranslatingString()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_elepsedTime);
            if(timeSpan.Hours != 0)
            {
                _hours = timeSpan.Hours.ToString();
                _minutes = timeSpan.Minutes.ToString();
                _seconds = timeSpan.Seconds.ToString();
                Show(_minutes, _seconds, _hours,_ch);
            }
            else if(timeSpan.Hours == 0)
            {
                _minutes = timeSpan.Minutes.ToString();
                _seconds = timeSpan.Seconds.ToString();
                Show(_minutes, _seconds);
            }
        }

        protected void SetFull()
        {
            _currentVolume = _countreward;
            if (_currentVolume < _maxVolume)
            {
                _isFull = false;
                _infoImageTakeIncome.gameObject.SetActive(false);
                SetTime();
                TranslatingString();
            }
            else
            {
                _currentVolume = _maxVolume;
                _rewardMarker = 1;
                _isFull = true;
                MarketChanged?.Invoke();
                _infoImageTakeIncome.gameObject.SetActive(true);
                _textTakeIncome.text = 1.ToString();
                ShowSliderText();
            }
        }

        protected void CalculateTime()
        {
            DateTime result = DateTime.ParseExact(_convertedTime,"u",CultureInfo.InvariantCulture);
            TimeSpan timePassed = DateTime.UtcNow - result;
            _second = (float)timePassed.TotalSeconds;
            _second = Math.Clamp(_second, 0,7 * 24 * 60 * 60);
            _elepsedTime = _currentTimerTime;
            CurrentBuilding();
            GetCostConstruction();
            _text.gameObject.SetActive(false);
            _buttonBuild.gameObject.SetActive(false);
            ButtonRewardEndupLevel();
            ShowNewUpgradePrice();
            NewUpgrageBuilding();
            LevelUp();
            CalculateTimeClosedWindow();
        }

        protected void AccrualTime()
        {
            _convertedTime = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
            _currentTimerTime = _elepsedTime;
            SavaData(_convertedTime, _currentTimerTime);
        }

        protected void SetCard(int value)
        {
            int cVolume = (int)_currentVolume;
            int count = Random.Range(1,cVolume + 1);
            int result =  _allHeroes.EnrollRandomHeroCard(true,0f, count, value,null,null,this);
            if (result >= 0)
            {
                _currentVolume -= count;
                _countreward -= count;
            }
        }

        protected void SetListTypeHeroCard(CardType type)
        {
            _soldierTypeCard.Clear();
            SetCardSelection(_allHeroes.AllHeroCards,type);

            if (_soldierTypeCard.Count > 0)
            {
                _buttonBuild.gameObject.SetActive(false);
                ButtonRewardEndupLevel();
            }
            else
            {
                _takeIncome.gameObject.SetActive(false);
                _buttonBuild.gameObject.SetActive(false);
                _upgradeButton.gameObject.SetActive(false);
                _text.gameObject.SetActive(false);
                _textCastlBuild.gameObject.SetActive(false);
                _allTextCard.gameObject.SetActive(true);
                _allTextCard.text = "Все карты собраны";
            }
        }

        private void SetCardSelection(List<Soldier> soldiers,CardType type)
        {
            foreach (var item in soldiers)
            {
                if (item.DataSoldier.Type == type)
                {
                    bool result = CountCardSoldier(item);
                    if(result)
                        _soldierTypeCard.Add(item);
                }
            }
        }

        private bool CountCardSoldier(Soldier soldier)
        {
            foreach (var item in _persistenProgressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if(soldier.HeroTypeId == item.TypeId)
                {
                    int cout = item.GetMaxCountCard();
                    if (cout > item.CurrentCountCard)
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        private void CurrentBuilding()
        {
            _countCurrency = _passiveIncome.GetPassiveIncome(this);
            _maxVolume = _passiveIncome.GetPassiveIncomeVolume(this);
        }

        private void UpgradeBuilding()
        {
            _level++;
            _countCurrency = _passiveIncome.GetPassiveIncome(this);
            _maxVolume = _passiveIncome.GetPassiveIncomeVolume(this);
        }

        private void NewUpgrageBuilding()
        {
            _level++;
            _newCurrency =_passiveIncome.GetPassiveIncome(this);
            _level--;
        }

        private void OpenUpgrade()
        {
            if (_isOpen == false)
            {
                SetTime();
                _isOpen = true;
                _persistenProgressService.Progress.Achievements.AllValueBuildBuildingAch++;
                OpenBuilding?.Invoke();
                _buttonBuild.gameObject.SetActive(false);
                ButtonRewardEndupLevel();
                _isCalculationsTime = true;
                TryBuilding?.Invoke();
            }
        }

        private void ButtonRewardEndupLevel()
        {
            _textLevel.gameObject.SetActive(true);
            _imageLevelIcon.gameObject.SetActive(true);
            _takeIncome.gameObject.SetActive(true);
            _upgradeButton.gameObject.SetActive(true);
        }

        private void Restart()
        {
            SetNumberBattles(_currentBattles);
        }

        private void LevelUp()
        {
            _textLevel.text =_level.ToString();
        }

        private void Show(string minutes, string seconds, string hours = null, string ch = null)
        {
            _currentVolume = MathF.Round(_currentVolume);
            string incom1 = AbbreviationsNumbers.ShortNumber(_currentVolume);
            _maxVolume = MathF.Round(_maxVolume);
            string incom2 = AbbreviationsNumbers.ShortNumber(_maxVolume);
            _textSliderTakeIncome.text = string.Format($"{incom1}/{incom2} ({hours}{ch}{minutes}:{seconds})");
            _slider.value = _currentVolume / _maxVolume;
        }

        private void ShowSliderText()
        {
            _currentVolume = MathF.Round( _currentVolume);
            _maxVolume = Mathf.Round( _maxVolume);
            string incom1 = AbbreviationsNumbers.ShortNumber(_currentVolume);
            string incom2 = AbbreviationsNumbers.ShortNumber(_maxVolume);
            _textSliderTakeIncome.text = incom1 + "/" + incom2;
            _slider.value = _currentVolume / _maxVolume;
        }

        private void SetTime()
        {
            _elepsedTime = _generationTime;
        }

        private void CalculateTimeClosedWindow()
        {
            if (_isFull == false)
            {
                SatrTimeAbsence();
            }
            _isCalculationsTime = true;
        }

        private void SatrTimeAbsence()
        {
            while (_second != 0)
            {
                if (_second >= _elepsedTime)
                {
                    _second -= _elepsedTime;
                    _elepsedTime = 0;
                    if (_elepsedTime == 0)
                    {
                        _elepsedTime = _generationTime;
                        _currentVolume += _countCurrency;
                        _countreward += _countCurrency;
                        if (_currentVolume == _maxVolume)
                        {
                            ShowSliderText();
                            _isFull = true;
                            _infoImageTakeIncome.gameObject.SetActive(true);
                            _textTakeIncome.text = 1.ToString();
                            _rewardMarker = 1;
                            MarketChanged?.Invoke();
                            _second = 0;
                        }
                    }
                }
                else
                {
                    _elepsedTime -= _second;
                    _second = 0;
                }
            }
        }
    }
}
