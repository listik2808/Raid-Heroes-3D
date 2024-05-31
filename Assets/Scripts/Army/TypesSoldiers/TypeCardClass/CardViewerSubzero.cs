using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerSubzero : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataSubzero _soldierDataSubzero;
        private Subzero _subzero;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _subzero.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _subzero.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _subzero.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _subzero.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _subzero.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_subzero.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _subzero.Rank.UpdateLevelHero -= SaveDataHero;
            _subzero.Rank.ChangeCountCard -= SaveDataHero;
            _subzero.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataSubzero.CloseScreen();
            _soldierDataSubzero.enabled = false;
            _soldierDataSubzero.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataSubzero.SetCard(this);
            GetData();
            _soldierDataSubzero.gameObject.SetActive(true);
            _soldierDataSubzero.enabled = true;
            _soldierDataSubzero.SetSoldier(CameraView);
            _soldierDataSubzero.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _subzero.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _subzero = (Subzero)Card.Soldier;
            if (_subzero.Rank.CurrentLevelHero >= 0)
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
            _soldierDataSubzero.Construct(_subzero, _progressService, CameraView);
            _soldierDataSubzero.SetSoldier(CameraView);
            _soldierDataSubzero.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataSubzero.AllMarkerSpecAttak + _soldierDataSubzero.AllUpRangMarker
             + _soldierDataSubzero.AllMarkerSurvability + _soldierDataSubzero.AllMarkerMeleeDamage;
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
            if (_subzero == null)
            {
                _subzero = (Subzero)Card.Soldier;
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
            _subzero.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _subzero.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _subzero.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _subzero.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _subzero.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_subzero.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _subzero.Rank.UpdateLevelHero += SaveDataHero;
            _subzero.Rank.ChangeCountCard += SaveDataHero;
            _subzero.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataSubzero.SetComponent(_subzero,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenSpecialAttackLevel = _subzero.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenSurvivabilityLevel = _subzero.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenMeleeLevel = _subzero.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.SubzeroHero.CurrenSpeedLevel = _subzero.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepSpecialAttack = _subzero.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepSurvivability = _subzero.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepMelee = _subzero.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.SubzeroHero.CurrentStepMobility = _subzero.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.BestLevelSpecialSkill = _subzero.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.BestSurvivalRateLevel = _subzero.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.BestMeleerateLevel = _subzero.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.SubzeroHero.BestLevelMobility = _subzero.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentLevelHero = _subzero.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.UnitOpened = _subzero.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.Hired = _subzero.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentCountCard = _subzero.Rank.CurrentCountCard;
        }
    }
}