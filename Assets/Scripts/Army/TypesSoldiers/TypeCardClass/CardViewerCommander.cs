using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerCommander : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataCommander _soldierDataCommander;
        private Commander _commander;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _commander.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _commander.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _commander.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _commander.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _commander.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_commander.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _commander.Rank.UpdateLevelHero -= SaveDataHero;
            _commander.Rank.ChangeCountCard -= SaveDataHero;
            _commander.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataCommander.CloseScreen();
            _soldierDataCommander.enabled = false;
            _soldierDataCommander.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataCommander.SetCard(this);
            GetData();
            _soldierDataCommander.gameObject.SetActive(true);
            _soldierDataCommander.enabled = true;
            _soldierDataCommander.SetSoldier(CameraView);
            _soldierDataCommander.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _commander.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _commander = (Commander)Card.Soldier;
            if (_commander.Rank.CurrentLevelHero >= 0)
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
            _soldierDataCommander.Construct(_commander, _progressService, CameraView);
            _soldierDataCommander.SetSoldier(CameraView);
            _soldierDataCommander.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataCommander.AllMarkerSpecAttak + _soldierDataCommander.AllUpRangMarker
              + _soldierDataCommander.AllMarkerSurvability + _soldierDataCommander.AllMarkerMeleeDamage;
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
            if (_commander == null)
            {
                _commander = (Commander)Card.Soldier;
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
            _commander.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _commander.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _commander.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _commander.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _commander.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_commander.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _commander.Rank.UpdateLevelHero += SaveDataHero;
            _commander.Rank.ChangeCountCard += SaveDataHero;
            _commander.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataCommander.SetComponent(_commander,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenSpecialAttackLevel = _commander.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenSurvivabilityLevel = _commander.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenMeleeLevel = _commander.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.CommanderHero.CurrenSpeedLevel = _commander.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepSpecialAttack = _commander.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepSurvivability = _commander.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepMelee = _commander.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.CommanderHero.CurrentStepMobility = _commander.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.BestLevelSpecialSkill = _commander.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.BestSurvivalRateLevel = _commander.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.BestMeleerateLevel = _commander.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.CommanderHero.BestLevelMobility = _commander.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentLevelHero = _commander.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.UnitOpened = _commander.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.Hired = _commander.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentCountCard = _commander.Rank.CurrentCountCard;
        }
    }
}