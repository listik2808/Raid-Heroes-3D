using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataWitcherGreen : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private WitcherGreen _witcherGreen;

        public void Construct(WitcherGreen witcherGreen, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _witcherGreen = witcherGreen;
            _soldier = witcherGreen;
            _witcherGreen.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _witcherGreen.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.UnitOpened);
            _witcherGreen.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.Hired);
            _witcherGreen.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentCountCard);
            _witcherGreen.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero);
        }

        public void SetComponent(WitcherGreen witcherGreen,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(witcherGreen,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextSurvivability, _witcherGreen.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _witcherGreen.DurationRecoverySpecAttack);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextSurvivability, _witcherGreen.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _witcherGreen.DurationRecoverySpecAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _witcherGreen;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.WitcherGreenHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _witcherGreen.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepSpecialAttack);
            _witcherGreen.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepSurvivability);
            _witcherGreen.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherGreenHero.CurrentStepMelee);
            //_witcherGreen.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.WitcherGreenHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_witcherGreen, _witcherGreen.BaseValue, AssetPath.TextBaseHealing,
                _witcherGreen.MaxBaseValue, AssetPath.TextMaxHealing, _witcherGreen.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _witcherGreen);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _witcherGreen, AssetPath.TextSec);
        }
    }
}
