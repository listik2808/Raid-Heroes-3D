using UnityEngine.UI;
using UnityEngine;
using Scripts.Army.PlayerSquad;
using Scripts.Army.AllCadsHeroes;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.TaskAchievements;

namespace Assets.Scripts.CheatPanel
{
    public class Cards : CheatPanelComponent
    {
        [SerializeField] private GetHeroCardsAchievements _getHeroCardsAchievements;
        [SerializeField] private MarkersImprovingSoldiers _markersImprovingSoldiers;
        public InputField Count;
        public Dropdown DropDown;
        [SerializeField] private Squad _squad;
        [SerializeField] private HerosCards _heroCards;
        private int value;
        bool _resultAllCardHero = false;
        bool _resultHeros;
        bool _resultSquad;
        private ISaveLoadService _saveLoadService;
        private IPersistenProgressService _persistenProgressService;

        public override void Change()
        {
            Card newCard = new Card(Input.text);
            CardType type = new CardType();
            switch (DropDown.value)
            {
                case 0:
                    type = CardType.Simple;
                    break;
                case 1:
                    type = CardType.Rare;
                    break;
                case 2:
                    type = CardType.Epic;
                    break;
            }
            value = int.Parse(Count.text);
            Debug.Log(newCard.GetHashCode());
            _persistenProgressService.Progress.PlayerData.Cards.Add(newCard, type, int.Parse(Count.text));
            SetCard(Input.text);
        }

        public override void Start()
        {
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void SetCard(string name)
        {
            _resultAllCardHero = AllCardsHeros(name);

            if (_resultAllCardHero == false)
            {
                _resultSquad = SearchSquad(name);
            }

            if (_resultSquad == false && _resultAllCardHero == false)
            {
                _resultHeros = SearchHeros(name);
            }
        }

        private bool AllCardsHeros(string name)
        {
            foreach (SoldierCard heroCard in _heroCards.SoldierСardsAll)
            {
                if (heroCard.SoldierCardViewer.Card.Name == name )
                {
                    SetSoldier(heroCard);
                    _squad.Heroes.AddCardSoldier(heroCard);
                    _squad.TransferringCardHeroes(heroCard);
                    heroCard.gameObject.SetActive(true);
                    SetCountCard(heroCard);
                    heroCard.ShowCountCard();
                    _heroCards.RemoveCard(heroCard);
                    return true;
                }
            }
            return false;
        }

        private bool SearchSquad(string name)
        {
            foreach (SoldierCard card in _squad.SoldierСards)
            {
                if (card.SoldierCardViewer.Card.Name == name )
                {
                    SetSoldier(card);
                    SetCountCard(card);
                    card.ShowCountCard();
                    return true;
                }
            }
            return false;
        }

        private bool SearchHeros(string name)
        {
            foreach (SoldierCard card in _squad.Heroes.SoldierСards)
            {
                if (card.SoldierCardViewer.Card.Name == name)
                {
                    SetSoldier(card);
                    SetCountCard(card);
                    card.ShowCountCard();
                    return true;
                }
            }
            return false;
        }

        private void SetCountCard(SoldierCard card)
        {
            card.SoldierCardViewer.AddCountCard(value);
            foreach (var item in _persistenProgressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if(item.TypeId == card.SoldierCardViewer.Card.Soldier.HeroTypeId)
                {
                    item.CurrentCountCard += value;
                    _persistenProgressService.Progress.Achievements.AchievementAllCountCard += value;
                    _getHeroCardsAchievements.SetCountCard(_persistenProgressService);
                    _saveLoadService.SaveProgress();
                }
            }
            _markersImprovingSoldiers.ActivateRecalculation();
        }

        private void SetSoldier(SoldierCard card)
        {
            card.SoldierCardViewer.Card.Soldier.Rank.SetSoldier(card.SoldierCardViewer.Card.Soldier);
        }
    }
}
