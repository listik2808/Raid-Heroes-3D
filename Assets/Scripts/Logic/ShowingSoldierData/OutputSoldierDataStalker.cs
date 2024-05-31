using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataStalker : UpgradeData
    {
        public const string Proc = "%";
        public const string FirstSkillValueText = "Демотивация";
        public const string SecondSkillValueText = "Действует ";
        public const string TextSec = " сек.";
        private Stalker _stalker;
        private IPersistenProgressService _progressService;

        public void Construct(Stalker stalker, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _stalker = stalker;
            _soldier = stalker;
            _stalker.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _stalker.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.StalkerHero.UnitOpened);
            _stalker.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.StalkerHero.Hired);
            _stalker.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentCountCard);
            _stalker.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.StalkerHero);
        }

        public void SetComponent(Stalker stalker,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(stalker, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _stalker.CurrenValueSpecAttack, newSpecialDamage, sing,Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _stalker.CurrentDurationAction, _stalker.NewDurationAction,sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _stalker.CurrenValueSpecAttack,Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _stalker.DurationRecoverySpecAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _stalker;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.StalkerHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _stalker.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepSpecialAttack);
            _stalker.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepSurvivability);
            _stalker.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.StalkerHero.CurrentStepMelee);
            //_stalker.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.StalkerHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_stalker, _stalker.BaseMultiplier, AssetPath.BaseMultiplier,
                _stalker.MaxMultiplier, AssetPath.MaxMultiplier, _stalker.DurationRecoverySpecAttack, AssetPath.TextSec, _stalker.MinDurationAction.ToString(),
                AssetPath.BaseDurationMultiplier, _stalker.MaxDurationAction.ToString(), AssetPath.MaxDurationMultiplier);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _stalker);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _stalker, TextSec);
        }
    }
}
