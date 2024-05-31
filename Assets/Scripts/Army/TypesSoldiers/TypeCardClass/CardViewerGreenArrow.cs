using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerGreenArrow : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataGreenArrow _soldierDataGreenArrow;
        private GreenArrow _greenArrow;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _greenArrow.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _greenArrow.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _greenArrow.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _greenArrow.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _greenArrow.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            // _greenArrow.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _greenArrow.Rank.UpdateLevelHero -= SaveDataHero;
            _greenArrow.Rank.ChangeCountCard -= SaveDataHero;
            _greenArrow.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataGreenArrow.CloseScreen();
            _soldierDataGreenArrow.enabled = false;
            _soldierDataGreenArrow.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataGreenArrow.SetCard(this);
            GetData();
            _soldierDataGreenArrow.gameObject.SetActive(true);
            _soldierDataGreenArrow.enabled = true;
            _soldierDataGreenArrow.SetSoldier(CameraView);
            _soldierDataGreenArrow.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _greenArrow.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _greenArrow = (GreenArrow)Card.Soldier;
            if (_greenArrow.Rank.CurrentLevelHero >= 0)
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
            _soldierDataGreenArrow.Construct(_greenArrow, _progressService, CameraView);
            _soldierDataGreenArrow.SetSoldier(CameraView);
            _soldierDataGreenArrow.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataGreenArrow.AllMarkerSpecAttak + _soldierDataGreenArrow.AllUpRangMarker
              + _soldierDataGreenArrow.AllMarkerSurvability + _soldierDataGreenArrow.AllMarkerMeleeDamage;
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
            if (_greenArrow == null)
            {
                _greenArrow = (GreenArrow)Card.Soldier;
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
            _greenArrow.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _greenArrow.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _greenArrow.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _greenArrow.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _greenArrow.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_greenArrow.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _greenArrow.Rank.UpdateLevelHero += SaveDataHero;
            _greenArrow.Rank.ChangeCountCard += SaveDataHero;
            _greenArrow.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataGreenArrow.SetComponent(_greenArrow,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenSpecialAttackLevel = _greenArrow.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenSurvivabilityLevel = _greenArrow.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenMeleeLevel = _greenArrow.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.GreenArrowHero.CurrenSpeedLevel = _greenArrow.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepSpecialAttack = _greenArrow.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepSurvivability = _greenArrow.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepMelee = _greenArrow.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.GreenArrowHero.CurrentStepMobility = _greenArrow.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.BestLevelSpecialSkill = _greenArrow.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.BestSurvivalRateLevel = _greenArrow.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.BestMeleerateLevel = _greenArrow.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.GreenArrowHero.BestLevelMobility = _greenArrow.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentLevelHero = _greenArrow.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.UnitOpened = _greenArrow.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.Hired = _greenArrow.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentCountCard = _greenArrow.Rank.CurrentCountCard;
        }
    }
}