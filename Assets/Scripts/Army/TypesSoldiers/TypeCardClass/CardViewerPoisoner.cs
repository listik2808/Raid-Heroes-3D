using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerPoisoner : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataPoisoner _soldierDataPoisoner;
        private Poisoner _poisoner;
        private IPersistenProgressService _persistenProgressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _poisoner.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _poisoner.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _poisoner.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _poisoner.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _poisoner.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_poisoner.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _poisoner.Rank.UpdateLevelHero -= SaveDataHero;
            _poisoner.Rank.ChangeCountCard -= SaveDataHero;
            _poisoner.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataPoisoner.CloseScreen();
            _soldierDataPoisoner.enabled = false;
            _soldierDataPoisoner.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataPoisoner.SetCard(this);
            GetData();
            _soldierDataPoisoner.gameObject.SetActive(true);
            _soldierDataPoisoner.enabled = true;
            _soldierDataPoisoner.SetSoldier(CameraView);
            _soldierDataPoisoner.SetDataSoldier(CameraView,_persistenProgressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _poisoner.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _poisoner = (Poisoner)Card.Soldier;
            if (_poisoner.Rank.CurrentLevelHero >= 0)
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
            _soldierDataPoisoner.Construct(_poisoner, _persistenProgressService, CameraView);
            _soldierDataPoisoner.SetSoldier(CameraView);
            _soldierDataPoisoner.SetValuesCharacteristics(CameraView,_persistenProgressService);
            _allMarker = _soldierDataPoisoner.AllMarkerSpecAttak + _soldierDataPoisoner.AllUpRangMarker
             + _soldierDataPoisoner.AllMarkerSurvability + _soldierDataPoisoner.AllMarkerMeleeDamage;
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
            if (_poisoner == null)
            {
                _poisoner = (Poisoner)Card.Soldier;
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
            if (_persistenProgressService == null)
                _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
            if (_saveLoadService == null)
                _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void GetData()
        {
            _poisoner.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _poisoner.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _poisoner.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _poisoner.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _poisoner.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_poisoner.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _poisoner.Rank.UpdateLevelHero += SaveDataHero;
            _poisoner.Rank.ChangeCountCard += SaveDataHero;
            _poisoner.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataPoisoner.SetComponent(_poisoner,_persistenProgressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenSpecialAttackLevel = _poisoner.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenSurvivabilityLevel = _poisoner.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenMeleeLevel = _poisoner.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.PoisonerHero.CurrenSpeedLevel = _poisoner.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepSpecialAttack = _poisoner.SpecialSkillLevelData.CurrentStepSkill;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepSurvivability = _poisoner.SurvivabilityLevelData.CurrentStepSkill;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepMelee = _poisoner.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.PoisonerHero.CurrentStepMobility = _poisoner.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.BestLevelSpecialSkill = _poisoner.SoldiersStatsLevel.BestLevelSpecialSkill;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.BestSurvivalRateLevel = _poisoner.SoldiersStatsLevel.BestSurvivalRateLevel;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.BestMeleerateLevel = _poisoner.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.PoisonerHero.BestLevelMobility = _poisoner.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentLevelHero = _poisoner.Rank.CurrentLevelHero;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.UnitOpened = _poisoner.UnitOpened;
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.Hired = _poisoner.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _persistenProgressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentCountCard = _poisoner.Rank.CurrentCountCard;
        }
    }
}