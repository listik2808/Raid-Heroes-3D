using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerPriest : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataPriest _soldierDataPriest;
        private Priest _priest;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _priest.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _priest.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _priest.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _priest.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _priest.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_priest.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _priest.Rank.UpdateLevelHero -= SaveDataHero;
            _priest.Rank.ChangeCountCard -= SaveDataHero;
            _priest.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataPriest.CloseScreen();
            _soldierDataPriest.enabled = false;
            _soldierDataPriest.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataPriest.SetCard(this);
            GetData();
            _soldierDataPriest.gameObject.SetActive(true);
            _soldierDataPriest.enabled = true;
            _soldierDataPriest.SetSoldier(CameraView);
            _soldierDataPriest.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _priest.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _priest = (Priest)Card.Soldier;
            if (_priest.Rank.CurrentLevelHero >= 0)
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
            _soldierDataPriest.Construct(_priest, _progressService, CameraView);
            _soldierDataPriest.SetSoldier(CameraView);
            _soldierDataPriest.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataPriest.AllMarkerSpecAttak + _soldierDataPriest.AllUpRangMarker
             + _soldierDataPriest.AllMarkerSurvability + _soldierDataPriest.AllMarkerMeleeDamage;
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

        public override void SaveDataHero()
        {
            SetComponentSevices();
            if (_priest == null)
            {
                _priest = (Priest)Card.Soldier;
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

        private void GetData()
        {
            _priest.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _priest.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _priest.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _priest.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _priest.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_priest.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _priest.Rank.UpdateLevelHero += SaveDataHero;
            _priest.Rank.ChangeCountCard += SaveDataHero;
            _priest.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataPriest.SetComponent(_priest,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenSpecialAttackLevel = _priest.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenSurvivabilityLevel = _priest.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenMeleeLevel = _priest.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.PriestHero.CurrenSpeedLevel = _priest.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepSpecialAttack = _priest.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepSurvivability = _priest.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepMelee = _priest.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.PriestHero.CurrentStepMobility = _priest.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PriestHero.BestLevelSpecialSkill = _priest.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.BestSurvivalRateLevel = _priest.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.BestMeleerateLevel = _priest.SoldiersStatsLevel.BestMeleerateLevel;
           // PlayerData.TypeHero.PriestHero.BestLevelMobility = _priest.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentLevelHero = _priest.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.UnitOpened = _priest.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.PriestHero.Hired = _priest.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentCountCard = _priest.Rank.CurrentCountCard;
        }
    }
}