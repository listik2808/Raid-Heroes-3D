using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.StaticData;
using Source.Scripts.Logic;
using System;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public abstract class UpgradeData : MonoBehaviour
    {
        public const string Sing = ">";
        [SerializeField] protected ScreenSkill ScreenSkill;
        [SerializeField] protected ScreenUprageSoldier _screenUprageSoldier;
        private IPersistenProgressService _progressService;
        protected float _powerSoldier;
        protected int _currentLevelSpecealSkill =1;
        protected int _currentSurvivabilityLevel =1;
        protected int _currentMelleDamageLevel = 1;
        //protected int _currentSpeedLevel = 1;
        protected int _maxLevelSpecealSkill;
        protected int _maxSurvivabilityLevel;
        protected int _maxMelleDamageLevel;
        //protected int _maxSpeedLevel;
        protected Soldier _soldier;
        protected SoldierCardViewer SoldierCard;
        protected CameraParent CameraParent;
        protected int CurrentIdex = 0;

        private int _allUpRangMarker = 0;

        private int _allMarkerSpecAttak = 0;
        private int _upgrageCoinsSpecAttac = 0;
        private int _upgrageDiamondSpecAtack = 0;

        private int _allMarkerSurvability = 0;
        private int _upgrageCoinsSurvability = 0;
        private int _upgrageDiamondSurvability = 0;

        private int _allMarkerMeleeDamage = 0;
        private int _upgrageCoinsMeleeDamage = 0;
        private int _upgrageDiamondMeleedamage = 0;

        public SoldierCardViewer SoldierCardViewer => SoldierCard;
        public int AllMarkerMeleeDamage => _allMarkerMeleeDamage;
        public int AllMarkerSurvability => _allMarkerSurvability;
        public int AllUpRangMarker => _allUpRangMarker;
        public int AllMarkerSpecAttak => _allMarkerSpecAttak;
        public ScreenUprageSoldier ScreenUprageSoldier => _screenUprageSoldier;

        public event Action ChangedCharacteristics;
        public event Action Restart;

        private void OnEnable()
        {
            _screenUprageSoldier.ButtonMultiplier.LoadMeltiplier += SetPriceMultiplier;
            _screenUprageSoldier.ButtonMultiplier.Button.onClick.AddListener(SetPriceMultiplier);
            ScreenSkill.ButtonSurwabilityIcon.Button.onClick.AddListener(SetTextSurwability);
            ScreenSkill.ButtonSurwabilityBackground.Button.onClick.AddListener(SetTextSurwability);
            ScreenSkill.ButtonMeleeDamageIcon.Button.onClick.AddListener(SetTextMeleeDamage);
            ScreenSkill.ButtonMeleeDamageBackground.Button.onClick.AddListener(SetTextMeleeDamage);
            ScreenSkill.ButtonSpecialSkillIcon.Button.onClick.AddListener(SetTextSpecialSkill);
            ScreenSkill.ButtonSpecialSkillBackground.Button.onClick.AddListener(SetTextSpecialSkill);
            ScreenSkill.ButtonLeft.Button.onClick.AddListener(SwitchingSkillLeft);
            ScreenSkill.ButtonRight.Button.onClick.AddListener(SwitchingSkillRight);
            _screenUprageSoldier.RangUI.UpRank.ButtonOnClic += UpRank;
            _screenUprageSoldier.RemuveSoldierButton.ButtonOnClic += RemuveSoldierSquad;
            _screenUprageSoldier.AddSoldierButton.ButtonOnClic += AddSoldierSquad;
            UpgradeSubscription();
            SubscribeEvolution();
            _screenUprageSoldier.Squad.Construct(this);
            _screenUprageSoldier.CanvasGroup.alpha = 0;
        }

        private void OnDisable()
        {
            _screenUprageSoldier.ButtonMultiplier.LoadMeltiplier -= SetPriceMultiplier;
            _screenUprageSoldier.MakeAnyImprovementsHeroAchievement.SetUpSkill();
            _screenUprageSoldier.MakeEvolutionHeroesAchievement.SetUpEvolutionSkill();
            _screenUprageSoldier.UpRankAchievement.SetUpRank();
            _screenUprageSoldier.ButtonMultiplier.Button.onClick.RemoveListener(SetPriceMultiplier);
            ScreenSkill.ButtonSurwabilityIcon.Button.onClick.RemoveListener(SetTextSurwability);
            ScreenSkill.ButtonSurwabilityBackground.Button.onClick.RemoveListener(SetTextSurwability);
            ScreenSkill.ButtonMeleeDamageIcon.Button.onClick.RemoveListener(SetTextMeleeDamage);
            ScreenSkill.ButtonMeleeDamageBackground.Button.onClick.RemoveListener(SetTextMeleeDamage);
            ScreenSkill.ButtonSpecialSkillIcon.Button.onClick.RemoveListener(SetTextSpecialSkill);
            ScreenSkill.ButtonSpecialSkillBackground.Button.onClick.RemoveListener(SetTextSpecialSkill);
            ScreenSkill.ButtonLeft.Button.onClick.RemoveListener(SwitchingSkillLeft);
            ScreenSkill.ButtonRight.Button.onClick.RemoveListener(SwitchingSkillRight);
            _screenUprageSoldier.RangUI.UpRank.ButtonOnClic -= UpRank;
            _screenUprageSoldier.RemuveSoldierButton.ButtonOnClic -= RemuveSoldierSquad;
            _screenUprageSoldier.AddSoldierButton.ButtonOnClic -= AddSoldierSquad;
            UnsubscribeUpdate();
            EvolutionUnsubscribing();
            _screenUprageSoldier.Squad.Close();
        }

        public void CloseScreen()
        {
            _screenUprageSoldier.CanvasGroup.alpha = 1;
            _screenUprageSoldier.gameObject.SetActive(false);
        }
        public abstract void SetTextSpecialSkill();
        public abstract void SetTextSurwability();
        public abstract void SetTextMeleeDamage();
        public abstract void FillSpecialSkill(float newSpecialDamage ,string sing = null);
        public abstract void FillSpecialSkill();
        public abstract void SetSoldier(CameraParent cameraParent);

        public void SetCard(SoldierCardViewer card)
        {
            SoldierCard = card;
        }
        public void SwitchingSkillLeft()
        {
            if (CurrentIdex - 1 >= 1)
            {
                CurrentIdex--;
                ScreenSelection(CurrentIdex);
            }
            else
            {
                CurrentIdex = 3;
                ScreenSelection(CurrentIdex);
            }
        }

        public void SwitchingSkillRight()
        {
            if (CurrentIdex + 1 <= 3)
            {
                CurrentIdex++;
                ScreenSelection(CurrentIdex);
            }
            else
            {
                CurrentIdex = 1;
                ScreenSelection(CurrentIdex);
            }
        }

        public void SetDataSoldier(CameraParent cameraParent,IPersistenProgressService progressService)
        {
            _screenUprageSoldier.RangUI.ActivateCamera(cameraParent);
            SetValuesCharacteristics(cameraParent, progressService);
            
            _screenUprageSoldier.gameObject.SetActive(true);
        }

        public void SetValuesCharacteristics(CameraParent cameraParent,IPersistenProgressService persistenProgressService)
        {
            if(_progressService == null)
            {
                _progressService = persistenProgressService;
            }
            _screenUprageSoldier.SetSoldier(_soldier);
            _screenUprageSoldier.AddSoldierSquad();
            CameraParent = cameraParent;
            _screenUprageSoldier.RangUI.SetTextureHero(cameraParent);
            ButtonAddAndRemuveSoldier();
            FillRank();
            _allUpRangMarker = _screenUprageSoldier.RangUI.UpRangMarker;
            SetMaxLevelSkill();
            SetIconSpecialSkill(_soldier.IconSpecAttack);
            SetSpecialAttackSoldier(_soldier.SpecialAttack);
            if (_soldier.DataSoldier.Hired == false)
            {
                _screenUprageSoldier.RangUI.PowerText.gameObject.SetActive(false);
                DisbleButtonUpgrade();
                DisableSlidier();
                FillSpecialSkill();
                FillSurvivability(_soldier.CurrentHealth);
                SetMeleeDamageData(_soldier.CurrentMeleeDamage);
                //SetSpeedSkill(_soldier.CurrentSpeed);
                if (_soldier.Rank.CurrentCountCard < _soldier.Rank.MaxCountCard)
                {
                    //_screenUprageSoldier.RangUI.TextHiringCards.gameObject.SetActive(true);
                    _screenUprageSoldier.RangUI.ShowCountCardHiring();
                }
                else
                {
                    _screenUprageSoldier.RangUI.TextHiringCards.gameObject.SetActive(false);
                }
            }
            else
            {
                _screenUprageSoldier.RangUI.TextHiringCards.gameObject.SetActive(false);
                _screenUprageSoldier.RangUI.Container.SetActive(true);
                ActivateButtonUpgrade();
                ActivateSlidier();
                _soldier.SpecialSkillUpgrade();
                SetPriceMultiplier();
                _allMarkerSpecAttak = TrySetMarkerSpecSkillCoin();
                _allMarkerSurvability = TrySetMarkerSurvivability();
                _allMarkerMeleeDamage = TrySetMarkerMeleeDamage();
                RenderingLevelEvolutionSpecialSkill();
                RenderingLevelEvolutionSurvivability();
                RenderingLevelEvolutionMeleeDamage();
                PoverSoldierShow();
            }
        }

        private void SetPriceMultiplier()
        {
            if (_soldier.UnitOpened)
            {
                TrySetNewSkillSpecSkill();
                TrySetNewSurvabilitySkill();
                TrySetNewMelleDamageSkill();
            }
        }

        private void TrySetNewMelleDamageSkill()
        {
            if (_soldier.SoldiersStatsLevel.CurrentMeleelevel == _maxMelleDamageLevel && _soldier.MeleeDamageLevelData.CurrentStepSkill == _soldier.MeleeDamageLevelData.MaxStepValue)
            {
                SetMeleeDamageData(_soldier.CurrentMeleeDamage);
            }
            else
            {
                RenderingSliderMeleeDamage(_screenUprageSoldier.ButtonMultiplier);
                if (_screenUprageSoldier.ButtonMultiplier.IsMax == false && _screenUprageSoldier.ButtonMultiplier.IsTen == false)
                {
                    if(_soldier.MeleeDamageLevelData.CurrentStepSkill < _soldier.MeleeDamageLevelData.MaxStepValue)
                    {
                        _soldier.SetNewMeleeDamageMultiplier(_soldier.SoldiersStatsLevel.CurrentMeleelevel, _screenUprageSoldier.CurerntStepMeleeDamage);
                    }
                    else if(_soldier.SoldiersStatsLevel.CurrentMeleelevel < _soldier.SoldiersStatsLevel.MaxLevelStatsHero)
                    {
                        _soldier.SetNewMeleeDamageMultiplier(_soldier.SoldiersStatsLevel.CurrentMeleelevel +1, _screenUprageSoldier.CurerntStepMeleeDamage);
                    }
                }
                else
                {
                    _soldier.SetNewMeleeDamageMultiplier(_screenUprageSoldier.LevelMeleeDamageSkill, _screenUprageSoldier.CurerntStepMeleeDamage);
                }

                SetMeleeDamageData(_soldier.CurrentMeleeDamage, _soldier.NewMeleeDamage, Sing);
            }
        }

        private void TrySetNewSurvabilitySkill()
        {
            if (_soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel == _maxSurvivabilityLevel && _soldier.SurvivabilityLevelData.CurrentStepSkill == _soldier.SurvivabilityLevelData.MaxStepValue)
            {
                FillSurvivability(_soldier.CurrentHealth);
            }
            else
            {
                RenderingSliderSurvivability(_screenUprageSoldier.ButtonMultiplier);
                if (_screenUprageSoldier.ButtonMultiplier.IsMax == false && _screenUprageSoldier.ButtonMultiplier.IsTen == false)
                {
                    if (_soldier.SurvivabilityLevelData.CurrentStepSkill < _soldier.SurvivabilityLevelData.MaxStepValue)
                    {
                        _soldier.SetNewHpMultiplier(_soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel, _screenUprageSoldier.CurrentStepHp);
                    }
                    else if(_soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel < _soldier.SoldiersStatsLevel.MaxLevelStatsHero)
                    {
                        _soldier.SetNewHpMultiplier(_soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel +1, _screenUprageSoldier.CurrentStepHp);
                    }
                    
                }
                else
                {
                    _soldier.SetNewHpMultiplier(_screenUprageSoldier.LevelHpSkill, _screenUprageSoldier.CurrentStepHp);
                }

                FillSurvivability(_soldier.CurrentHealth, _soldier.NewHealth, Sing);
            }
        }

        private void TrySetNewSkillSpecSkill()
        {
            if (_soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill == _maxLevelSpecealSkill && _soldier.SpecialSkillLevelData.CurrentStepSkill == _soldier.SpecialSkillLevelData.MaxStepValue)
            {
                FillSpecialSkill();
            }
            else
            {
                RenderingSliderSpecialSkill(_screenUprageSoldier.ButtonMultiplier);
                if (_screenUprageSoldier.ButtonMultiplier.IsMax == false && _screenUprageSoldier.ButtonMultiplier.IsTen == false)
                {
                    if(_soldier.SpecialSkillLevelData.CurrentStepSkill < _soldier.SpecialSkillLevelData.MaxStepValue)
                    {
                        _soldier.SetNewSkillValueXXX(_soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill, _screenUprageSoldier.CurrentStepSpecSkill, _screenUprageSoldier.Soldier.SpecialSkillLevelData.MaxStepValue);
                    }
                    else if(_soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill < _soldier.SoldiersStatsLevel.MaxLevelStatsHero)
                    {
                        _soldier.SetNewSkillValueXXX(_soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill+1, _screenUprageSoldier.CurrentStepSpecSkill, _screenUprageSoldier.Soldier.SpecialSkillLevelData.MaxStepValue);
                    }
                }
                else
                {
                    _soldier.SetNewSkillValueXXX(_screenUprageSoldier.LevelSpecSkill, _screenUprageSoldier.CurrentStepSpecSkill, _screenUprageSoldier.Soldier.SpecialSkillLevelData.MaxStepValue);
                }

                FillSpecialSkill(_soldier.NewSpecialDamage, Sing);
            }
        }

        protected void SetIconSpecialSkill(Sprite sprite)
        {
            _screenUprageSoldier.SpecialSkill.SetSpecialSkillIcon(sprite);
        }

        protected void SetSpecialAttackSoldier(SpecialAttack specialAttack)
        {
            _screenUprageSoldier.SpecialSkill.SetTypeAttack(specialAttack);
        }

        protected void SetMaxLevelSkill()
        {
            _maxLevelSpecealSkill = _soldier.Rank.MaxLevelSkill;
            _maxMelleDamageLevel = _soldier.Rank.MaxLevelSkill;
            _maxSurvivabilityLevel = _soldier.Rank.MaxLevelSkill;
            //_maxSpeedLevel = _soldier.Rank.MaxLevelSkill;
        }

        protected void FillRank()
        {
            _screenUprageSoldier.RangUI.SetPositionSquad(_soldier.DataSoldier.InSquad,_screenUprageSoldier.SoldierHero.Count);
            _screenUprageSoldier.RangUI.SetRang(_soldier);
            _screenUprageSoldier.RangUI.SetPrice(_soldier,_progressService);
            _screenUprageSoldier.RangUI.SetСard();
            foreach (SoldierCard item in _screenUprageSoldier.Squad.SoldierСards)
            {
                item.ShowCountCard();
            }
        }

        private void SetStepOrLevelSpecSkill(float valueStep, Soldier soldier)
        {
            float currentValueSpecSkill = valueStep;
            while(currentValueSpecSkill > 0)
            {
                if(soldier.SpecialSkillLevelData.CurrentStepSkill < soldier.SpecialSkillLevelData.MaxStepValue)
                {
                    soldier.SpecialSkillLevelData.AddStep();
                    _soldier.SpecialSkillUpgrade();
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                    currentValueSpecSkill--;
                }
                else if (_currentLevelSpecealSkill < _soldier.SoldiersStatsLevel.BestLevelSpecialSkill)
                {
                    SpecialSkillEvolution();
                    //currentValueSpecSkill--;
                }
            }
        }

        private void SetStepOrLevelHpSkill(float valueStep, Soldier soldier)
        {
            float currentValueHpSkill = valueStep;

            while (currentValueHpSkill > 0)
            {
                if (soldier.SurvivabilityLevelData.CurrentStepSkill < soldier.SurvivabilityLevelData.MaxStepValue)
                {
                    soldier.SurvivabilityLevelData.AddStep();
                    _soldier.SurvivabilityLevelData.SkillSurvivabikityUpgrade(_soldier);
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                    currentValueHpSkill--;
                }
                else if (_currentSurvivabilityLevel < _soldier.SoldiersStatsLevel.BestSurvivalRateLevel)
                {
                    SurvivabilityEvolution();
                    //currentValueHpSkill--;
                }
            }
        }

        private void SetStepOrLevelMelleDamageSkill(float valueStep, Soldier soldier)
        {
            float currentValueMeleeDamageSkill = valueStep;

            while (currentValueMeleeDamageSkill > 0)
            {
                if (soldier.MeleeDamageLevelData.CurrentStepSkill < soldier.MeleeDamageLevelData.MaxStepValue)
                {
                    soldier.MeleeDamageLevelData.AddStep();
                    _soldier.MeleeDamageLevelData.SkillMeleeDamageUpgrade(_soldier);
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                    currentValueMeleeDamageSkill--;
                }
                else if (_currentMelleDamageLevel < _soldier.SoldiersStatsLevel.BestMeleerateLevel)
                {
                    MelleDamageSkillEvolution();
                   // currentValueMeleeDamageSkill--;
                }
            }
        }

        private int TrySetMarkerMeleeDamage()
        {
            if (_soldier.MeleeDamageLevelData.CurrentStepSkill < _soldier.MeleeDamageLevelData.MaxStepValue)
            {
                if (_progressService.Progress.Wallet.Coins.Count > 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.MeleeDamage.CurrentCoinPrice)
                {
                    _upgrageCoinsMeleeDamage = 1;
                    _upgrageDiamondMeleedamage = 0;
                    _screenUprageSoldier.MeleeDamage.ActivateMarkerCoinImpruvment(1);
                    _screenUprageSoldier.MeleeDamage.DiactivateMarkerDiamondEvolution();
                }
                else
                {
                    ZeroMeleeDamage();
                }
            }
            else if (_soldier.MeleeDamageLevelData.CurrentStepSkill == _soldier.MeleeDamageLevelData.MaxStepValue && _currentMelleDamageLevel == _maxMelleDamageLevel)
            {
                ZeroMeleeDamage();
            }
            else if (_soldier.MeleeDamageLevelData.CurrentStepSkill == _soldier.MeleeDamageLevelData.MaxStepValue && _currentMelleDamageLevel != _maxMelleDamageLevel)
            {
                var value = Price.GetUpgradeCostDiamonds(_currentMelleDamageLevel, _soldier.SoldiersStatsLevel.BestMeleerateLevel);
                if (value > 0 && _progressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.MeleeDamage.CurrentDiamondPrice)
                {
                    _upgrageCoinsMeleeDamage = 0;
                    _upgrageDiamondMeleedamage = 1;
                    _screenUprageSoldier.MeleeDamage.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.MeleeDamage.DiactivateMarkerCoinImpruvment();
                }
                else if (value <= 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.MeleeDamage.CurrentCoinPrice)
                {
                    _upgrageCoinsMeleeDamage = 0;
                    _upgrageDiamondMeleedamage = 1;
                    _screenUprageSoldier.MeleeDamage.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.MeleeDamage.DiactivateMarkerCoinImpruvment();
                }
                else
                {
                    ZeroMeleeDamage();
                }
            }
            return _upgrageCoinsMeleeDamage + _upgrageDiamondMeleedamage;
        }

        private int TrySetMarkerSurvivability()
        {
            if (_soldier.SurvivabilityLevelData.CurrentStepSkill < _soldier.SurvivabilityLevelData.MaxStepValue)
            {
                if (_progressService.Progress.Wallet.Coins.Count > 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.Survivability.CurrentCoinPrice)
                {
                    _upgrageCoinsSurvability = 1;
                    _upgrageDiamondSurvability = 0;
                    _screenUprageSoldier.Survivability.ActivateMarkerCoinImpruvment(1);
                    _screenUprageSoldier.Survivability.DiactivateMarkerDiamondEvolution();
                }
                else
                {
                    ZoroSurvability();
                }
            }
            else if (_soldier.SurvivabilityLevelData.CurrentStepSkill == _soldier.SurvivabilityLevelData.MaxStepValue && _currentSurvivabilityLevel == _maxSurvivabilityLevel)
            {
                ZoroSurvability();
            }
            else if (_soldier.SurvivabilityLevelData.CurrentStepSkill == _soldier.SurvivabilityLevelData.MaxStepValue && _currentSurvivabilityLevel != _maxSurvivabilityLevel)
            {
                var value = Price.GetUpgradeCostDiamonds(_currentSurvivabilityLevel, _soldier.SoldiersStatsLevel.BestSurvivalRateLevel);
                if (value > 0 && _progressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.Survivability.CurrentDiamondPrice)
                {
                    _upgrageCoinsSurvability = 0;
                    _upgrageDiamondSurvability = 1;
                    _screenUprageSoldier.Survivability.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.Survivability.DiactivateMarkerCoinImpruvment();
                }
                else if (value <= 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.Survivability.CurrentCoinPrice)
                {
                    _upgrageCoinsSurvability = 0;
                    _upgrageDiamondSurvability = 1;
                    _screenUprageSoldier.Survivability.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.Survivability.DiactivateMarkerCoinImpruvment();
                }
                else
                {
                    ZoroSurvability();
                }
            }
            return _upgrageCoinsSurvability + _upgrageDiamondSurvability;
        }

        private int TrySetMarkerSpecSkillCoin()
        {
            if(_soldier.SpecialSkillLevelData.CurrentStepSkill < _soldier.SpecialSkillLevelData.MaxStepValue)
            {
                if (_progressService.Progress.Wallet.Coins.Count > 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.SpecialSkill.CurrentCoinPrice)
                {
                    _upgrageCoinsSpecAttac = 1;
                    _upgrageDiamondSpecAtack = 0;
                    _screenUprageSoldier.SpecialSkill.ActivateMarkerCoinImpruvment(1);
                    _screenUprageSoldier.SpecialSkill.DiactivateMarkerDiamondEvolution();
                }
                else
                {
                    ZeroSpecialSkill();
                }
            }
            else if(_soldier.SpecialSkillLevelData.CurrentStepSkill == _soldier.SpecialSkillLevelData.MaxStepValue && _currentLevelSpecealSkill == _maxLevelSpecealSkill)
            {
                ZeroSpecialSkill();
            }
            else if(_soldier.SpecialSkillLevelData.CurrentStepSkill == _soldier.SpecialSkillLevelData.MaxStepValue && _currentLevelSpecealSkill != _maxLevelSpecealSkill)
            {
                var value = Price.GetUpgradeCostDiamonds(_currentLevelSpecealSkill, _soldier.SoldiersStatsLevel.BestLevelSpecialSkill);
                if (value > 0 && _progressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.SpecialSkill.CurrentDiamondPrice)
                {
                    _upgrageCoinsSpecAttac = 0;
                    _upgrageDiamondSpecAtack = 1;
                    _screenUprageSoldier.SpecialSkill.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.SpecialSkill.DiactivateMarkerCoinImpruvment();
                }
                else if (value <= 0 && _progressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.SpecialSkill.CurrentCoinPrice)
                {
                    _upgrageCoinsSpecAttac = 0;
                    _upgrageDiamondSpecAtack = 1;
                    _screenUprageSoldier.SpecialSkill.ActivateMarkerDiamondEvolution(1);
                    _screenUprageSoldier.SpecialSkill.DiactivateMarkerCoinImpruvment();
                }
                else
                {
                    ZeroSpecialSkill();
                }
            }
            return _upgrageCoinsSpecAttac + _upgrageDiamondSpecAtack;
        }

        private void ZeroSpecialSkill()
        {
            _upgrageCoinsSpecAttac = 0;
            _upgrageDiamondSpecAtack = 0;
            _screenUprageSoldier.SpecialSkill.DiactivateMarkerCoinImpruvment();
            _screenUprageSoldier.SpecialSkill.DiactivateMarkerDiamondEvolution();
        }

        private void ZoroSurvability()
        {
            _upgrageCoinsSurvability = 0;
            _upgrageDiamondSurvability = 0;
            _screenUprageSoldier.Survivability.DiactivateMarkerCoinImpruvment();
            _screenUprageSoldier.Survivability.DiactivateMarkerDiamondEvolution();
        }

        private void ZeroMeleeDamage()
        {
            _upgrageCoinsMeleeDamage = 0;
            _upgrageDiamondMeleedamage = 0;
            _screenUprageSoldier.MeleeDamage.DiactivateMarkerCoinImpruvment();
            _screenUprageSoldier.MeleeDamage.DiactivateMarkerDiamondEvolution();
        }

        protected void SetFillingBarSpecialSkill()
        {
            if(_screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.SpecialSkill.CurrentCoinPrice)
            {
                if(_screenUprageSoldier.ButtonMultiplier.IsMax || _screenUprageSoldier.ButtonMultiplier.IsTen)
                {
                    SetStepOrLevelSpecSkill(_screenUprageSoldier.AllStep, _screenUprageSoldier.Soldier);
                }
                else
                {
                    _soldier.SpecialSkillLevelData.AddStep();
                    _soldier.SpecialSkillUpgrade();
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                }
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.SpecialSkill.CurrentCoinPrice);
                RenderingSliderSpecialSkill(_screenUprageSoldier.ButtonMultiplier);
                if (_soldier.SpecialSkillLevelData.CurrentStepSkill == _soldier.SpecialSkillLevelData.MaxStepValue && _currentLevelSpecealSkill == _maxLevelSpecealSkill)
                    FillSpecialSkill();
                else
                {
                    if (_screenUprageSoldier.AllStep > 1)
                    {
                        SetPriceMultiplier();
                    }
                    else
                    {
                        FillSpecialSkill(_soldier.NewSpecialDamage, Sing);
                    }
                }
                _allMarkerSpecAttak = TrySetMarkerSpecSkillCoin();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        protected void SetFillingBarSurvivability()
        {
            if(_screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.Survivability.CurrentCoinPrice)
            {
                if (_screenUprageSoldier.ButtonMultiplier.IsMax || _screenUprageSoldier.ButtonMultiplier.IsTen)
                {
                    SetStepOrLevelHpSkill(_screenUprageSoldier.AllStepHp, _screenUprageSoldier.Soldier);
                }
                else
                {
                    _soldier.SurvivabilityLevelData.AddStep();
                    _soldier.SurvivabilityLevelData.SkillSurvivabikityUpgrade(_soldier);
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                }
                
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.Survivability.CurrentCoinPrice);
                RenderingSliderSurvivability(_screenUprageSoldier.ButtonMultiplier);
                if (_soldier.SurvivabilityLevelData.CurrentStepSkill == _maxSurvivabilityLevel && _currentSurvivabilityLevel == _maxSurvivabilityLevel)
                    FillSurvivability(_soldier.CurrentHealth);
                else
                {
                    if (_screenUprageSoldier.AllStepHp > 1)
                    {
                        SetPriceMultiplier();
                    }
                    else
                    {
                        FillSurvivability(_soldier.CurrentHealth, _soldier.NewHealth, Sing);
                    }
                }
                _allMarkerSurvability = TrySetMarkerSurvivability();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        protected void SetFillingBarMeleeDamage()
        {
            if(_screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.MeleeDamage.CurrentCoinPrice)
            {
                if (_screenUprageSoldier.ButtonMultiplier.IsMax || _screenUprageSoldier.ButtonMultiplier.IsTen)
                {
                    SetStepOrLevelMelleDamageSkill(_screenUprageSoldier.AllStepMeleeDamage, _screenUprageSoldier.Soldier);
                }
                else
                {
                    _soldier.MeleeDamageLevelData.AddStep();
                    _soldier.MeleeDamageLevelData.SkillMeleeDamageUpgrade(_soldier);
                    _progressService.Progress.Achievements.AllUpHeroSkill++;
                    ChangedCharacteristics?.Invoke();
                    PoverSoldierShow();
                }
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.MeleeDamage.CurrentCoinPrice);
                RenderingSliderMeleeDamage(_screenUprageSoldier.ButtonMultiplier);
                if (_soldier.MeleeDamageLevelData.CurrentStepSkill == _maxMelleDamageLevel && _currentMelleDamageLevel == _maxMelleDamageLevel)
                    SetMeleeDamageData(_soldier.CurrentMeleeDamage);
                else
                {
                    if (_screenUprageSoldier.AllStepMeleeDamage > 1)
                    {
                        SetPriceMultiplier();
                    }
                    else
                    {
                        SetMeleeDamageData(_soldier.CurrentMeleeDamage, _soldier.NewMeleeDamage, Sing);
                    }
                }
                _allMarkerMeleeDamage = TrySetMarkerMeleeDamage();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        //protected void SetFillingBarSpeedSkill()
        //{
        //    if(_screenUprageSoldier.Wallet.Coins.Count >= _screenUprageSoldier.SpeedSkill.CurrentCoinPrice)
        //    {
        //        _screenUprageSoldier.Wallet.Coins.Reduce(_screenUprageSoldier.SpeedSkill.CurrentCoinPrice);

        //        _soldier.SpeedSkillLevelData.AddStep();
        //        _soldier.SpeedSkillLevelData.SkillSpeedUpgrade(_soldier);
        //        ChangedCharacteristics?.Invoke();
        //        PoverSoldierShow();
        //        RenderingSliderSpeedSkill();
        //        if (_soldier.SpeedSkillLevelData.CurrentStepSkill == _maxSpeedLevel && _currentSpeedLevel == _maxSpeedLevel)
        //            SetSpeedSkill(_soldier.CurrentSpeed);
        //        else
        //            SetSpeedSkill(_soldier.CurrentSpeed, _soldier.NewSpeed, Sing);
        //    }
        //    else
        //    {
        //        _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
        //    }
        //}

        //protected void UpEvolutionSpeedSkill()
        //{
        //    var value = Price.GetUpgradeCostDiamonds(_currentSpeedLevel, _soldier.SoldiersStatsLevel.BestLevelMobility);
        //    if (value > 0 && _screenUprageSoldier.Wallet.Diamonds.Count >= _screenUprageSoldier.SpeedSkill.CurrentDiamondPrice)
        //    {
        //        _screenUprageSoldier.Wallet.Diamonds.Reduce(_screenUprageSoldier.SpeedSkill.CurrentDiamondPrice);
        //        SpeedSkillEvolution();
        //    }
        //    else if (value <= 0 && _screenUprageSoldier.Wallet.Coins.Count >= _screenUprageSoldier.SpeedSkill.CurrentCoinPrice)
        //    {
        //        _screenUprageSoldier.Wallet.Coins.Reduce(_screenUprageSoldier.SpeedSkill.CurrentCoinPrice);
        //        SpeedSkillEvolution();
        //    }
        //    else
        //    {
        //        _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
        //    }
        //}

        protected void UpEvolutionMelleDamageSkill()
        {
            var value = Price.GetUpgradeCostDiamonds(_currentMelleDamageLevel, _soldier.SoldiersStatsLevel.BestMeleerateLevel);
            if(value > 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.MeleeDamage.CurrentDiamondPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Reduce(_screenUprageSoldier.MeleeDamage.CurrentDiamondPrice);
                MelleDamageSkillEvolution();
            }
            else if(value <= 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.MeleeDamage.CurrentCoinPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.MeleeDamage.CurrentCoinPrice);
                MelleDamageSkillEvolution();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        protected void UpEvolutionSurvivabilitySkill()
        {
            var value = Price.GetUpgradeCostDiamonds(_currentSurvivabilityLevel, _soldier.SoldiersStatsLevel.BestSurvivalRateLevel);
            if(value > 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.Survivability.CurrentDiamondPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Reduce(_screenUprageSoldier.Survivability.CurrentDiamondPrice);
                SurvivabilityEvolution();
            }
            else if(value <= 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.Survivability.CurrentCoinPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.Survivability.CurrentCoinPrice);
                SurvivabilityEvolution();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        protected void UpEvolutionSpecialSkill()
        {
            var value = Price.GetUpgradeCostDiamonds(_currentLevelSpecealSkill, _soldier.SoldiersStatsLevel.BestLevelSpecialSkill);
            if (value > 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Count >= _screenUprageSoldier.SpecialSkill.CurrentDiamondPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Diamonds.Reduce(_screenUprageSoldier.SpecialSkill.CurrentDiamondPrice);
                SpecialSkillEvolution();
            }
            else if(value <= 0 && _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Count >= _screenUprageSoldier.SpecialSkill.CurrentCoinPrice)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Coins.Reduce(_screenUprageSoldier.SpecialSkill.CurrentCoinPrice);
                SpecialSkillEvolution();
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        protected void FillSurvivability(float currentHeath, float newHealth,string sing = null)
        {
            _screenUprageSoldier.Survivability.FillSurvivabilityIndicators(currentHeath,newHealth,sing);
        }
        protected void FillSurvivability(float currentHeath)
        {
            _screenUprageSoldier.Survivability.FillSurvivabilityIndicators(currentHeath);
        }

        protected void SetMeleeDamageData(float currentMeleeDamage, float newMeleeDamage, string sing = null)
        {
            _screenUprageSoldier.MeleeDamage.FullMeleeDamageIndicator(currentMeleeDamage,newMeleeDamage,sing);
        }
        protected void SetMeleeDamageData(float currentMeleeDamage)
        {
            _screenUprageSoldier.MeleeDamage.FullMeleeDamageIndicator(currentMeleeDamage);
        }

        //protected void SetSpeedSkill(float currentSpeed, float newSpeed,string sing = null)
        //{
        //    _screenUprageSoldier.SpeedSkill.SetSkillIndicatorsSpeed(currentSpeed,newSpeed,sing);
        //}

        //protected void SetSpeedSkill(float currentSpeed)
        //{
        //    _screenUprageSoldier.SpeedSkill.SetSkillIndicatorsSpeed(currentSpeed);
        //}

        protected void RenderingSliderSpecialSkill(ButtonMultiplier buttonMultiplier)
        {
            _screenUprageSoldier.FillingBarSpecialSkill(_soldier.SpecialSkillLevelData.CurrentStepSkill, _soldier.Rank.MaxStepSkill,_maxLevelSpecealSkill,_currentLevelSpecealSkill,_soldier.SoldiersStatsLevel.BestLevelSpecialSkill, buttonMultiplier);
        }

        protected void RenderingSliderSurvivability(ButtonMultiplier buttonMultiplier)
        {
            _screenUprageSoldier.FillingBarSurvivability(_soldier.SurvivabilityLevelData.CurrentStepSkill, _soldier.Rank.MaxStepSkill, _maxSurvivabilityLevel,_currentSurvivabilityLevel,_soldier.SoldiersStatsLevel.BestSurvivalRateLevel, buttonMultiplier);
        }

        protected void RenderingSliderMeleeDamage(ButtonMultiplier buttonMultiplier)
        {
            _screenUprageSoldier.FillingBarMeleeDamage(_soldier.MeleeDamageLevelData.CurrentStepSkill, _soldier.Rank.MaxStepSkill, _maxMelleDamageLevel,_currentMelleDamageLevel, _soldier.SoldiersStatsLevel.BestMeleerateLevel, buttonMultiplier);
        }

        //protected void RenderingSliderSpeedSkill()
        //{
        //    _screenUprageSoldier.FillingBarSpeedSkill(_soldier.SpeedSkillLevelData.CurrentStepSkill, _soldier.Rank.MaxStepSkill, _maxSpeedLevel,_currentSpeedLevel, _soldier.SoldiersStatsLevel.BestLevelMobility);
        //}

        private void UpgradeSubscription()
        {
            _screenUprageSoldier.SpecialSkill.ImprovementButton.ButtonOnClic += SetFillingBarSpecialSkill;
            _screenUprageSoldier.Survivability.ImprovementButton.ButtonOnClic += SetFillingBarSurvivability;
            _screenUprageSoldier.MeleeDamage.ImprovementButton.ButtonOnClic += SetFillingBarMeleeDamage;
            //_screenUprageSoldier.SpeedSkill.ImprovementButton.ButtonOnClic += SetFillingBarSpeedSkill;
        }

        private void UnsubscribeUpdate()
        {
            _screenUprageSoldier.SpecialSkill.ImprovementButton.ButtonOnClic -= SetFillingBarSpecialSkill;
            _screenUprageSoldier.Survivability.ImprovementButton.ButtonOnClic -= SetFillingBarSurvivability;
            _screenUprageSoldier.MeleeDamage.ImprovementButton.ButtonOnClic -= SetFillingBarMeleeDamage;
            //_screenUprageSoldier.SpeedSkill.ImprovementButton.ButtonOnClic -= SetFillingBarSpeedSkill;
        }

        private void SubscribeEvolution()
        {
            _screenUprageSoldier.SpecialSkill.ButtonEvolution.ButtonOnClic += UpEvolutionSpecialSkill;
            _screenUprageSoldier.Survivability.ButtonEvolution.ButtonOnClic += UpEvolutionSurvivabilitySkill;
            _screenUprageSoldier.MeleeDamage.ButtonEvolution.ButtonOnClic += UpEvolutionMelleDamageSkill;
            //_screenUprageSoldier.SpeedSkill.ButtonEvolution.ButtonOnClic += UpEvolutionSpeedSkill;
        }

        private void EvolutionUnsubscribing()
        {
            _screenUprageSoldier.SpecialSkill.ButtonEvolution.ButtonOnClic -= UpEvolutionSpecialSkill;
            _screenUprageSoldier.Survivability.ButtonEvolution.ButtonOnClic -= UpEvolutionSurvivabilitySkill;
            _screenUprageSoldier.MeleeDamage.ButtonEvolution.ButtonOnClic -= UpEvolutionMelleDamageSkill;
            //_screenUprageSoldier.SpeedSkill.ButtonEvolution.ButtonOnClic -= UpEvolutionSpeedSkill;
        }

        private void AddSoldierSquad()
        {
            foreach (var item in _screenUprageSoldier.Squad.Heroes.SoldierСards)
            {
                if (_soldier.HeroTypeId == item.SoldierCardViewer.Card.Soldier.HeroTypeId && _screenUprageSoldier.Squad.Soldiers.Count < 9)
                {
                    _screenUprageSoldier.Squad.Heroes.TransferringCardSquad(item);
                    _screenUprageSoldier.Squad.Heroes.ClearCardHeroes(item);
                    _screenUprageSoldier.Squad.AddSoldier(item);
                    _screenUprageSoldier.RangUI.SetPositionSquad(_soldier.DataSoldier.InSquad,_screenUprageSoldier.SoldierHero.Count);
                    ButtonAddAndRemuveSoldier();
                    break;
                }
            }
        }

        private void RemuveSoldierSquad()
        {
            _screenUprageSoldier.AddSoldierSquad();
            _screenUprageSoldier.Squad.RemoveFromSquad(_soldier);
            _screenUprageSoldier.RangUI.SetPositionSquad(_soldier.DataSoldier.InSquad, _screenUprageSoldier.SoldierHero.Count);
            ButtonAddAndRemuveSoldier();
            _screenUprageSoldier.CheckSwitchButtons();
        }

        //private bool CheckingIdenticalSoldiers()
        //{
        //    foreach (Soldier soldierSquad in _screenUprageSoldier.SoldierSquad)
        //    {
        //        if (soldierSquad.HeroTypeId == _soldier.HeroTypeId)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void RenderingLevelEvolutionSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetValueLevelEvolution(_currentLevelSpecealSkill);
        }

        private void RenderingLevelEvolutionSurvivability()
        {
            _screenUprageSoldier.Survivability.SetValueLevelEvolution(_currentSurvivabilityLevel);
        }

        private void RenderingLevelEvolutionMeleeDamage()
        {
            _screenUprageSoldier.MeleeDamage.SetValueLevelEvolution(_currentMelleDamageLevel);
        }

        //private void RenderingLevelEvolutionSpeedSkill()
        //{
        //    _screenUprageSoldier.SpeedSkill.SetValueLevelEvolution(_currentSpeedLevel);
        //}

        private void ButtonAddAndRemuveSoldier()
        {
            if (_soldier.DataSoldier.InSquad && _screenUprageSoldier.Squad.Soldiers.Count > 1)
            {
               ActivateButtonRemuve();
                DiactivatedButtonAddSoldier();
            }
            else if (_soldier.DataSoldier.InSquad && _screenUprageSoldier.Squad.Soldiers.Count == 1 && _soldier.DataSoldier.Hired)
            {
                _screenUprageSoldier.RemuveSoldierButton.gameObject.SetActive(false);
                _screenUprageSoldier.RangUI.Message.gameObject.SetActive(true);
                
                DiactivatedButtonAddSoldier();
            }
            else if (_soldier.UnitOpened && _soldier.DataSoldier.InSquad == false && _screenUprageSoldier.Squad.Soldiers.Count < 9)
            {
                _screenUprageSoldier.RangUI.Message.gameObject.SetActive(false);
                DiactivateButtonRemuve();
                ActivateButtonAddsoldier();
            }
            else if (_soldier.DataSoldier.InSquad == false && _soldier.UnitOpened == false && _soldier.DataSoldier.Hired == false)
            {
                _screenUprageSoldier.RangUI.Message.gameObject.SetActive(false);
            }
            else if (_soldier.DataSoldier.InSquad && _soldier.UnitOpened == false && _soldier.DataSoldier.Hired == false)
            {
                _screenUprageSoldier.RangUI.Message.gameObject.SetActive(false);
            }
        }

        private void ActivateButtonRemuve()
        {
            _screenUprageSoldier.RemuveSoldierButton.gameObject.SetActive(true);
            _screenUprageSoldier.RemuveSoldierButton.Button.interactable = true;
        }

        private void DiactivateButtonRemuve()
        {
            _screenUprageSoldier.RemuveSoldierButton.gameObject.SetActive(false);
            _screenUprageSoldier.RemuveSoldierButton.Button.interactable = false;
        }

        private void ActivateButtonAddsoldier()
        {
            if (_soldier.DataSoldier.Hired)
            {
                _screenUprageSoldier.AddSoldierButton.gameObject.SetActive(true);
                _screenUprageSoldier.AddSoldierButton.Button.interactable = true;
            }
        }

        private void DiactivatedButtonAddSoldier()
        {
            _screenUprageSoldier.AddSoldierButton.gameObject.SetActive(false);
            _screenUprageSoldier.AddSoldierButton.Button.interactable = false;
        }

        private void DisableSlidier()
        {
            _screenUprageSoldier.MeleeDamage.SliderSkill.gameObject.SetActive(false);
            _screenUprageSoldier.Survivability.SliderSkill.gameObject.SetActive(false);
            _screenUprageSoldier.SpecialSkill.SliderSkill.gameObject.SetActive(false);
            //_screenUprageSoldier.SpeedSkill.SliderSkill.gameObject.SetActive(false);
        }

        private void ActivateSlidier()
        {
            _screenUprageSoldier.MeleeDamage.SliderSkill.gameObject.SetActive(true);
            _screenUprageSoldier.Survivability.SliderSkill.gameObject.SetActive(true);
            _screenUprageSoldier.SpecialSkill.SliderSkill.gameObject.SetActive(true);
            //_screenUprageSoldier.SpeedSkill.SliderSkill.gameObject.SetActive(true);
        }

        private void DisbleButtonUpgrade()
        {
            _screenUprageSoldier.RangUI.ButtonAdd.gameObject.SetActive(false);
            _screenUprageSoldier.SpecialSkill.ImprovementButton.gameObject.SetActive(false);
            _screenUprageSoldier.Survivability.ImprovementButton.gameObject.SetActive(false);
            _screenUprageSoldier.MeleeDamage.ImprovementButton.gameObject.SetActive(false);
            _screenUprageSoldier.SpecialSkill.ButtonEvolution.gameObject.SetActive(false);
            _screenUprageSoldier.Survivability.ButtonEvolution.gameObject.SetActive(false);
            _screenUprageSoldier.MeleeDamage.ButtonEvolution.gameObject.SetActive(false);
            //_screenUprageSoldier.SpeedSkill.ImprovementButton.gameObject.SetActive(false);
        }

        private void ActivateButtonUpgrade()
        {
            if(_soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill == _maxLevelSpecealSkill && _soldier.SpecialSkillLevelData.CurrentStepSkill == _soldier.SpecialSkillLevelData.MaxStepValue)
            {
                _screenUprageSoldier.SpecialSkill.ImprovementButton.gameObject.SetActive(false);
            }
            else
            {
                _screenUprageSoldier.SpecialSkill.ImprovementButton.gameObject.SetActive(true);
            }
            
            if(_soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel == _maxSurvivabilityLevel && _soldier.SurvivabilityLevelData.CurrentStepSkill == _soldier.SurvivabilityLevelData.MaxStepValue)
            {
                _screenUprageSoldier.Survivability.ImprovementButton.gameObject.SetActive(false);
            }
            else
            {
                _screenUprageSoldier.Survivability.ImprovementButton.gameObject.SetActive(true);
            }

            if(_soldier.SoldiersStatsLevel.CurrentMeleelevel == _maxMelleDamageLevel && _soldier.MeleeDamageLevelData.CurrentStepSkill == _soldier.MeleeDamageLevelData.MaxStepValue)
            {
                _screenUprageSoldier.MeleeDamage.ImprovementButton.gameObject.SetActive(false);
            }
            else
            {
                _screenUprageSoldier.MeleeDamage.ImprovementButton.gameObject.SetActive(true);
            }
            
            //_screenUprageSoldier.RangUI.ButtonAdd.gameObject.SetActive(true);
            //_screenUprageSoldier.SpeedSkill.ImprovementButton.gameObject.SetActive(true);
        }

        private void UpRank()
        {
            if( _screenUprageSoldier.ProgressService.Progress.Wallet.Stars.Count >= _screenUprageSoldier.RangUI.Price)
            {
                _screenUprageSoldier.ProgressService.Progress.Wallet.Stars.Reduce(_screenUprageSoldier.RangUI.Price);
                _soldier.Rank.SetSoldier(_soldier);
                _soldier.Rank.UpLevelHero(_screenUprageSoldier.RangUI, _screenUprageSoldier.ProgressService,_screenUprageSoldier.Hire);
                _progressService.Progress.Achievements.AllValueRankAch = _soldier.Rank.CurrentLevelHero;
                if (_screenUprageSoldier.Squad.SoldierСards.Count < 9)
                {
                    AddSoldierSquad();

                }
                SetValuesCharacteristics(CameraParent, _progressService);
                Restart?.Invoke();
                // SetDataSoldier(CameraParent, _progressService);
            }
            else
            {
                _screenUprageSoldier.ScreenNoPay.gameObject.SetActive(true);
            }
        }

        private void PoverSoldierShow()
        {
            _powerSoldier = _soldier.Power.GetUnitDPS(_soldier);
            //_powerSoldier = _soldier.RoundUp(_powerSoldier,2);

            _screenUprageSoldier.RangUI.SetPowerHero(_powerSoldier);
        }

        private void SpecialSkillEvolution()
        {
            _currentLevelSpecealSkill++;
            RenderingLevelEvolutionSpecialSkill();
            _soldier.SpecialSkillLevelData.ResetStepLevelSkill();
            _progressService.Progress.Achievements.AllUpEvolutionSkill++;
            _soldier.SoldiersStatsLevel.CurrentLevelSpecialSkill = _currentLevelSpecealSkill;
            //_soldier.SoldiersStatsLevel.SetSpecAttac(_currentLevelSpecealSkill);
            _soldier.SpecialSkillUpgrade();
            RenderingSliderSpecialSkill(_screenUprageSoldier.ButtonMultiplier);
            if(_screenUprageSoldier.AllStep > 1)
            {
                SetPriceMultiplier();
            }
            else
            {
                FillSpecialSkill(_soldier.NewSpecialDamage, Sing);
            }
            _allMarkerSpecAttak = TrySetMarkerSpecSkillCoin();
            SetMaxLevelSkill();
            ChangedCharacteristics?.Invoke();
            PoverSoldierShow();
        }

        private void SurvivabilityEvolution()
        {
            _currentSurvivabilityLevel++;
            RenderingLevelEvolutionSurvivability();
            _soldier.SurvivabilityLevelData.ResetStepLevelSkill();
            _progressService.Progress.Achievements.AllUpEvolutionSkill++;
            _soldier.SoldiersStatsLevel.CurrentSurvivabilityLevel = _currentSurvivabilityLevel;
            _soldier.SurvivabilityLevelData.SkillSurvivabikityUpgrade(_soldier);
            RenderingSliderSurvivability(_screenUprageSoldier.ButtonMultiplier);
            if(_screenUprageSoldier.AllStepHp > 1)
            {
                SetPriceMultiplier();
            }
            else
            {
                FillSurvivability(_soldier.CurrentHealth, _soldier.NewHealth, Sing);
            }
            _allMarkerSurvability = TrySetMarkerSurvivability();
            SetMaxLevelSkill();
            ChangedCharacteristics?.Invoke();
            PoverSoldierShow();
        }

        private void MelleDamageSkillEvolution()
        {
            _currentMelleDamageLevel++;
            RenderingLevelEvolutionMeleeDamage();
            _soldier.MeleeDamageLevelData.ResetStepLevelSkill();
            _progressService.Progress.Achievements.AllUpEvolutionSkill++;
            _soldier.SoldiersStatsLevel.CurrentMeleelevel = _currentMelleDamageLevel;
            _soldier.MeleeDamageLevelData.SkillMeleeDamageUpgrade(_soldier);
            RenderingSliderMeleeDamage(_screenUprageSoldier.ButtonMultiplier);
            if(_screenUprageSoldier.AllStepMeleeDamage > 1)
            {
                SetPriceMultiplier();
            }
            else
            {
                SetMeleeDamageData(_soldier.CurrentMeleeDamage, _soldier.NewMeleeDamage, Sing);
            }
            _allMarkerMeleeDamage = TrySetMarkerMeleeDamage();
            SetMaxLevelSkill();
            ChangedCharacteristics?.Invoke();
            PoverSoldierShow();
        }

        private void ScreenSelection(int currentIndex)
        {
            switch (currentIndex)
            {
                case 1:
                    SetTextSpecialSkill();
                    break;
                case 2:
                    SetTextSurwability();
                    break;
                case 3:
                    SetTextMeleeDamage();
                    break;
            }
        }
        //private void SpeedSkillEvolution()
        //{
        //    _currentSpeedLevel++;
        //    RenderingLevelEvolutionSpeedSkill();
        //    _soldier.SpeedSkillLevelData.SkillSpeedUpgrade(_soldier);
        //    SetSpeedSkill(_soldier.CurrentSpeed, _soldier.NewSpeed, Sing);
        //    SetMaxLevelSkill();
        //    _soldier.SpeedSkillLevelData.ResetStepLevelSkill();
        //    _soldier.SoldiersStatsLevel.CurrentSpeedLevel = _currentSpeedLevel;
        //    ChangedCharacteristics?.Invoke();
        //    RenderingSliderSpeedSkill();
        //    PoverSoldierShow();
        //}
    }
}
