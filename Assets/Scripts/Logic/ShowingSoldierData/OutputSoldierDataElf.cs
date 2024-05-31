using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataElf : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Elf _elf;

        public void Construct(Elf elf, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _elf = elf;
            _soldier = elf;
            _elf.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _elf.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.ElfHero.UnitOpened);
            _elf.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.ElfHero.Hired);
            _elf.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentCountCard);
            _elf.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.ElfHero);
        }

        public void SetComponent(Elf elf,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
           Construct(elf,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _elf.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _elf.CurrentRangeAttack, _elf.NewValueRange, sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _elf.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _elf.CurrentRangeAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _elf;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.ElfHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _elf.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepSpecialAttack);
            _elf.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepSurvivability);
            _elf.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ElfHero.CurrentStepMelee);
            //_elf.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.ElfHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_elf, _elf.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _elf.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _elf.DurationRecoverySpecAttack, AssetPath.TextSec, _elf.BaseRange.ToString(),
                AssetPath.TextBaseRange, _elf.MaxBaseRange.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _elf);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _elf, AssetPath.TextSec);
        }
    }
}
