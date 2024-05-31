using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerStalker : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataStalker _soldierDataStalker;
        private Stalker _stalker;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _stalker.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _stalker.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _stalker.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _stalker.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _stalker.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_stalker.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _stalker.Rank.UpdateLevelHero -= SaveDataHero;
            _stalker.Rank.ChangeCountCard -= SaveDataHero;
            _stalker.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataStalker.CloseScreen();
            _soldierDataStalker.enabled = false;
            _soldierDataStalker.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataStalker.SetCard(this);
            GetData();
            _soldierDataStalker.gameObject.SetActive(true);
            _soldierDataStalker.enabled = true;
            _soldierDataStalker.SetSoldier(CameraView);
            _soldierDataStalker.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _stalker.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _stalker = (Stalker)Card.Soldier;
            if (_stalker.Rank.CurrentLevelHero >= 0)
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
            _soldierDataStalker.Construct(_stalker, _progressService, CameraView);
            _soldierDataStalker.SetSoldier(CameraView);
            _soldierDataStalker.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataStalker.AllMarkerSpecAttak + _soldierDataStalker.AllUpRangMarker
             + _soldierDataStalker.AllMarkerSurvability + _soldierDataStalker.AllMarkerMeleeDamage;
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
            if (_stalker == null)
            {
                _stalker = (Stalker)Card.Soldier;
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
            _stalker.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _stalker.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _stalker.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _stalker.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _stalker.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_stalker.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _stalker.Rank.UpdateLevelHero += SaveDataHero;
            _stalker.Rank.ChangeCountCard += SaveDataHero;
            _stalker.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataStalker.SetComponent(_stalker,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenSpecialAttackLevel = _stalker.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenSurvivabilityLevel = _stalker.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenMeleeLevel = _stalker.SoldiersStatsLevel.CurrentMeleelevel;
           // PlayerData.TypeHero.StalkerHero.CurrenSpeedLevel = _stalker.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepSpecialAttack = _stalker.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepSurvivability = _stalker.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepMelee = _stalker.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.StalkerHero.CurrentStepMobility = _stalker.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.BestLevelSpecialSkill = _stalker.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.BestSurvivalRateLevel = _stalker.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.BestMeleerateLevel = _stalker.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.StalkerHero.BestLevelMobility = _stalker.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentLevelHero = _stalker.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.UnitOpened = _stalker.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.Hired = _stalker.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentCountCard = _stalker.Rank.CurrentCountCard;
        }
    }
}