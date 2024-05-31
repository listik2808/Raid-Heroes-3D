using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierViking : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Viking _viking;

        public void Construct(Viking viking, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _viking = viking;
            _soldier = viking;
            _viking.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _viking.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.VikingHero.UnitOpened);
            _viking.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.VikingHero.Hired);
            _viking.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentCountCard);
            _viking.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.VikingHero);
        }

        public void SetComponent(Viking viking,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(viking, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _viking.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _viking.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _viking.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _viking.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _viking;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.VikingHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _viking.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepSpecialAttack);
            _viking.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepSurvivability);
            _viking.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.VikingHero.CurrentStepMelee);
            //_viking.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.VikingHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_viking, _viking.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _viking.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _viking.DurationRecoverySpecAttack, AssetPath.TextSec, _viking.RestoringSpecialSkill.ToString(),
                AssetPath.DurationStunning);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _viking);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _viking, AssetPath.TextSec);
        }
    }
}
