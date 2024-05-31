using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerWitcherBlue : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataWitcherBlue _soldierDataWitcherBlue;
        private WitcherBlue _witcherBlue;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _witcherBlue.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _witcherBlue.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _witcherBlue.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherBlue.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherBlue.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_witcherBlue.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _witcherBlue.Rank.UpdateLevelHero -= SaveDataHero;
            _witcherBlue.Rank.ChangeCountCard -= SaveDataHero;
            _witcherBlue.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataWitcherBlue.CloseScreen();
            _soldierDataWitcherBlue.enabled = false;
            _soldierDataWitcherBlue.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataWitcherBlue.SetCard(this);
            GetData();
            _soldierDataWitcherBlue.gameObject.SetActive(true);
            _soldierDataWitcherBlue.enabled = true;
            _soldierDataWitcherBlue.SetSoldier(CameraView);
            _soldierDataWitcherBlue.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _witcherBlue.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _witcherBlue = (WitcherBlue)Card.Soldier;
            
            if (_witcherBlue.Rank.CurrentLevelHero >= 0)
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
            _soldierDataWitcherBlue.Construct(_witcherBlue, _progressService, CameraView);
            _soldierDataWitcherBlue.SetSoldier(CameraView);
            _soldierDataWitcherBlue.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataWitcherBlue.AllMarkerSpecAttak + _soldierDataWitcherBlue.AllUpRangMarker
             + _soldierDataWitcherBlue.AllMarkerSurvability + _soldierDataWitcherBlue.AllMarkerMeleeDamage;
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
            if (_witcherBlue == null)
            {
                _witcherBlue = (WitcherBlue)Card.Soldier;
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
            _witcherBlue.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _witcherBlue.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _witcherBlue.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherBlue.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _witcherBlue.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_witcherBlue.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _witcherBlue.Rank.UpdateLevelHero += SaveDataHero;
            _witcherBlue.Rank.ChangeCountCard += SaveDataHero;
            _witcherBlue.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataWitcherBlue.SetComponent(_witcherBlue,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenSpecialAttackLevel = _witcherBlue.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenSurvivabilityLevel = _witcherBlue.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenMeleeLevel = _witcherBlue.SoldiersStatsLevel.CurrentMeleelevel;
           // PlayerData.TypeHero.WitcherBlueHero.CurrenSpeedLevel = _witcherBlue.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepSpecialAttack = _witcherBlue.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepSurvivability = _witcherBlue.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepMelee = _witcherBlue.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.WitcherBlueHero.CurrentStepMobility = _witcherBlue.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.BestLevelSpecialSkill = _witcherBlue.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.BestSurvivalRateLevel = _witcherBlue.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.BestMeleerateLevel = _witcherBlue.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.WitcherBlueHero.BestLevelMobility = _witcherBlue.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentLevelHero = _witcherBlue.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.UnitOpened = _witcherBlue.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.Hired = _witcherBlue.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentCountCard = _witcherBlue.Rank.CurrentCountCard;
        }
    }
}