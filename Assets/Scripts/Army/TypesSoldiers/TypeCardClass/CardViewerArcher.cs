using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerArcher : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataArcher _soldierDataArcher;
        private Archer _archer;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _archer.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _archer.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _archer.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _archer.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _archer.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_archer.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _archer.Rank.UpdateLevelHero -= SaveDataHero;
            _archer.Rank.ChangeCountCard -= SaveDataHero;
            _archer.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataArcher.CloseScreen();
            _soldierDataArcher.enabled = false;
            _soldierDataArcher.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataArcher.SetCard(this);
            GetData();
            _soldierDataArcher.gameObject.SetActive(true);
            _soldierDataArcher.enabled = true;
            _soldierDataArcher.SetSoldier(CameraView);
            _soldierDataArcher.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _archer.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _archer = (Archer)Card.Soldier;
            if (_archer.Rank.CurrentLevelHero >= 0)
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
            _soldierDataArcher.Construct(_archer, _progressService, CameraView);
            _soldierDataArcher.SetSoldier(CameraView);
            _soldierDataArcher.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataArcher.AllMarkerSpecAttak + _soldierDataArcher.AllUpRangMarker
              + _soldierDataArcher.AllMarkerSurvability + _soldierDataArcher.AllMarkerMeleeDamage;
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
            if (_archer == null)
            {
                _archer = (Archer)Card.Soldier;
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
            _archer.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _archer.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _archer.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _archer.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _archer.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_archer.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _archer.Rank.UpdateLevelHero += SaveDataHero;
            _archer.Rank.ChangeCountCard += SaveDataHero;
            _archer.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataArcher.SetComponent(_archer,_progressService,CameraView);
            
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenSpecialAttackLevel = _archer.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenSurvivabilityLevel = _archer.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenMeleeLevel = _archer.SoldiersStatsLevel.CurrentMeleelevel;
           // PlayerData.TypeHero.ArcherHero.CurrenSpeedLevel = _archer.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepSpecialAttack = _archer.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepSurvivability = _archer.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepMelee = _archer.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.ArcherHero.CurrentStepMobility = _archer.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.BestLevelSpecialSkill = _archer.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.BestSurvivalRateLevel = _archer.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.BestMeleerateLevel = _archer.SoldiersStatsLevel.BestMeleerateLevel;
           // PlayerData.TypeHero.ArcherHero.BestLevelMobility = _archer.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentLevelHero = _archer.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.UnitOpened = _archer.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.Hired = _archer.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentCountCard = _archer.Rank.CurrentCountCard;
        }
    }
}