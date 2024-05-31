using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerFairy : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataFairy _soldierDataFairy;
        private Fairy _fairy;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _fairy.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _fairy.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _fairy.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _fairy.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _fairy.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_fairy.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _fairy.Rank.UpdateLevelHero -= SaveDataHero;
            _fairy.Rank.ChangeCountCard -= SaveDataHero;
            _fairy.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataFairy.CloseScreen();
            _soldierDataFairy.enabled = false;
            _soldierDataFairy.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataFairy.SetCard(this);
            GetData();
            _soldierDataFairy.gameObject.SetActive(true);
            _soldierDataFairy.enabled = true;
            _soldierDataFairy.SetSoldier(CameraView);
            _soldierDataFairy.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _fairy.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _fairy = (Fairy)Card.Soldier;
            if (_fairy.Rank.CurrentLevelHero >= 0)
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
            _soldierDataFairy.Construct(_fairy, _progressService, CameraView);
            _soldierDataFairy.SetSoldier(CameraView);
            _soldierDataFairy.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataFairy.AllMarkerSpecAttak + _soldierDataFairy.AllUpRangMarker
              + _soldierDataFairy.AllMarkerSurvability + _soldierDataFairy.AllMarkerMeleeDamage;
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
            if (_fairy == null)
            {
                _fairy = (Fairy)Card.Soldier;
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
            _fairy.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _fairy.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _fairy.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _fairy.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _fairy.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_fairy.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _fairy.Rank.UpdateLevelHero += SaveDataHero;
            _fairy.Rank.ChangeCountCard += SaveDataHero;
            _fairy.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataFairy.SetComponent(_fairy,_progressService, CameraView);
            
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenSpecialAttackLevel = _fairy.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenSurvivabilityLevel = _fairy.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenMeleeLevel = _fairy.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.FairyHero.CurrenSpeedLevel = _fairy.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepSpecialAttack = _fairy.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepSurvivability = _fairy.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepMelee = _fairy.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.FairyHero.CurrentStepMobility = _fairy.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.FairyHero.BestLevelSpecialSkill = _fairy.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.BestSurvivalRateLevel = _fairy.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.BestMeleerateLevel = _fairy.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.FairyHero.BestLevelMobility = _fairy.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentLevelHero = _fairy.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.UnitOpened = _fairy.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.FairyHero.Hired = _fairy.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentCountCard = _fairy.Rank.CurrentCountCard;
        }
    }
}