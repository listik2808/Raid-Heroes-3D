using Scripts.Army.TypesSoldiers;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine.SceneManagement;
using System;
using Scripts.Logic.ShowingSoldierData;
using TMPro;
using Scripts.StaticData;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Army.PlayerSquad
{
    public class Squad : MonoBehaviour, ISavedProgress
    {
        public const string TextSquadCount = "ОТРЯД";

        [SerializeField] private List<Soldier> _soldiers = new List<Soldier>();
        [SerializeField] private List<SoldierCard> _soldierСard = new List<SoldierCard>();
        [SerializeField] private Heroes _heroes;
        [SerializeField] private Transform _containerHero;
        [SerializeField] private TMP_Text _textCountSquad;
        [SerializeField] private CanvasHeroes _canvasHeroes;
        private List<HeroTypeId> _newSoldierType = new List<HeroTypeId>();
        private UpgradeData _upgradeData;
        private int _maxCountSqaud = 9;

        public List<Soldier> Soldiers => _soldiers;
        public List<SoldierCard> SoldierСards => _soldierСard;
        public Heroes Heroes => _heroes;

        public event Action ChangedSquad;
        public event Action SetMarker;
        public event Action<SoldierCard> AddCountCardSquad;

        private void OnEnable()
        {
            _heroes.AddSoldierCard += AddSoldier;
            _canvasHeroes.Activate += ActivateteCameraSoldier;
            _canvasHeroes.Deactivate += DiactivateCameraSoldier;
        }

        private void OnDisable()
        {
            _heroes.AddSoldierCard -= AddSoldier;
            _canvasHeroes.Activate -= ActivateteCameraSoldier;
            _canvasHeroes.Deactivate -= DiactivateCameraSoldier;
            //_upgradeData.ChangedCharacteristics -= Call;
        }

        public void Construct(UpgradeData upgradeData)
        {
            _upgradeData = upgradeData;
            _upgradeData.ChangedCharacteristics += Call;
        }

        public void Close()
        {
            _upgradeData.ChangedCharacteristics -= Call;
        }

        public void ClirListSoldier()
        {
            _soldiers.Clear();
            _soldierСard.Clear();
        }

        public void Call()
        {
            ChangedSquad?.Invoke();
        }

        public void AddSoldier(SoldierCard soldierСard)
        {
            _soldiers.Add(soldierСard.Soldier);
            _newSoldierType.Add(soldierСard.SoldierCardViewer.Card.Soldier.HeroTypeId);
            ChangedSquad?.Invoke();
            soldierСard.Soldier.DataSoldier.SetCardActivation();
            soldierСard.Soldier.DataSoldier.AddSquad();
            _soldierСard.Add(soldierСard);
            soldierСard.SoldierCardViewer.SetComponent();
            soldierСard.SoldierCardViewer.SaveDataHero();
            ShowTextCountSquad();
            _heroes.Save();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _soldiers = progress.SquadPlayer.SoldiersPlayer;
            _newSoldierType = progress.SquadPlayer.HeroTypeIds;
            if (SceneManager.GetActiveScene().name == AssetPath.SceneMain)
            {

                ActivatedSoldierCardCardAll(_newSoldierType, progress);
                if (_soldiers.Count == 0 && _soldierСard.Count > 0)
                {
                    ActivateSoldierCardSquad(_newSoldierType, progress);
                }
                else if (_soldiers.Count > 0 && _soldierСard.Count == 0)
                {
                    ActivatedSoldierCardCardAll(_newSoldierType, progress);
                }
                else if (_soldiers.Count == 0 && _soldierСard.Count == 0)
                {
                    ActivatedSoldierCardCardAll(_newSoldierType, progress);
                }

                foreach (var item in _soldierСard)
                {
                    AddCountCardSquad?.Invoke(item);
                }
            }
            Call();
            ShowTextCountSquad();
            SetMarker?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.SquadPlayer.SoldiersPlayer = _soldiers;
            progress.SquadPlayer.HeroTypeIds = _newSoldierType;
        }

        public void ActivateteCameraSoldier()
        {
            foreach (SoldierCard item in _soldierСard)
            {
                item.ActivateCamera();
            }
        }

        public void DiactivateCameraSoldier()
        {
            foreach (SoldierCard item in _soldierСard)
            {
                item.DiactivateCamera();
            }
        }

        public void TransferringCardHeroes(SoldierCard soldierСard)
        {
            soldierСard.transform.SetParent(_containerHero.transform);
        }

        public void RemoveFromSquad(Soldier soldier)
        {
            List<SoldierCard> soldierСards = new List<SoldierCard>();
            List<Soldier> soldiers = new List<Soldier>();
            List<HeroTypeId> typeIds = new List<HeroTypeId>();

            foreach (SoldierCard soldiercard in _soldierСard)
            {
                if(soldier.HeroTypeId == soldiercard.SoldierCardViewer.Card.Soldier.HeroTypeId)
                {
                    _heroes.AddCardSoldier(soldiercard);
                    TransferringCardHeroes(soldiercard);
                    soldier.DataSoldier.RemuveAquad();
                    foreach (Soldier item in _soldiers)
                    {
                        if(soldier.HeroTypeId != item.HeroTypeId)
                        {
                            soldiers.Add(item);
                            typeIds.Add(item.HeroTypeId);
                        }
                    }
                }
                else
                {
                    soldierСards.Add(soldiercard);
                }
            }
            _soldiers.Clear();
            _soldierСard.Clear();
            _newSoldierType.Clear();
            _soldiers = soldiers;
            _newSoldierType = typeIds;
            _soldierСard = soldierСards;
            ShowTextCountSquad();
            ChangedSquad?.Invoke();
            _heroes.Save();
        }

        private void ActivatedSoldierCardCardAll(List<HeroTypeId>soldier, PlayerProgress progress)
        {
            List<Soldier> soldierUnits = new List<Soldier>();
            for (int i = 0; i < soldier.Count; i++)
            {
                for(int j = 0; j < _heroes.HerosCards.SoldierСardsAll.Count;j++)
                {
                    if (soldier[i] == _heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier.HeroTypeId)
                    {
                        _soldierСard.Add(_heroes.HerosCards.SoldierСardsAll[j]);
                        soldierUnits.Add(_heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier);
                        _heroes.TransferringCardSquad(_heroes.HerosCards.SoldierСardsAll[j]);
                        _heroes.HerosCards.SoldierСardsAll[j].SetSoliedrMap(_heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier);
                        int countCards = progress.PlayerData.TypeHero.AllHerosType.GetCountCardsHero(_heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier);
                        _heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier.Rank.CurrentCountCard = countCards;
                        _heroes.HerosCards.SoldierСardsAll[j].Soldier.DataSoldier.SetCardActivation();
                        _heroes.HerosCards.SoldierСardsAll[j].Soldier.DataSoldier.AddSquad();
                        _heroes.HerosCards.SoldierСardsAll[j].gameObject.SetActive(true);
                        _heroes.HerosCards.RemoveCard(_heroes.HerosCards.SoldierСardsAll[j]);
                    }
                }
            }
            _soldiers.Clear();
            _soldiers = soldierUnits;
        }

        private void ActivateSoldierCardSquad(List<HeroTypeId> soldier, PlayerProgress progress)
        {
            List<Soldier> soldierUnits = new List<Soldier>();
            for (int i = 0; i < soldier.Count; i++)
            {
                for (int j = 0; j < _soldierСard.Count; j++)
                {
                    if (soldier[i] == _soldierСard[j].SoldierCardViewer.Card.Soldier.HeroTypeId)
                    {
                        soldierUnits.Add(_soldierСard[j].SoldierCardViewer.Card.Soldier);
                        _soldierСard[j].SetSoliedrMap(_heroes.HerosCards.SoldierСardsAll[j].SoldierCardViewer.Card.Soldier);
                        int countCards = progress.PlayerData.TypeHero.AllHerosType.GetCountCardsHero(_soldierСard[j].SoldierCardViewer.Card.Soldier);
                        _soldierСard[j].SoldierCardViewer.Card.Soldier.Rank.CurrentCountCard = countCards;
                        _soldierСard[j].Soldier.DataSoldier.SetCardActivation();
                        _soldierСard[j].Soldier.DataSoldier.AddSquad();
                        _soldierСard[j].gameObject.SetActive(true);
                        _heroes.HerosCards.RemoveCard(_soldierСard[j]);
                    }
                }
            }
            _soldiers.Clear();
            _soldiers = soldierUnits;
        }

        private void ShowTextCountSquad()
        {
            _textCountSquad.text = TextSquadCount +" "+ _soldiers.Count.ToString() +"/"+_maxCountSqaud;
        }
    }
}
