using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataCharmer : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Charmer _charmer;

        public void Construct(Charmer charmer, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _charmer = charmer;
            _soldier = charmer;
            _charmer.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _charmer.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.CharmerHero.UnitOpened);
            _charmer.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.CharmerHero.Hired);
            _charmer.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentCountCard);
            _charmer.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.CharmerHero);
        }

        public void SetComponent(Charmer charmer,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(charmer, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextDuration, _charmer.CurrenValueSpecAttack, newSpecialDamage, sing,AssetPath.TextSec);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _charmer.BaseDurationAction);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextDuration, _charmer.CurrenValueSpecAttack, AssetPath.TextSec);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _charmer.BaseDurationAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _charmer;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.CharmerHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _charmer.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepSpecialAttack);
            _charmer.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepSurvivability);
            _charmer.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CharmerHero.CurrentStepMelee);
            //_charmer.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.CharmerHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            Debug.Log(_charmer.BaseDurationAction);
            Debug.Log(_charmer.MaxBaseDurationAction);
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_charmer, _charmer.BaseDurationAction, AssetPath.BaseDurationMultiplier,
                _charmer.MaxBaseDurationAction, AssetPath.MaxDurationMultiplier, _charmer.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _charmer);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _charmer, AssetPath.TextSec);
        }
    }
}
