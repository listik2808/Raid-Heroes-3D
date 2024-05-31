using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataPriest : UpgradeData
    {
        public const string FirstSkillValueText = "Урон скулета";
        public const string SecondSkillValueText = "Здоровье скелета ";
        public const string TextSec = " сек.";
        private IPersistenProgressService _progressService;
        private Priest _priest;

        public void Construct(Priest priest, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _priest = priest;
            _soldier = priest;
            _priest.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _priest.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.PriestHero.UnitOpened);
            _priest.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.PriestHero.Hired);
            _priest.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentCountCard);
            _priest.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.PriestHero);
        }

        public void SetComponent(Priest priest,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(priest, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _priest.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _priest.CurrentHealthSkeleta, _priest.NewHealthSkeleta,sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _priest.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _priest.CurrentHealthSkeleta);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _priest;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.PriestHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _priest.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepSpecialAttack);
            _priest.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepSurvivability);
            _priest.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PriestHero.CurrentStepMelee);
            //_priest.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.PriestHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_priest, _priest.BaseValueDamageSkeleta, AssetPath.TextBaseDamage,
                _priest.MaxDamageSkelet, AssetPath.TextMaxDamage, _priest.DurationRecoverySpecAttack, AssetPath.TextSec,_priest.BaseHealthSkelet.ToString(),AssetPath.TextHealthSkelet,
                _priest.MaxHpSkelet.ToString(),AssetPath.TextMaxHealthSkelet);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _priest);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _priest, TextSec);
        }
    }
}
