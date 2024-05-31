using Scripts.Army.AllCadsHeroes;
using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.CameraMuve;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.SpecificationsUI;
using Scripts.Logic;
using Scripts.Logic.TaskAchievements;
using Scripts.NextScene;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class WarSelectionScreen : Screen
    {
        private const int _value = 1;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<Soldier> _soldiers =new List<Soldier>();
        [SerializeField] private Heroes _heroes;
        [SerializeField] private HerosCards _heroCards;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _textTypeHero;
        [SerializeField] private TMP_Text _textTypeAttack;
        [SerializeField] private BookmarkButton _forwardButton;
        [SerializeField] private BookmarkButton _backButton;
        [SerializeField] private RankUI _rankUI;
        [SerializeField] private List<SoldierCardViewer> _soldiersCardsViewer;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private RawImage _imageRaw;
        [SerializeField] private List<CameraParent> _herocCameraParent = new List<CameraParent>();
        [SerializeField] private HireHeroes _hireHeroes;
        [SerializeField] private GetHeroCardsAchievements _getHeroCardsAchievements;
        [SerializeField] private BattleScene _battleScene;
        [SerializeField] private Squad _squad;

        private IPersistenProgressService _progressService;
        private int _minSlotNumber = 0;
        private int _maxSlotNumber;
        private Soldier _currentSoldier;
        private ISaveLoadService _saveLoadService;
        private bool _isSelectionWindow;
        private string _nameHero;
        private string _nameAttack;

        public event Action Play;

        public void Construct(ISaveLoadService saveLoadService,IPersistenProgressService persistenProgressService)
        {
            _progressService = persistenProgressService;
            _saveLoadService = saveLoadService;
            _isSelectionWindow = YandexGame.savesData.SelectionWindow;
            if (_isSelectionWindow)
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnEnable()
        {
            //ResetHero += ResetPositionHero;
            _bookmark.ButtonOnClic += InformAboutChoice;
            Play += Close;
            _forwardButton.ButtonOnClic += ChooseNextFighter;
            _backButton.ButtonOnClic += SelectPreviousFighter;
            YandexGame.GetDataEvent += Save;
            _audioSource.Play();
        }

        protected override void OnDisable()
        {
            //ResetHero -= ResetPositionHero;
            _bookmark.ButtonOnClic -= InformAboutChoice;
            Play -= Close;
            _forwardButton.ButtonOnClic -= ChooseNextFighter;
            _backButton.ButtonOnClic -= SelectPreviousFighter;
            YandexGame.GetDataEvent -= Save;
        }

        private void Start()
        {
            _canvasGroup.blocksRaycasts = false;
            _maxSlotNumber = _soldiers.Count -1;
            ShowSoldier(_minSlotNumber);
        }

        private void ShowSoldier(int number)
        {
            _herocCameraParent[number].gameObject.SetActive(true);
            _imageRaw.texture = _herocCameraParent[number].RenderTexture;
            _nameHero = CustomRuLocalization.GetRuHeroTypeId((int)_soldiers[number].HeroTypeId);
            _nameAttack = CustomRuLocalization.GetSpecAttack((int)_soldiers[number].SpecialAttack);
            _textTypeHero.text = _nameHero;
            if (_soldiers[number].SpecialAttack == StaticData.SpecialAttack.Stun)
            {
                _textTypeAttack.text = $"{_nameAttack} <sprite name=Stun>";
            }
            else if(_soldiers[number].SpecialAttack == StaticData.SpecialAttack.DoubleAttack)
            {
                _textTypeAttack.text = $"{_nameAttack} <sprite name=DoubleAttack>";
            }
            else if(_soldiers[number].SpecialAttack == StaticData.SpecialAttack.Shoot)
            {
                _textTypeAttack.text = $"{_nameAttack} <sprite name=Shoot>";
            }
            _currentSoldier = _soldiers[number];
        }

        private void InformAboutChoice()
        {
            _canvasGroup.blocksRaycasts = true;
            _currentSoldier.DataSoldier.SetCardActivation();
            _currentSoldier.Rank.SetSoldier(_currentSoldier);
            _currentSoldier.Rank.UpLevelHero(_rankUI,_progressService, _hireHeroes);
            CardSearch(_currentSoldier);
            _heroes.AddSoldier(_currentSoldier);
            _heroes.SetNewCardHero(_progressService.Progress);
           
            SoldierCard pointCard = _heroes.GetCardTransform(_currentSoldier);
            _heroes.TransferringCardSquad(pointCard);
            //_currentSoldier.DataSoldier.SetCard(pointCard);
            _heroes.ClearCardHeroes(pointCard);
            _heroCards.RemoveCard(pointCard);
            _heroes.AddCardSquad(pointCard);
            SaveDataOneSoldiers();
            _currentSoldier.Rank.AddCountCard(5);
            _progressService.Progress.Achievements.AchievementAllCountCard += 5;
            _getHeroCardsAchievements.SetCountCard(_progressService);
            _isSelectionWindow = true;
            YandexGame.savesData.SelectionWindow = _isSelectionWindow;
            if (_currentSoldier.HeroTypeId == StaticData.HeroTypeId.Archer)
            {
                var count = Random.Range(0, 100);
                if(count > 50)
                {
                    _progressService.Progress.Training.HeroType = HeroTypeId.Knight;
                }
                else
                {
                    _progressService.Progress.Training.HeroType = HeroTypeId.Assassin;
                }
            }
            else
            {
                _progressService.Progress.Training.HeroType = HeroTypeId.Archer;
            }
            _cameraMovement.MoveCamera();
            _battleScene.ActivateAnimationFinger();
            _currentSoldier.DataSoldier.SoldierСard.SoldierCardViewer.SaveDataHero();
            Save();
        }

        private void Save()
        {
            if(YandexGame.SDKEnabled == true)
            {
                _saveLoadService.SaveProgress();
            }
            Play?.Invoke();
        }

        private void SaveDataOneSoldiers()
        {
            foreach (var item in _progressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if (item.TypeId == _currentSoldier.HeroTypeId)
                {
                    item.CurrentCountCard += 5;
                    item.CurrentLevelHero = 0;
                    item.UnitOpened = true;
                    item.Hired = true;
                }
            }
        }

        private void CardSearch(Soldier soldier)
        {
            foreach (var card in _soldiersCardsViewer)
            {
                if(card.Card.Soldier.HeroTypeId == soldier.HeroTypeId)
                {
                    card.RenderinfTextCountCard(5);
                }
            }
        }

        private void ChooseNextFighter()
        {
            SearchForNextSoldier(_value);
        }

        private void SelectPreviousFighter()
        {
            LookForPreviousSoldier(_value);
        }

        private void LookForPreviousSoldier(int value)
        {
            int number = _soldiers.IndexOf(_currentSoldier);
            _herocCameraParent[number].gameObject.SetActive(false);
            if (number - value < _minSlotNumber)
            {
                ShowSoldier(_maxSlotNumber);
            }
            else
            {
                number -= value;
                ShowSoldier(number);
            }
        }

        private void SearchForNextSoldier(int value)
        {
            int number = _soldiers.IndexOf(_currentSoldier);
            _herocCameraParent[number].gameObject.SetActive(false);
            
            if (number + value > _maxSlotNumber)
            {
                ShowSoldier(_minSlotNumber);
            }
            else 
            {
                number += value;
                ShowSoldier(number);
            }
        }
    }
}
