using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataWitcherRed : UpgradeData
    {
        private WitcherRed _witcherRed;
        private IPersistenProgressService _progressService;

        public void Construct(WitcherRed witcherRed, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _witcherRed = witcherRed;
            _soldier = witcherRed;
            _witcherRed.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _witcherRed.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.UnitOpened);
            _witcherRed.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.Hired);
            _witcherRed.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentCountCard);
            _witcherRed.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero);
        }

        public void SetComponent(WitcherRed witcherRed,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(witcherRed,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _witcherRed.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _witcherRed.RadiusAction, _witcherRed.NewRadius, sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _witcherRed.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _witcherRed.RadiusAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _witcherRed;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.WitcherRedHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _witcherRed.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepSpecialAttack);
            _witcherRed.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepSurvivability);
            _witcherRed.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherRedHero.CurrentStepMelee);
            //_witcherRed.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.WitcherRedHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_witcherRed, _witcherRed.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _witcherRed.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _witcherRed.DurationRecoverySpecAttack, AssetPath.TextSec, _witcherRed.BaseRadius.ToString(),
                AssetPath.TextBaseRange, _witcherRed.MaxRadius.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _witcherRed);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _witcherRed, AssetPath.TextSec);
        }
    }
}
