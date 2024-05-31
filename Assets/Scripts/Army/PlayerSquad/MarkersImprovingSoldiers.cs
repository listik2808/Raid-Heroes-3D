using Scripts.Army.AllCadsHeroes;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Army.PlayerSquad
{
    public class MarkersImprovingSoldiers : MonoBehaviour
    {
        [SerializeField] private Squad _squad;
        [SerializeField] private Image _infoMarker;
        [SerializeField] private TMP_Text _infoTextMarker;
        [SerializeField] private Heroes _heroes;
        [SerializeField] private HerosCards _heroCards;
        [SerializeField] private ActivUpgradeSoldier _upgradeSoldier;
        private IPersistenProgressService _progressService;
        private int _allMarker = 0;
        private bool _isActiv = false;

        private void OnEnable()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            _progressService.Progress.Wallet.Coins.OnValueChanged += SetMarkers;
            _progressService.Progress.Wallet.Diamonds.OnValueChanged += SetMarkers;
            _progressService.Progress.Wallet.Stars.OnValueChanged += SetMarkers;
            _heroes.AddCardCountCard += Add;
            _squad.AddCountCardSquad += Add;
            _squad.SetMarker += SetMarkers;
            _heroes.StartHeroMarker += SetMarkers;
        }

        private void OnDisable()
        {
            _progressService.Progress.Wallet.Coins.OnValueChanged -= SetMarkers;
            _progressService.Progress.Wallet.Diamonds.OnValueChanged -= SetMarkers;
            _progressService.Progress.Wallet.Stars.OnValueChanged -= SetMarkers;
            _heroes.AddCardCountCard -= Add;
            _squad.AddCountCardSquad -= Add;
            Remove(_squad.SoldierСards);
            Remove(_heroes.SoldierСards);
            _squad.SetMarker -= SetMarkers;
            _heroes.StartHeroMarker -= SetMarkers;
        }

        public void ActivateRecalculation()
        {
            _isActiv = false;
            SetMarkers();
        }

        private void SetMarkers()
        {
            _allMarker = 0;

            foreach (var item in _upgradeSoldier.UpgradeDatas)
            {
                if (item.enabled == true)
                {
                    _isActiv = true;
                    item.SoldierCardViewer.Start();
                    item.SoldierCardViewer.SetComponent();
                    _allMarker += item.SoldierCardViewer.AllMarker;
                }
            }

            if (_isActiv == false)
            {
                SetCountMarkersHeroes();
                SetCountMarkersSquad();
                TrySetCountMarkersButtonMenu();
            }
        }

        private void Remove(List<SoldierCard> soldierCards)
        {
            foreach (SoldierCard item in soldierCards)
            {
                item.Soldier.Rank.AddCardCount -= ChekCountCard;
                item.Soldier.Rank.LevelUp -= SetMarkers;
            }
        }

        private void Add(SoldierCard soldierCard)
        {
            soldierCard.Soldier.Rank.SetSoldier(soldierCard.SoldierCardViewer.Card.Soldier);
            soldierCard.Soldier.Rank.AddCardCount += ChekCountCard;
            soldierCard.Soldier.Rank.LevelUp += SetMarkers;
        }

        private void ChekCountCard(Soldier soldier)
        {
            _allMarker = 0;
            SetCountMarkersHeroes();
            SetCountMarkersSquad();
            TrySetCountMarkersButtonMenu();
            if (SceneManager.GetActiveScene().name == AssetPath.SceneMain)
            {
                TrySetCountMarkersButtonMenu();
            }
        }

        private void TrySetCountMarkersButtonMenu()
        {
            if (_allMarker > 0)
            {
                _infoMarker.gameObject.SetActive(true);
                _infoTextMarker.text = _allMarker.ToString();
            }
            else
            {
                _infoMarker.gameObject.SetActive(false);
            }
        }

        private void SetCountMarkersSquad()
        {
            if (_squad.SoldierСards != null && _squad.SoldierСards.Count > 0)
            {
                foreach (var item in _squad.SoldierСards)
                {
                    item.SoldierCardViewer.Start();
                    item.SoldierCardViewer.SetComponent();
                    _allMarker += item.SoldierCardViewer.AllMarker;
                }
            }
        }

        private void SetCountMarkersHeroes()
        {
            if (_heroes.SoldierСards != null && _heroes.SoldierСards.Count > 0)
            {
                foreach (var itemHeros in _heroes.SoldierСards)
                {
                    itemHeros.SoldierCardViewer.Start();
                    itemHeros.SoldierCardViewer.SetComponent();
                    _allMarker += itemHeros.SoldierCardViewer.AllMarker;
                }
            }
        }
    }
}
