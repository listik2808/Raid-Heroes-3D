using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerPrincess : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataPrincess _soldierDataPrincess;
        private Princess _princess;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _princess.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _princess.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _princess.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _princess.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _princess.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_princess.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _princess.Rank.UpdateLevelHero -= SaveDataHero;
            _princess.Rank.ChangeCountCard -= SaveDataHero;
            _princess.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataPrincess.CloseScreen();
            _soldierDataPrincess.enabled = false;
            _soldierDataPrincess.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataPrincess.SetCard(this);
            GetData();
            _soldierDataPrincess.gameObject.SetActive(true);
            _soldierDataPrincess.enabled = true;
            _soldierDataPrincess.SetSoldier(CameraView);
            _soldierDataPrincess.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _princess.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _princess = (Princess)Card.Soldier;
            if (_princess.Rank.CurrentLevelHero >= 0)
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
            _soldierDataPrincess.Construct(_princess, _progressService, CameraView);
            _soldierDataPrincess.SetSoldier(CameraView);
            _soldierDataPrincess.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataPrincess.AllMarkerSpecAttak + _soldierDataPrincess.AllUpRangMarker
             + _soldierDataPrincess.AllMarkerSurvability + _soldierDataPrincess.AllMarkerMeleeDamage;
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
            if (_princess == null)
            {
                _princess = (Princess)Card.Soldier;
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
            _princess.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _princess.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _princess.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _princess.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _princess.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_princess.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _princess.Rank.UpdateLevelHero += SaveDataHero;
            _princess.Rank.ChangeCountCard += SaveDataHero;
            _princess.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataPrincess.SetComponent(_princess,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenSpecialAttackLevel = _princess.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenSurvivabilityLevel = _princess.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenMeleeLevel = _princess.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.PrincessHero.CurrenSpeedLevel = _princess.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepSpecialAttack = _princess.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepSurvivability = _princess.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepMelee = _princess.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.PrincessHero.CurrentStepMobility = _princess.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.BestLevelSpecialSkill = _princess.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.BestSurvivalRateLevel = _princess.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.BestMeleerateLevel = _princess.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.PrincessHero.BestLevelMobility = _princess.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentLevelHero = _princess.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.UnitOpened = _princess.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.Hired = _princess.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentCountCard = _princess.Rank.CurrentCountCard;
        }
    }
}