using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army.PlayerSquad
{
    public class SoldierCard : MonoBehaviour
    {
        [SerializeField] private SoldierCardViewer _soldierCardViewer;
        [SerializeField] private List<Image> _imageList = new List<Image>();
        [SerializeField] private Sprite _default;
        [SerializeField] private Sprite _upgrade;
        [SerializeField] private CameraParent _cameraParent;
        private Soldier _soldier;
        private IPersistenProgressService _progressService;

        public Soldier Soldier => _soldier;
        public SoldierCardViewer SoldierCardViewer => _soldierCardViewer;
        public CameraParent CameraParent => _cameraParent;

        private void OnEnable()
        {
            LoadCountCard();
            _soldier.Rank.ChangeCountCardRang += ShowCountCard;
            ShowCountCard();
        }

        private void OnDisable()
        {
            _soldier.Rank.ChangeCountCardRang -= ShowCountCard;
        }

        public void ActivateCamera()
        {
            _cameraParent.gameObject.SetActive(true);
        }

        public void DiactivateCamera()
        {
            _cameraParent.gameObject.SetActive(false);
        }

        public void SetSoliedrMap(Soldier soldier)
        {
            _soldier = soldier;
            
            _soldierCardViewer.gameObject.SetActive(true);
            _soldier.DataSoldier.SetCardSoldier(this);
            soldier.OpenHeroCard(true);
        }

        public void ShowCountCard()
        {
            _soldierCardViewer.Text.text = _soldierCardViewer.Card.Soldier.Rank.CurrentCountCard.ToString() + " / " + _soldierCardViewer.Card.Soldier.Rank.MaxCountCard.ToString();
            RenderingRank();
        }

        private void LoadCountCard()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            foreach (var item in _progressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
            {
                if(item.TypeId == _soldier.HeroTypeId)
                {
                    int value = item.CurrentCountCard;
                    int level = item.CurrentLevelHero;
                    _soldier.Rank.SetLevelHero(level);
                    int valueMaxCard = item.GetCurrentMaxCountCard();
                    _soldier.Rank.SetMaxCountCard(valueMaxCard);
                }
            }
        }

        private void RenderingRank()
        {
            int count = _soldier.Rank.CurrentLevelHero;
            if (count > 0)
            {
                for (int i = 0; i < _imageList.Count; i++)
                {
                    if (count > 0)
                    {
                        _imageList[i].sprite = _upgrade;
                        count--;
                    }
                }
            }
            else
            {
                foreach (var item in _imageList)
                {
                    item.sprite = _default;
                }
            }
        }
    }
}
