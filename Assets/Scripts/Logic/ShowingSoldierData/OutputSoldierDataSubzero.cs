using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataSubzero : UpgradeData
    {
        private Subzero _subzero;
        private IPersistenProgressService _progressService;

        public void Construct(Subzero subzero, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _subzero = subzero;
            _soldier = subzero;
            _subzero.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _subzero.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.UnitOpened);
            _subzero.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.Hired);
            _subzero.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentLevelHero,
                _progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentCountCard);
            _subzero.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.SubzeroHero);
        }

        public void SetComponent(Subzero subzero,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(subzero,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _subzero.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _subzero.FreezingTime, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _subzero.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _subzero.FreezingTime, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _subzero;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.SubzeroHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _subzero.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepSpecialAttack);
            _subzero.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepSurvivability);
            _subzero.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.SubzeroHero.CurrentStepMelee);
            //_subzero.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.SubzeroHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_subzero, _subzero.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _subzero.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _subzero.DurationRecoverySpecAttack, AssetPath.TextSec,
                _subzero.DurationRecoverySpecAttack.ToString(),
                AssetPath.TextFreezing);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _subzero);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _subzero, AssetPath.TextSec);
        }
    }
}
