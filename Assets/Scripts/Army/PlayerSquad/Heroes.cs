using Scripts.Army.AllCadsHeroes;
using Scripts.Army.TypesSoldiers;
using Scripts.Data;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Army.PlayerSquad
{
    public class Heroes : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private List<Soldier> _soldiers = new List<Soldier>();
        [SerializeField] private List<SoldierCard> _soldierСards = new List<SoldierCard>();
        [SerializeField] private Transform _containerSquad;
        [SerializeField] private HerosCards _herosCards;
        [SerializeField] private CanvasHeroes _canvasHeroes;

        private bool _isEmpty = false;
        private ISaveLoadService _saveLoadService;
        private List<HeroTypeId> _typeIds = new List<HeroTypeId>();
        public HerosCards HerosCards => _herosCards;

        public List<Soldier>Soldiers => _soldiers;
        public List<SoldierCard> SoldierСards => _soldierСards;

        public event Action<SoldierCard> AddSoldierCard;
        public event Action ChangadSquadCastle;
        public event Action StartHeroMarker;
        public event Action<SoldierCard> AddCardCountCard;

        private void OnEnable()
        {
            _canvasHeroes.Activate += ActivateteCameraSoldier;
            _canvasHeroes.Deactivate += DiactivateCameraSoldier;
        }

        private void OnDisable()
        {
            _canvasHeroes.Activate -= ActivateteCameraSoldier;
            _canvasHeroes.Deactivate -= DiactivateCameraSoldier;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroesSquad.SoldierHerosSquad = _soldiers;
            progress.HeroesSquad.HeroTypeIds = _typeIds;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _soldiers = progress.HeroesSquad.SoldierHerosSquad;
            _typeIds = progress.HeroesSquad.HeroTypeIds;

            if (_soldiers.Count > 0)
            {
                OpenAndMoveHeroesMap(progress);
            }
            ChangadSquadCastle?.Invoke();
            StartHeroMarker?.Invoke();
        }

        public void OpenAndMoveHeroesMap(PlayerProgress progress)
        {
            SetNewCardHero(progress);
            foreach (var item in _soldierСards)
            {
                item.transform.SetParent(_herosCards.ContainerHeroe.transform);
                _herosCards.RemoveCard(item);
                item.ShowCountCard();
            }
        }

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        public void Save()
        {
            _saveLoadService.SaveProgress();
        }

        public void ActivateteCameraSoldier()
        {
            foreach (SoldierCard item in _soldierСards)
            {
                item.ActivateCamera();
            }
        }

        public void DiactivateCameraSoldier()
        {
            foreach (SoldierCard item in _soldierСards)
            {
                item.DiactivateCamera();
            }
        }

        public void ClierListSoldier()
        {
            _soldiers.Clear();
            _soldierСards.Clear();
        }

        public void AddSoldier(Soldier currentSoldier)
        {
            CheckingPresenceSquad(currentSoldier);
            if (_isEmpty == false)
            {
                _soldiers.Add(currentSoldier);
                _typeIds.Add(currentSoldier.HeroTypeId);
            }
            
            
            ChangadSquadCastle?.Invoke();
        }

        public void AddCardSoldier(SoldierCard soldierСard)
        {
            _soldierСards.Add(soldierСard);
           
            var sold = soldierСard.SoldierCardViewer.Card.Soldier;
            CheckingPresenceSquad(sold);
            if(_isEmpty == false)
            {
                _soldiers.Add(sold);
                _typeIds.Add(sold.HeroTypeId);
            }
            
          
            soldierСard.SetSoliedrMap(sold);
            soldierСard.SoldierCardViewer.SaveDataHero();
            Save();
            ChangadSquadCastle?.Invoke();
        }

        public void SetNewCardHero(PlayerProgress playerProgress)
        {
            //List<Soldier> unitsSoldier = new List<Soldier>();
            foreach (HeroTypeId soldier in _typeIds)
            {
                foreach (SoldierCard card in _herosCards.SoldierСardsAll)
                {
                    if (soldier == card.SoldierCardViewer.Card.Soldier.HeroTypeId && card.gameObject.activeInHierarchy == false)
                    {
                        if(CheckingCardAvailability(card) == false)
                        {
                            _soldiers.Add(card.SoldierCardViewer.Card.Soldier);
                        }
                        //unitsSoldier.Add(card.SoldierCardViewer.Card.Soldier);
                        card.SetSoliedrMap(card.SoldierCardViewer.Card.Soldier);
                        card.gameObject.SetActive(true);
                        _soldierСards.Add(card);
                        AddCardCountCard?.Invoke(card);
                        int countCard = playerProgress.PlayerData.TypeHero.AllHerosType.GetCountCardsHero(card.Soldier);
                        card.Soldier.Rank.CurrentCountCard = countCard;
                       // card.SoldierCardViewer.SaveDataHero();
                    }
                }
            }
            //_soldiers.Clear();
            //_soldiers = unitsSoldier;
        }
        
        private bool CheckingCardAvailability(SoldierCard card)
        {
            foreach (var item in _soldiers)
            {
                if(item.HeroTypeId == card.SoldierCardViewer.Card.Soldier.HeroTypeId)
                {
                    return true;
                }
            }
            return false;
        }

        public SoldierCard GetCardTransform(Soldier hero)
        {
            foreach(SoldierCard soldierСard in _soldierСards)
            {
                if(soldierСard.SoldierCardViewer.Card.Soldier.HeroTypeId == hero.HeroTypeId)
                {
                    return soldierСard;
                }
            }
            return null;
        }

        public void TransferringCardSquad(SoldierCard pointCard)
        {
            pointCard.transform.SetParent(_containerSquad.transform);
        }

        public void AddCardSquad(SoldierCard pointCard)
        {
            pointCard.SoldierCardViewer.Start();
            AddSoldierCard?.Invoke(pointCard);
        }

        public void ClearCardHeroes(SoldierCard card)
        {
            RemoveSoldier(card.SoldierCardViewer.Card.Soldier);
            RemoveCardHero(card);
        }

        private void CheckingPresenceSquad(Soldier soldier)
        {
            foreach (Soldier item in _soldiers)
            {
                if (item.HeroTypeId == soldier.HeroTypeId)
                {
                    _isEmpty = true;
                }
            }
        }

        private void RemoveCardHero(SoldierCard card)
        {
            List<SoldierCard> soldierCardsNew = new List<SoldierCard>();

            foreach (SoldierCard item in _soldierСards)
            {
                if(item.Soldier.HeroTypeId != card.Soldier.HeroTypeId)
                {
                    soldierCardsNew.Add(item);
                }
            }
            _soldierСards.Clear();
            _soldierСards = soldierCardsNew;
        }

        private void RemoveSoldier(Soldier hero) 
        {
            List<Soldier> soldiers = new List<Soldier>();
            List<HeroTypeId> heroTypes = new List<HeroTypeId>();
            foreach (Soldier soldier in _soldiers)
            {
                if(soldier.HeroTypeId != hero.HeroTypeId)
                {
                    soldiers.Add(soldier);
                    heroTypes.Add(soldier.HeroTypeId);
                }
            }
            _soldiers.Clear();
            _soldiers = soldiers;
            _typeIds = heroTypes;
            ChangadSquadCastle?.Invoke();
        }
    }
}
