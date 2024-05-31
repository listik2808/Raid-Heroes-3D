using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Logic.ShowingSoldierData;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public class CardViewerKnight : SoldierCardViewer
    {
        [SerializeField] private OutputSoldierDataKnight _soldierDataKnight;
        private Knight _knight;
        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public override void CloseUpgradeSoldier()
        {
            _knight.SoldiersStatsLevel.UpdateBestLevelSkill -= SaveDataHero;
            _knight.SoldiersStatsLevel.UpdateCurrentLevelSkill -= SaveDataHero;
            _knight.SpecialSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _knight.SurvivabilityLevelData.UpdateStepSkill -= SaveDataHero;
            _knight.MeleeDamageLevelData.UpdateStepSkill -= SaveDataHero;
            //_knight.SpeedSkillLevelData.UpdateStepSkill -= SaveDataHero;
            _knight.Rank.UpdateLevelHero -= SaveDataHero;
            _knight.Rank.ChangeCountCard -= SaveDataHero;
            _knight.Rank.BackgroundChanged -= SetbaseComponent;
            _soldierDataKnight.CloseScreen();
            _soldierDataKnight.enabled = false;
            _soldierDataKnight.gameObject.SetActive(false);
        }

        public override void OpenSoldierUpgradeScreen()
        {
            _soldierDataKnight.SetCard(this);
            GetData();
            _soldierDataKnight.gameObject.SetActive(true);
            _soldierDataKnight.enabled = true;
            _soldierDataKnight.SetSoldier(CameraView);
            _soldierDataKnight.SetDataSoldier(CameraView, _progressService);
        }

        public override void Start()
        {
            if (CameraView == null)
            {
                CameraView = _knight.DataSoldier.SoldierСard.CameraParent;
            }
            SetComponentSevices();
            _knight = (Knight)Card.Soldier;

            if (_knight.Rank.CurrentLevelHero >= 0)
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
            _soldierDataKnight.Construct(_knight, _progressService, CameraView);
            _soldierDataKnight.SetSoldier(CameraView);
            _soldierDataKnight.SetValuesCharacteristics(CameraView, _progressService);
            _allMarker = _soldierDataKnight.AllMarkerSpecAttak + _soldierDataKnight.AllUpRangMarker
                + _soldierDataKnight.AllMarkerSurvability + _soldierDataKnight.AllMarkerMeleeDamage;
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
            if (_knight == null)
            {
                _knight = (Knight)Card.Soldier;
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
            _knight.SoldiersStatsLevel.UpdateBestLevelSkill += SaveDataHero;
            _knight.SoldiersStatsLevel.UpdateCurrentLevelSkill += SaveDataHero;
            _knight.SpecialSkillLevelData.UpdateStepSkill += SaveDataHero;
            _knight.SurvivabilityLevelData.UpdateStepSkill += SaveDataHero;
            _knight.MeleeDamageLevelData.UpdateStepSkill += SaveDataHero;
            //_knight.SpeedSkillLevelData.UpdateStepSkill += SaveDataHero;
            _knight.Rank.UpdateLevelHero += SaveDataHero;
            _knight.Rank.ChangeCountCard += SaveDataHero;
            _knight.Rank.BackgroundChanged += SetbaseComponent;
            _soldierDataKnight.SetComponent(_knight,_progressService, CameraView);
        }

        private void SaveCurrentLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenSpecialAttackLevel = _knight.SoldiersStatsLevel.CurrentLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenSurvivabilityLevel = _knight.SoldiersStatsLevel.CurrentSurvivabilityLevel;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenMeleeLevel = _knight.SoldiersStatsLevel.CurrentMeleelevel;
            //PlayerData.TypeHero.KnightHero.CurrenSpeedLevel = _knight.SoldiersStatsLevel.CurrentSpeedLevel;
        }

        private void SaveCurrentStepSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepSpecialAttack = _knight.SpecialSkillLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepSurvivability = _knight.SurvivabilityLevelData.CurrentStepSkill;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepMelee = _knight.MeleeDamageLevelData.CurrentStepSkill;
            //PlayerData.TypeHero.KnightHero.CurrentStepMobility = _knight.SpeedSkillLevelData.CurrentStepSkill;
        }

        private void SaveBestLevelSkill()
        {
            _progressService.Progress.PlayerData.TypeHero.KnightHero.BestLevelSpecialSkill = _knight.SoldiersStatsLevel.BestLevelSpecialSkill;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.BestSurvivalRateLevel = _knight.SoldiersStatsLevel.BestSurvivalRateLevel;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.BestMeleerateLevel = _knight.SoldiersStatsLevel.BestMeleerateLevel;
            //PlayerData.TypeHero.KnightHero.BestLevelMobility = _knight.SoldiersStatsLevel.BestLevelMobility;
        }

        private void SaveLevelHero()
        {
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentLevelHero = _knight.Rank.CurrentLevelHero;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.UnitOpened = _knight.UnitOpened;
            _progressService.Progress.PlayerData.TypeHero.KnightHero.Hired = _knight.DataSoldier.Hired;
        }

        private void SaveCountCards()
        {
            _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentCountCard = _knight.Rank.CurrentCountCard;
        }
    }
}