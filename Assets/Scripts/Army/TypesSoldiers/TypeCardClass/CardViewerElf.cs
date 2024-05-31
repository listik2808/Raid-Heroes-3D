using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerElf : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataElf _soldierDataElf;
        private Elf _elf;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _elf.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _elf.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _elf.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _elf.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _elf.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_elf.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _elf.Rank.UpdateLevelHero -= SaveDataHero;
            _elf.Rank.ChangeCountCard -= SaveDataHero;
            _elf.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataElf.CloseScreen();
            _soldierDataElf.enabled = false;
            _soldierDataElf.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataElf.SetCard(this);
            GetData();
            _soldierDataElf.gameObject.SetActive(true);
            _soldierDataElf.enabled = true;
            _soldierDataElf.SetSoldier(CameraView);
            _soldierDataElf.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _elf.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _elf = (Elf)Card.Soldier;
            if (_elf.Rank.CurrentLevelHero >= 0)
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
            _soldierDataElf.Construct(_elf, _progressService, CameraView);
            _soldierDataElf.SetSoldier(CameraView);
            _soldierDataElf.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataElf.AllMarkerSpecAttak + _soldierDataElf.AllUpRangMarker
              + _soldierDataElf.AllMarkerSurvability + _soldierDataElf.AllMarkerMeleeDamage;
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
            if (_elf == null)
            {
                _elf = (Elf)Card.Soldier;
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
            _elf.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _elf.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _elf.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _elf.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _elf.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_elf.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _elf.Rank.UpdateLevelHero += SaveDataHero;
            _elf.Rank.ChangeCountCard += SaveDataHero;
            _elf.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataElf.SetComponent(_elf,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenSpecialAttackLevel = _elf.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenSurvivabilityLevel = _elf.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenMeleeLevel = _elf.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.ElfHero.CurrenSpeedLevel = _elf.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepSpecialAttack = _elf.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepSurvivability = _elf.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepMelee = _elf.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.ElfHero.CurrentStepMobility = _elf.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.ElfHero.BestLevelSpecialSkill = _elf.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.BestSurvivalRateLevel = _elf.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.BestMeleerateLevel = _elf.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.ElfHero.BestLevelMobility = _elf.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentLevelHero = _elf.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.UnitOpened = _elf.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.ElfHero.Hired = _elf.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentCountCard = _elf.Rank.CurrentCountCard;
        }
    }
}