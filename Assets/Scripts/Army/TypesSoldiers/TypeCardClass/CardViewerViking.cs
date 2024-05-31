using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerViking : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierViking _soldierDataViking;
        private Viking _viking;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _viking.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _viking.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _viking.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _viking.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _viking.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_viking.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _viking.Rank.UpdateLevelHero -= SaveDataHero;
            _viking.Rank.ChangeCountCard -= SaveDataHero;
            _viking.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataViking.CloseScreen();
            _soldierDataViking.enabled = false;
            _soldierDataViking.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataViking.SetCard(this);
            GetData();
            _soldierDataViking.gameObject.SetActive(true);
            _soldierDataViking.enabled = true;
            _soldierDataViking.SetSoldier(CameraView);
            _soldierDataViking.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _viking.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _viking = (Viking)Card.Soldier;
            if (_viking.Rank.CurrentLevelHero >= 0)
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
            _soldierDataViking.Construct(_viking, _progressService, CameraView);
            _soldierDataViking.SetSoldier(CameraView);
            _soldierDataViking.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataViking.AllMarkerSpecAttak + _soldierDataViking.AllUpRangMarker
             + _soldierDataViking.AllMarkerSurvability + _soldierDataViking.AllMarkerMeleeDamage;
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
            if (_viking == null)
            {
                _viking = (Viking)Card.Soldier;
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
            _viking.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _viking.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _viking.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _viking.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _viking.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_viking.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _viking.Rank.UpdateLevelHero += SaveDataHero;
            _viking.Rank.ChangeCountCard += SaveDataHero;
            _viking.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataViking.SetComponent(_viking,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenSpecialAttackLevel = _viking.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenSurvivabilityLevel = _viking.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenMeleeLevel = _viking.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.VikingHero.CurrenSpeedLevel = _viking.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepSpecialAttack = _viking.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepSurvivability = _viking.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepMelee = _viking.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.VikingHero.CurrentStepMobility = _viking.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.VikingHero.BestLevelSpecialSkill = _viking.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.BestSurvivalRateLevel = _viking.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.BestMeleerateLevel = _viking.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.VikingHero.BestLevelMobility = _viking.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentLevelHero = _viking.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.UnitOpened = _viking.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.VikingHero.Hired = _viking.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentCountCard = _viking.Rank.CurrentCountCard;
        }
    }
}