using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerAssasin : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataAssasin _soldierDataAssasin;
        private Assassin _assasin;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        public override void CloseUpgradeSoldier()
        {
            _assasin.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _assasin.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _assasin.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _assasin.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _assasin.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_assasin.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _assasin.Rank.UpdateLevelHero -= SaveDataHero;
            _assasin.Rank.ChangeCountCard -= SaveDataHero;
            _assasin.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataAssasin.CloseScreen();
            _soldierDataAssasin.enabled = false;
            _soldierDataAssasin.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataAssasin.SetCard(this);
            GetData();
            _soldierDataAssasin.gameObject.SetActive(true);
            _soldierDataAssasin.enabled = true;
            _soldierDataAssasin.SetSoldier(CameraView);
            _soldierDataAssasin.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if(CameraView == null)
            {
                 CameraView = _assasin.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _assasin = (Assassin)Card.Soldier;
            if (_assasin.Rank.CurrentLevelHero >= 0)
                SetbaseComponent();
            else
                SetBaseComponentZoro();
        }

        public override void SetComponent()
        {
            _soldierDataAssasin.Construct(_assasin, _progressService, CameraView);
            _soldierDataAssasin.SetSoldier(CameraView);
            _soldierDataAssasin.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataAssasin.AllMarkerSpecAttak + _soldierDataAssasin.AllUpRangMarker
                + _soldierDataAssasin.AllMarkerSurvability + _soldierDataAssasin.AllMarkerMeleeDamage;
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
            if (_assasin == null)
            {
                _assasin = (Assassin)Card.Soldier;
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
            _assasin.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _assasin.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _assasin.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _assasin.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _assasin.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_assasin.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _assasin.Rank.UpdateLevelHero += SaveDataHero;
            _assasin.Rank.ChangeCountCard += SaveDataHero;
            _assasin.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataAssasin.SetComponent(_assasin,_progressService,CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenSpecialAttackLevel = _assasin.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenSurvivabilityLevel = _assasin.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenMeleeLevel = _assasin.SoldiersStatsLevel.CurrentMeleelevel;
           // PlayerData.TypeHero.AssassinHero.CurrenSpeedLevel = _assasin.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepSpecialAttack = _assasin.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepSurvivability = _assasin.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepMelee = _assasin.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.AssassinHero.CurrentStepMobility = _assasin.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.BestLevelSpecialSkill = _assasin.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.BestSurvivalRateLevel = _assasin.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.BestMeleerateLevel = _assasin.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.AssassinHero.BestLevelMobility = _assasin.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentLevelHero = _assasin.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.UnitOpened = _assasin.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.Hired = _assasin.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentCountCard = _assasin.Rank.CurrentCountCard;
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