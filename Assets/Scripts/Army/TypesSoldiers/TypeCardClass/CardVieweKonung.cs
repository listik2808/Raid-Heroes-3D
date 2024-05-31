using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardVieweKonung : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataKonung _soldierDataKonung;
        private Konung _konung;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _konung.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _konung.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _konung.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _konung.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _konung.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_konung.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _konung.Rank.UpdateLevelHero -= SaveDataHero;
            _konung.Rank.ChangeCountCard -= SaveDataHero;
            _konung.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataKonung.CloseScreen();
            _soldierDataKonung.enabled = false;
            _soldierDataKonung.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataKonung.SetCard(this);
            GetData();
            _soldierDataKonung.gameObject.SetActive(true);
            _soldierDataKonung.enabled = true;
            _soldierDataKonung.SetSoldier(CameraView);
            _soldierDataKonung.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _konung.DataSoldier.SoldierСard.CameraParent;
            }

            SetComponentSevices();
            _konung = (Konung)Card.Soldier;
            if (_konung.Rank.CurrentLevelHero >= 0)
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
            _soldierDataKonung.Construct(_konung, _progressService, CameraView);
            _soldierDataKonung.SetSoldier(CameraView);
            _soldierDataKonung.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataKonung.AllMarkerSpecAttak + _soldierDataKonung.AllUpRangMarker
             + _soldierDataKonung.AllMarkerSurvability + _soldierDataKonung.AllMarkerMeleeDamage;
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
            if (_konung == null)
            {
                _konung = (Konung)Card.Soldier;
            }
            SaveBestLevelSkill();
            SaveCountCards();
            SaveCurrentLevelSkill();
            SaveCurrentStepSkill();
            SaveLevelHero();
            _saveLoadService.SaveProgress();
        }

        private void GetData()
        {
            _konung.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _konung.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _konung.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _konung.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _konung.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_konung.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _konung.Rank.UpdateLevelHero += SaveDataHero;
            _konung.Rank.ChangeCountCard += SaveDataHero;
            _konung.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataKonung.SetComponent(_konung,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenSpecialAttackLevel = _konung.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenSurvivabilityLevel = _konung.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenMeleeLevel = _konung.SoldiersStatsLevel.CurrentMeleelevel;
           // PlayerData.TypeHero.KonungHero.CurrenSpeedLevel = _konung.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepSpecialAttack = _konung.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepSurvivability = _konung.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepMelee = _konung.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.KonungHero.CurrentStepMobility = _konung.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KonungHero.BestLevelSpecialSkill = _konung.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.BestSurvivalRateLevel = _konung.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.BestMeleerateLevel = _konung.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.KonungHero.BestLevelMobility = _konung.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentLevelHero = _konung.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.UnitOpened = _konung.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.KonungHero.Hired = _konung.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentCountCard = _konung.Rank.CurrentCountCard;
        }

        public override void SetComponentSevices()
        {
            if (_progressService == null)
                _progressService = AllServices.Container.Single<IPersistenProgressService>();
            if (_saveLoadService == null)
                _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }
    }
}