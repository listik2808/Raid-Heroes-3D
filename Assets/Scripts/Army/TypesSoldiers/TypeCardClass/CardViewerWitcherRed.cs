using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerWitcherRed : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataWitcherRed _soldierDataWitcherRed;
        private WitcherRed _witcherRed;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _witcherRed.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _witcherRed.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _witcherRed.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherRed.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherRed.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_witcherRed.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherRed.Rank.UpdateLevelHero -= SaveDataHero;
            _witcherRed.Rank.ChangeCountCard -= SaveDataHero;
            _witcherRed.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataWitcherRed.CloseScreen();
            _soldierDataWitcherRed.enabled = false;
            _soldierDataWitcherRed.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataWitcherRed.SetCard(this);
            GetData();
            _soldierDataWitcherRed.gameObject.SetActive(true);
            _soldierDataWitcherRed.enabled = true;
            _soldierDataWitcherRed.SetSoldier(CameraView);
            _soldierDataWitcherRed.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _witcherRed.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _witcherRed = (WitcherRed)Card.Soldier;
            if (_witcherRed.Rank.CurrentLevelHero >= 0)
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
            _soldierDataWitcherRed.Construct(_witcherRed, _progressService, CameraView);
            _soldierDataWitcherRed.SetSoldier(CameraView);
            _soldierDataWitcherRed.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataWitcherRed.AllMarkerSpecAttak + _soldierDataWitcherRed.AllUpRangMarker
            + _soldierDataWitcherRed.AllMarkerSurvability + _soldierDataWitcherRed.AllMarkerMeleeDamage;
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
            if (_witcherRed == null)
            {
                _witcherRed = (WitcherRed)Card.Soldier;
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
            _witcherRed.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _witcherRed.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _witcherRed.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherRed.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _witcherRed.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_witcherRed.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherRed.Rank.UpdateLevelHero += SaveDataHero;
            _witcherRed.Rank.ChangeCountCard += SaveDataHero;
            _witcherRed.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataWitcherRed.SetComponent(_witcherRed,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenSpecialAttackLevel = _witcherRed.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenSurvivabilityLevel = _witcherRed.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenMeleeLevel = _witcherRed.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.WitcherRedHero.CurrenSpeedLevel = _witcherRed.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepSpecialAttack = _witcherRed.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepSurvivability = _witcherRed.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepMelee = _witcherRed.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.WitcherRedHero.CurrentStepMobility = _witcherRed.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.BestLevelSpecialSkill = _witcherRed.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.BestSurvivalRateLevel = _witcherRed.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.BestMeleerateLevel = _witcherRed.SoldiersStatsLevel.BestMeleerateLevel;
           // PlayerData.TypeHero.WitcherRedHero.BestLevelMobility = _witcherRed.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentLevelHero = _witcherRed.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.UnitOpened = _witcherRed.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.Hired = _witcherRed.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentCountCard = _witcherRed.Rank.CurrentCountCard;
        }
    }
}