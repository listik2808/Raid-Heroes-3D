using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Logic.TaskAchievements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Logic.CastleConstruction
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private List<ConstructionCastle> _constructionCastleList;
        [SerializeField] private Image _imageInfo;
        [SerializeField] private TMP_Text _textCount;
        [SerializeField] private BuildBuildingsCastleAchievement _buildBuildingsCastleAchievement;
        [SerializeField] private ImproveBuildingsCastleAchievement _improveBuildingsCastleAchievement;
        private int _count = 0;
        private int _temporaryCounter = 0;
        private IPersistenProgressService _progressService;
        private bool _isLoad = false;

        private void OnEnable()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            _progressService.Progress.Wallet.Coins.OnValueChanged += TrySetMarker;
            foreach (var c in _constructionCastleList)
            {
                c.MarketChanged += TrySetMarker;
            }
            AddLisinerOpenCardBuilding();
            AddLisinerImpruvementBuilding();
        }

        private void OnDisable()
        {
            _progressService.Progress.Wallet.Coins.OnValueChanged -= TrySetMarker;
            foreach (var c in _constructionCastleList)
            {
                c.MarketChanged -= TrySetMarker;
            }
            RemuveLisinerOpenCardBuilding();
            RemuveLisinerImpruvementBuilding();
        }

        private void Start()
        {
            foreach (var item in _constructionCastleList)
            {
                item.LoadData(_progressService);
            }
            TrySetMarker();
            _isLoad = true;
        }

        private void Update()
        {
            if (_isLoad)
            {
                foreach (var item in _constructionCastleList)
                {
                    if (item.IsOpen)
                        item.FillingBar();
                }
            }
        }

        private void AddLisinerImpruvementBuilding()
        {
            foreach (ConstructionCastle item in _constructionCastleList)
            {
                item.ImpruvementBuilding += _improveBuildingsCastleAchievement.GetCountImpruvementBuilding;
            }
        }

        private void RemuveLisinerImpruvementBuilding()
        {
            foreach (ConstructionCastle item in _constructionCastleList)
            {
                item.ImpruvementBuilding -= _improveBuildingsCastleAchievement.GetCountImpruvementBuilding;
            }
        }

        private void AddLisinerOpenCardBuilding()
        {
            foreach (ConstructionCastle item in _constructionCastleList)
            {
                item.OpenBuilding += _buildBuildingsCastleAchievement.SetCountOpenCardBuilding;
            }
        }

        private void RemuveLisinerOpenCardBuilding()
        {
            foreach (ConstructionCastle item in _constructionCastleList)
            {
                item.OpenBuilding -= _buildBuildingsCastleAchievement.SetCountOpenCardBuilding;
            }
        }

        private void TrySetMarker()
        {
            _temporaryCounter = 0;
            foreach (var item in _constructionCastleList)
            {
                item.PossiblePurchase(_progressService);
                _temporaryCounter += item.GetActiveMarker();
                _count = _temporaryCounter;
            }
           

            if (_count > 0)
            {
                _imageInfo.gameObject.SetActive(true);
                _textCount.text = _count.ToString();
            }
            else
            {
                _imageInfo.gameObject.SetActive(false);
            }
        }
    }
}
