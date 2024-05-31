using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerKing : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataKing _soldierDataKing;
        private King _king;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _king.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _king.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _king.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _king.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _king.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_king.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _king.Rank.UpdateLevelHero -= SaveDataHero;
            _king.Rank.ChangeCountCard -= SaveDataHero;
            _king.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataKing.CloseScreen();
            _soldierDataKing.enabled = false;
            _soldierDataKing.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataKing.SetCard(this);
            GetData();
            _soldierDataKing.gameObject.SetActive(true);
            _soldierDataKing.enabled = true;
            _soldierDataKing.SetSoldier(CameraView);
            _soldierDataKing.SetDataSoldier(CameraView, _progressService);
        }
        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _king.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _king = (King)Card.Soldier;
            if (_king.Rank.CurrentLevelHero >= 0)
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
            _soldierDataKing.Construct(_king, _progressService, CameraView);
            _soldierDataKing.SetSoldier(CameraView);
            _soldierDataKing.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataKing.AllMarkerSpecAttak + _soldierDataKing.AllUpRangMarker
               + _soldierDataKing.AllMarkerSurvability + _soldierDataKing.AllMarkerMeleeDamage;
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
            if (_king == null)
            {
                _king = (King)Card.Soldier;
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
            _king.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _king.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _king.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _king.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _king.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_king.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _king.Rank.UpdateLevelHero += SaveDataHero;
            _king.Rank.ChangeCountCard += SaveDataHero;
            _king.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataKing.SetComponent(_king,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrenSpecialAttackLevel = _king.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrenSurvivabilityLevel = _king.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrenMeleeLevel = _king.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.KingHero.CurrenSpeedLevel = _king.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepSpecialAttack = _king.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepSurvivability = _king.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepMelee = _king.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.KingHero.CurrentStepMobility = _king.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KingHero.BestLevelSpecialSkill = _king.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KingHero.BestSurvivalRateLevel = _king.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.KingHero.BestMeleerateLevel = _king.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.KingHero.BestLevelMobility = _king.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentLevelHero = _king.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.KingHero.UnitOpened = _king.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.KingHero.Hired = _king.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentCountCard = _king.Rank.CurrentCountCard;
        }
    }
}