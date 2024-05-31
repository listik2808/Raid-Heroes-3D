using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerWarrior : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataWarrior _soldierDataWarrior;
        private Warrior _warrior;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _warrior.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _warrior.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _warrior.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _warrior.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _warrior.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_warrior.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _warrior.Rank.UpdateLevelHero -= SaveDataHero;
            _warrior.Rank.ChangeCountCard -= SaveDataHero;
            _warrior.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataWarrior.CloseScreen();
            _soldierDataWarrior.enabled = false;
            _soldierDataWarrior.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataWarrior.SetCard(this);
            GetData();
            _soldierDataWarrior.gameObject.SetActive(true);
            _soldierDataWarrior.enabled = true;
            _soldierDataWarrior.SetSoldier(CameraView);
            _soldierDataWarrior.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _warrior.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _warrior = (Warrior)Card.Soldier;
            if (_warrior.Rank.CurrentLevelHero >= 0)
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
            _soldierDataWarrior.Construct(_warrior, _progressService, CameraView);
            _soldierDataWarrior.SetSoldier(CameraView);
            _soldierDataWarrior.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataWarrior.AllMarkerSpecAttak + _soldierDataWarrior.AllUpRangMarker
             + _soldierDataWarrior.AllMarkerSurvability + _soldierDataWarrior.AllMarkerMeleeDamage;
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
            if (_warrior == null)
            {
                _warrior = (Warrior)Card.Soldier;
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
            _warrior.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _warrior.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _warrior.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _warrior.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _warrior.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_warrior.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _warrior.Rank.UpdateLevelHero += SaveDataHero;
            _warrior.Rank.ChangeCountCard += SaveDataHero;
            _warrior.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataWarrior.SetComponent(_warrior,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenSpecialAttackLevel = _warrior.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenSurvivabilityLevel = _warrior.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenMeleeLevel = _warrior.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.WarriorHero.CurrenSpeedLevel = _warrior.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepSpecialAttack = _warrior.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepSurvivability = _warrior.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepMelee = _warrior.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.WarriorHero.CurrentStepMobility = _warrior.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.BestLevelSpecialSkill = _warrior.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.BestSurvivalRateLevel = _warrior.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.BestMeleerateLevel = _warrior.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.WarriorHero.BestLevelMobility = _warrior.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentLevelHero = _warrior.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.UnitOpened = _warrior.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.Hired = _warrior.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentCountCard = _warrior.Rank.CurrentCountCard;
        }
    }
}