using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataPoisoner : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Poisoner _poisoner;

        public void Construct(Poisoner poisoner, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _poisoner = poisoner;
            _soldier = poisoner;
            _poisoner.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _poisoner.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.UnitOpened);
            _poisoner.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.Hired);
            _poisoner.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentCountCard);
            _poisoner.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.PoisonerHero);
        }

        public void SetComponent(Poisoner poisoner,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(poisoner, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _poisoner.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _poisoner.RadiusAction, _poisoner.NewRadius, sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _poisoner.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _poisoner.RadiusAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _poisoner;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.PoisonerHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _poisoner.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepSpecialAttack);
            _poisoner.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepSurvivability);
            _poisoner.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PoisonerHero.CurrentStepMelee);
            //_poisoner.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.PoisonerHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            _poisoner.SetMaxRange();
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_poisoner, _poisoner.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _poisoner.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _poisoner.DurationRecoverySpecAttack, AssetPath.TextSec, _poisoner.RadiusAction.ToString(),
                AssetPath.TextBaseRange, _poisoner.MaxRadius.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _poisoner);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _poisoner, AssetPath.TextSec);
        }
    }
}
