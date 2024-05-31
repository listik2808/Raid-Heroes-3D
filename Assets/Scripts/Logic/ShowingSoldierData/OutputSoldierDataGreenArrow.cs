using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataGreenArrow : UpgradeData
    {
        public const string FirstSkillValueText = "Урон ";
        public const string SecondSkillValueText = "Восстановление ";
        public const string TextSec = " сек.";
        private IPersistenProgressService _progressService;
        private GreenArrow _greenArrow;

        public void Construct(GreenArrow greenArrow, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _greenArrow = greenArrow;
            _soldier = greenArrow;
            _greenArrow.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _greenArrow.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.UnitOpened);
            _greenArrow.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.Hired);
            _greenArrow.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentCountCard);
            _greenArrow.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero);
        }

        public void SetComponent(GreenArrow greenArrow,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(greenArrow, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _greenArrow.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _greenArrow.DurationRecoverySpecAttack);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _greenArrow.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _greenArrow.DurationRecoverySpecAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _greenArrow;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.GreenArrowHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _greenArrow.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepSpecialAttack);
            _greenArrow.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepSurvivability);
            _greenArrow.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.GreenArrowHero.CurrentStepMelee);
            //_greenArrow.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.GreenArrowHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_greenArrow, _greenArrow.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _greenArrow.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _greenArrow.DurationRecoverySpecAttack, AssetPath.TextSec, _greenArrow.BaseRange.ToString(),
                AssetPath.TextBaseRange, _greenArrow.MaxBaseRange.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _greenArrow);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _greenArrow, TextSec);
        }
    }
}
