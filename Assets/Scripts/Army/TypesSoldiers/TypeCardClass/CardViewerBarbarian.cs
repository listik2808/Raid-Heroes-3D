using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerBarbarian : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataBarbarian _soldierDataBarbarian;
        private Barbarian _barbarian;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _barbarian.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _barbarian.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _barbarian.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _barbarian.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _barbarian.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_barbarian.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _barbarian.Rank.UpdateLevelHero -= SaveDataHero;
            _barbarian.Rank.ChangeCountCard -= SaveDataHero;
            _barbarian.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataBarbarian.CloseScreen();
            _soldierDataBarbarian.enabled = false;
            _soldierDataBarbarian.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataBarbarian.SetCard(this);
            GetData();
            _soldierDataBarbarian.gameObject.SetActive(true);
            _soldierDataBarbarian.enabled = true;
            _soldierDataBarbarian.SetSoldier(CameraView);
            _soldierDataBarbarian.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _barbarian.DataSoldier.SoldierСard.CameraParent;
            }

            SetComponentSevices();
            _barbarian = (Barbarian)Card.Soldier;
            if (_barbarian.Rank.CurrentLevelHero >= 0)
            {
                SetbaseComponent();
            }
            else
            {
                SetBaseComponentZoro();
            }
        }

        public override void SetComponent()
        {
            _allMarker = 0;
            _soldierDataBarbarian.Construct(_barbarian, _progressService,CameraView);
            _soldierDataBarbarian.SetSoldier(CameraView);
            _soldierDataBarbarian.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataBarbarian.AllMarkerSpecAttak + _soldierDataBarbarian.AllUpRangMarker
              + _soldierDataBarbarian.AllMarkerSurvability + _soldierDataBarbarian.AllMarkerMeleeDamage;
            if (_allMarker > 0)
            {
                InfoMarker.gameObject.SetActive(true);
                InfoText.text = _allMarker.ToString();
            }
            else
            {
                InfoMarker.gameObject.SetActive(false);
            }
        }

        private void GetData()
        {
            _barbarian.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _barbarian.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _barbarian.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _barbarian.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _barbarian.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_barbarian.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _barbarian.Rank.UpdateLevelHero += SaveDataHero;
            _barbarian.Rank.ChangeCountCard += SaveDataHero;
            _barbarian.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataBarbarian.SetComponent(_barbarian,_progressService, CameraView);
        }

        public override void SaveDataHero()
        {
            SetComponentSevices();
            if (_barbarian == null)
            {
                _barbarian = (Barbarian)Card.Soldier;
            }
            SaveBestLevelSkill();
            SaveCountCards();
            SaveCurrentLevelSkill();
            SaveCurrentStepSkill();
            SaveLevelHero();
            _saveLoadService.SaveProgress();
        }
        public override void SetComponentSevices()
        {
            if (_progressService == null)
                _progressService = AllServices.Container.Single<IPersistenProgressService>();
            if (_saveLoadService == null)
                _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }
        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenSpecialAttackLevel = _barbarian.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenSurvivabilityLevel = _barbarian.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenMeleeLevel = _barbarian.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.BarbarianHero.CurrenSpeedLevel = _barbarian.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepSpecialAttack = _barbarian.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepSurvivability = _barbarian.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepMelee = _barbarian.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.BarbarianHero.CurrentStepMobility = _barbarian.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.BestLevelSpecialSkill = _barbarian.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.BestSurvivalRateLevel = _barbarian.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.BestMeleerateLevel = _barbarian.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.BarbarianHero.BestLevelMobility = _barbarian.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentLevelHero = _barbarian.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.UnitOpened = _barbarian.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.Hired = _barbarian.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentCountCard = _barbarian.Rank.CurrentCountCard;
        }
    }
}