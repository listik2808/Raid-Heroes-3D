using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System.ComponentModel.Design;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerCharmer : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataCharmer _soldierDataCharmer;
        private Charmer _charmer;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _charmer.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _charmer.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _charmer.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _charmer.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _charmer.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_charmer.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _charmer.Rank.UpdateLevelHero -= SaveDataHero;
            _charmer.Rank.ChangeCountCard -= SaveDataHero;
            _charmer.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataCharmer.CloseScreen();
            _soldierDataCharmer.enabled = false;
            _soldierDataCharmer.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataCharmer.SetCard(this);
            GetData();
            _soldierDataCharmer.gameObject.SetActive(true);
            _soldierDataCharmer.enabled = true;
            _soldierDataCharmer.SetSoldier(CameraView);
            _soldierDataCharmer.SetDataSoldier(CameraView, _progressService);

        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _charmer.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _charmer = (Charmer)Card.Soldier;
            if (_charmer.Rank.CurrentLevelHero >= 0)
                SetbaseComponent();
            else
                SetBaseComponentZoro();
        }

        public override void SetComponent()
        {
            _allMarker = 0;
            _soldierDataCharmer.Construct(_charmer, _progressService, CameraView);
            _soldierDataCharmer.SetSoldier(CameraView);
            _soldierDataCharmer.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataCharmer.AllMarkerSpecAttak + _soldierDataCharmer.AllUpRangMarker
              + _soldierDataCharmer.AllMarkerSurvability + _soldierDataCharmer.AllMarkerMeleeDamage;
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
            if (_charmer == null)
            {
                _charmer = (Charmer)Card.Soldier;
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
            _charmer.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _charmer.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _charmer.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _charmer.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _charmer.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_charmer.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _charmer.Rank.UpdateLevelHero += SaveDataHero;
            _charmer.Rank.ChangeCountCard += SaveDataHero;
            _charmer.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataCharmer.SetComponent(_charmer,_progressService, CameraView);
            
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenSpecialAttackLevel = _charmer.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenSurvivabilityLevel = _charmer.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenMeleeLevel = _charmer.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.CharmerHero.CurrenSpeedLevel = _charmer.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepSpecialAttack = _charmer.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepSurvivability = _charmer.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepMelee = _charmer.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.CharmerHero.CurrentStepMobility = _charmer.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.BestLevelSpecialSkill = _charmer.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.BestSurvivalRateLevel = _charmer.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.BestMeleerateLevel = _charmer.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.CharmerHero.BestLevelMobility = _charmer.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentLevelHero = _charmer.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.UnitOpened = _charmer.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.Hired = _charmer.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentCountCard = _charmer.Rank.CurrentCountCard;
        }
    }
}