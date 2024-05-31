using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerBerserk : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataBerserk _soldierDataBerserk;
        private Berserk _berserk;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _berserk.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _berserk.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _berserk.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _berserk.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _berserk.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_berserk.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _berserk.Rank.UpdateLevelHero -= SaveDataHero;
            _berserk.Rank.ChangeCountCard -= SaveDataHero;
            _berserk.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataBerserk.CloseScreen();
            _soldierDataBerserk.enabled = false;
            _soldierDataBerserk.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataBerserk.SetCard(this);
            GetData();
            _soldierDataBerserk.gameObject.SetActive(true);
            _soldierDataBerserk.enabled = true;
            _soldierDataBerserk.SetSoldier(CameraView);
            _soldierDataBerserk.SetDataSoldier(CameraView, _progressService);
        }

        public override void SaveDataHero()
        {
            SetComponentSevices();
            if (_berserk == null)
            {
                _berserk = (Berserk)Card.Soldier;
            }
            SaveBestLevelSkill();
            SaveCountCards();
            SaveCurrentLevelSkill();
            SaveCurrentStepSkill();
            SaveLevelHero();
            _saveLoadService.SaveProgress();
        }

        public override void Start()
        {

            if (CameraView == null)
            {
                CameraView = _berserk.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _berserk = (Berserk)Card.Soldier;
            if (_berserk.Rank.CurrentLevelHero >= 0)
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
            _soldierDataBerserk.Construct(_berserk, _progressService, CameraView);
            _soldierDataBerserk.SetSoldier(CameraView);
            _soldierDataBerserk.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataBerserk.AllMarkerSpecAttak + _soldierDataBerserk.AllUpRangMarker
              + _soldierDataBerserk.AllMarkerSurvability + _soldierDataBerserk.AllMarkerMeleeDamage;
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

        public override void SetComponentSevices()
        {
            if (_progressService == null)
                _progressService = AllServices.Container.Single<IPersistenProgressService>();
            if (_saveLoadService == null)
                _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void GetData()
        {
            _berserk.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _berserk.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _berserk.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _berserk.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _berserk.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_berserk.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _berserk.Rank.UpdateLevelHero += SaveDataHero;
            _berserk.Rank.ChangeCountCard += SaveDataHero;
            _berserk.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataBerserk.SetComponent(_berserk,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenSpecialAttackLevel = _berserk.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenSurvivabilityLevel = _berserk.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenMeleeLevel = _berserk.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.BerserkHero.CurrenSpeedLevel = _berserk.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepSpecialAttack = _berserk.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepSurvivability = _berserk.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepMelee = _berserk.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.BerserkHero.CurrentStepMobility = _berserk.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.BestLevelSpecialSkill = _berserk.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.BestSurvivalRateLevel = _berserk.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.BestMeleerateLevel = _berserk.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.BerserkHero.BestLevelMobility = _berserk.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentLevelHero = _berserk.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.UnitOpened = _berserk.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.Hired = _berserk.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentCountCard = _berserk.Rank.CurrentCountCard;
        }
    }
}