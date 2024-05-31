using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataFairy : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Fairy _fairy;

        public void Construct(Fairy fairy, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _fairy = fairy;
            _soldier = fairy;
            _fairy.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _fairy.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.FairyHero.UnitOpened);
            _fairy.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.FairyHero.Hired);
            _fairy.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentCountCard);
            _fairy.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.FairyHero);
        }

        public void SetComponent(Fairy fairy,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(fairy,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextSurvivability, _fairy.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _fairy.DurationRecoverySpecAttack);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextSurvivability, _fairy.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _fairy.DurationRecoverySpecAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _fairy;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.FairyHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _fairy.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepSpecialAttack);
            _fairy.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepSurvivability);
            _fairy.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.FairyHero.CurrentStepMelee);
            //_fairy.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.FairyHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_fairy, _fairy.BaseValue, AssetPath.TextBaseHealing,
                _fairy.MaxBaseValue, AssetPath.TextMaxHealing, _fairy.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _fairy);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _fairy, AssetPath.TextSec);
        }
    }
}
