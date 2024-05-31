using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerWitcherGreen : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataWitcherGreen _soldierDataWitcherGreen;
        private WitcherGreen _witcherGreen;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _witcherGreen.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _witcherGreen.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _witcherGreen.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherGreen.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherGreen.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_witcherGreen.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherGreen.Rank.UpdateLevelHero -= SaveDataHero;
            _witcherGreen.Rank.ChangeCountCard -= SaveDataHero;
            _witcherGreen.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataWitcherGreen.CloseScreen();
            _soldierDataWitcherGreen.enabled = false;
            _soldierDataWitcherGreen.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataWitcherGreen.SetCard(this);
            GetData();
            _soldierDataWitcherGreen.gameObject.SetActive(true);
            _soldierDataWitcherGreen.enabled = true;
            _soldierDataWitcherGreen.SetSoldier(CameraView);
            _soldierDataWitcherGreen.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _witcherGreen.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _witcherGreen = (WitcherGreen)Card.Soldier;
            
            if (_witcherGreen.Rank.CurrentLevelHero >= 0)
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
            _soldierDataWitcherGreen.Construct(_witcherGreen, _progressService, CameraView);
            _soldierDataWitcherGreen.SetSoldier(CameraView);
            _soldierDataWitcherGreen.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataWitcherGreen.AllMarkerSpecAttak + _soldierDataWitcherGreen.AllUpRangMarker
             + _soldierDataWitcherGreen.AllMarkerSurvability + _soldierDataWitcherGreen.AllMarkerMeleeDamage;
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
            if (_witcherGreen == null)
            {
                _witcherGreen = (WitcherGreen)Card.Soldier;
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
            _witcherGreen.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _witcherGreen.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _witcherGreen.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherGreen.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _witcherGreen.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_witcherGreen.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherGreen.Rank.UpdateLevelHero += SaveDataHero;
            _witcherGreen.Rank.ChangeCountCard += SaveDataHero;
            _witcherGreen.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataWitcherGreen.SetComponent(_witcherGreen,_progressService,CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenSpecialAttackLevel = _witcherGreen.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenSurvivabilityLevel = _witcherGreen.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenMeleeLevel = _witcherGreen.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.WitcherGreenHero.CurrenSpeedLevel = _witcherGreen.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepSpecialAttack = _witcherGreen.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepSurvivability = _witcherGreen.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepMelee = _witcherGreen.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.WitcherGreenHero.CurrentStepMobility = _witcherGreen.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.BestLevelSpecialSkill = _witcherGreen.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.BestSurvivalRateLevel = _witcherGreen.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.BestMeleerateLevel = _witcherGreen.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.WitcherGreenHero.BestLevelMobility = _witcherGreen.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentLevelHero = _witcherGreen.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.UnitOpened = _witcherGreen.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.Hired = _witcherGreen.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentCountCard = _witcherGreen.Rank.CurrentCountCard;
        }
    }
}