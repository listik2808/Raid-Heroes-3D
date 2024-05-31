using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Logic.CastleConstruction;
using Scripts.Logic.TaskAchievements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Army.AllCadsHeroes
{
    public class ListAllHeroes : MonoBehaviour
    {
        [SerializeField] private List<Soldier> _allHeroCards = new List<Soldier>();
        [SerializeField] private ScreenCardShow _screenCardShow;
        [SerializeField] private Heroes _heroes;
        [SerializeField] private Squad _squad;
        [SerializeField] private GetHeroCardsAchievements _getHeroCardsAchievements;

        private List<Soldier>[] _allTypeHeroCards = new List<Soldier>[3];
        private List<Soldier> _allTypeHeroSimple = new List<Soldier>();
        private List<Soldier> _allTypeHeroRare = new List<Soldier>();
        private List<Soldier> _allTypeHeroEpic = new List<Soldier>();
        private List<Soldier> _typeCard = new List<Soldier>();
        private Soldier _soldierHero;
        private int _countcard;
        private int _allCountCard = 0;
        //private float _elepsedTime = 0;
        private bool _buttonPressed = false;
        private IPersistenProgressService _progress;
        private bool _squadHero = false;
        private bool _squadCastl = false;
        private BookmarkButton _bookmarkButton;
        private NextScene.MainStage _mainStage;
        public List<Soldier> AllHeroCards => _allHeroCards;
        public IPersistenProgressService Progress => _progress;
        public ScreenCardShow ScreenCardShow => _screenCardShow;

        private void Start()
        {
            _progress = AllServices.Container.Single<IPersistenProgressService>();
            _allCountCard = _progress.Progress.Achievements.AchievementAllCountCard;
        }

        public int EnrollRandomHeroCard(bool isTutor,float timer, int countCard, int rarity ,NextScene.MainStage mainStage = null, BookmarkButton button = null, ConstructionCastle constructionCastle = null)
        {
            if(button != null)
            {
                _bookmarkButton = button;
                _bookmarkButton.Button.onClick.AddListener(ButtonListener);
            }
            
            if(_progress.Progress.Training.Tutor == false && isTutor == false)
            {
                foreach(var item in _allHeroCards)
                {
                    if(item.HeroTypeId == _progress.Progress.Training.HeroType)
                    {
                        _soldierHero = item;
                    }
                }

                if (mainStage != null)
                {
                    _mainStage = mainStage;
                    mainStage.ScreenClose += StartShowCard;
                    _screenCardShow.SetComponentsCard(_soldierHero, countCard, this, null, mainStage);
                }
                else
                {
                    _screenCardShow.SetComponentsCard(_soldierHero, countCard, this, constructionCastle);
                }

                StartCoroutine(ScreenCardReward(timer));
                if(rarity == 1)
                    rarity = 0;
                else if (rarity == 2)
                    rarity = 1;
                else if(rarity == 3)
                    rarity = 2;

                return rarity;
            }
            else
            {
                Initialization();
                ClirAllList();
                _typeCard.Clear();
                if (rarity == 0)
                {
                    System.Random random = new System.Random();
                    SortingCardType(_allTypeHeroSimple, 0);
                    Shuffle(_allTypeHeroSimple);
                    SortingCardType(_allTypeHeroRare, 1);
                    Shuffle(_allTypeHeroRare);
                    SortingCardType(_allTypeHeroEpic, 2);
                    Shuffle(_allTypeHeroEpic);
                    _allTypeHeroCards[0] = _allTypeHeroSimple;
                    _allTypeHeroCards[1] = _allTypeHeroRare;
                    _allTypeHeroCards[2] = _allTypeHeroEpic;
                    SetTypeCardhero(random);
                }
                else if (rarity == 1)
                {
                    rarity = 0;
                    SortingCardType(_allTypeHeroSimple, rarity);
                    Shuffle(_allTypeHeroSimple);
                    _typeCard = _allTypeHeroSimple;
                }
                else if (rarity == 2)
                {
                    rarity = 1;
                    SortingCardType(_allTypeHeroRare, rarity);
                    Shuffle(_allTypeHeroRare);
                    _typeCard = _allTypeHeroRare;
                }
                else if (rarity == 3)
                {
                    rarity = 2;
                    SortingCardType(_allTypeHeroEpic, rarity);
                    Shuffle(_allTypeHeroEpic);
                    _typeCard = _allTypeHeroEpic;
                }
                int numberCard;
                if (_typeCard.Count > 0)
                {
                    numberCard = rarity;
                    int number = Random.Range(0, _typeCard.Count);
                    _soldierHero = _typeCard[number];

                    if (mainStage != null)
                    {
                        _mainStage = mainStage;
                        mainStage.ScreenClose += StartShowCard;
                        _screenCardShow.SetComponentsCard(_soldierHero, countCard, this, null, mainStage);
                    }
                    else
                        _screenCardShow.SetComponentsCard(_soldierHero, countCard, this, constructionCastle);

                    StartCoroutine(ScreenCardReward(timer));
                    //if (mainStage != null)
                    //    SetCard(countCard, _soldierHero,timer ,mainStage);
                    //else
                    //    SetCard(countCard, _soldierHero,timer, null,constructionCastle);
                }
                else
                {
                    numberCard = -1;
                }
                return numberCard;
            }
        }

        private void StartShowCard()
        {
            _mainStage.ScreenClose -= StartShowCard;
            _buttonPressed = true;
        }

        public void SetCard(int countCard, Soldier soldier,NextScene.MainStage mainStage = null,ConstructionCastle constructionCastle = null)
        {
            _squadHero = CheckingAvailability(_progress.Progress.SquadPlayer.SoldiersPlayer, _soldierHero,countCard); //(_squad.Soldiers, _soldierHero, countCard);
            if (_squadHero == false)
            {
                _squadCastl = CheckingAvailability(_progress.Progress.HeroesSquad.SoldierHerosSquad, _soldierHero, countCard);//(_heroes.Soldiers, _soldierHero, countCard);
                if (_squadCastl == false)
                {
                    _heroes.AddSoldier(soldier);
                    _progress.Progress.HeroesSquad.SetHerosSquadCastle(soldier);
                    SaveCountCards(soldier,countCard);
                    _heroes.OpenAndMoveHeroesMap(_progress.Progress);
                    //_heroes.LoadProgress(_progress.Progress);
                }
            }
            //if (mainStage != null)
            //    _screenCardShow.SetComponentsCard(_soldierHero, countCard, null, mainStage);
            //else
            //    _screenCardShow.SetComponentsCard(_soldierHero, countCard, constructionCastle);
            //StartCoroutine(ScreenCardReward(timer));
            //Invoke("ActivateScreen", timer);
        }

        private void ButtonListener()
        {
            _buttonPressed = true;
            _bookmarkButton.Button.onClick.RemoveListener(ButtonListener);
        }

        private IEnumerator ScreenCardReward(float timer)
        {
            if(_bookmarkButton != null)
            {
                while (_buttonPressed == false)
                {
                    yield return null;
                    if (_buttonPressed == true)
                    {
                        _screenCardShow.gameObject.SetActive(true);
                        _buttonPressed = false;
                    }
                }
            }
            else if (_mainStage != null)
            {
                while(_buttonPressed == false)
                {
                    yield return null;
                    if (_buttonPressed == true)
                    {
                        _screenCardShow.gameObject.SetActive(true);
                        _buttonPressed = false;
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(timer);
                _screenCardShow.gameObject.SetActive(true);
            }
           
        }

        private void SaveCountCards(Soldier soldier,int countCard)
        {
            if (_progress.Progress.Training.Tutor == false && _progress.Progress.Training.CountCard < 5)
            {
                _progress.Progress.Training.CountCard++;
            }
            soldier.Rank.SetSoldier(soldier);
            soldier.Rank.AddCountCard(countCard);
            foreach (var item in _progress.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if(item.TypeId == soldier.HeroTypeId)
                {
                    item.CurrentCountCard += countCard;
                    _allCountCard += countCard;
                    _progress.Progress.Achievements.AchievementAllCountCard = _allCountCard;
                    _getHeroCardsAchievements.SetCountCard(_progress);
                    _heroes.Save();
                }
            }
        }

        private void SetTypeCardhero(System.Random random)
        {
            int n = _allTypeHeroCards.Length - 1;
            int i = (int)Math.Round((n + 1) / ((n * random.NextDouble()) + 1) - 1);
            _typeCard = _allTypeHeroCards[i];
        }

        private void Initialization()
        {
            _allTypeHeroCards[0] = new List<Soldier>();
            _allTypeHeroCards[1] = new List<Soldier>();
            _allTypeHeroCards[2] = new List<Soldier>();
        }

        private bool TryAddCountCard(int countCard, Soldier soldier, NextScene.MainStage mainStage = null)
        {
            _countcard = countCard;
            
            foreach (var item in Progress.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if (item.TypeId == soldier.HeroTypeId)
                {
                    int count = item.GetMaxCountCard();
                    if(item.CurrentCountCard + countCard <= count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ActivateScreen()
        {
            _screenCardShow.gameObject.SetActive(true);
        }

        private bool CheckingAvailability( List<Soldier> soldiers,Soldier soldier,int countCard)
        {
            foreach (var item in soldiers)
            {
                if (item.HeroTypeId == soldier.HeroTypeId)
                {
                    SaveCountCards(item, countCard);
                    return true;
                }
            }
            return false;
        }

        private void ClirAllList()
        {
            foreach (var item in _allTypeHeroCards)
            {
                item.Clear();
            }
        }

        private void SortingCardType(List<Soldier> allTypeHero, int value)
        {
            foreach (Soldier hero in _allHeroCards)
            {
                if (hero.DataSoldier.Type == (CardType)value)
                {
                    bool resultCountCard = TryAddCountCard(1, hero);
                    if (resultCountCard)
                    {
                        allTypeHero.Add(hero);
                    }
                }
            }
        }

        private void Shuffle(List<Soldier> soldier)
        {
            for (int i = 0; i < soldier.Count; i++)
            {
                Soldier temp = soldier[i];
                int randomIndex = Random.Range(i, soldier.Count);
                soldier[i] = soldier[randomIndex];
                soldier[randomIndex] = temp;
            }
        }
    }
}
