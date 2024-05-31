using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataWarrior : UpgradeData
    {
        private Warrior _warrior;
        private IPersistenProgressService _progressService;

        public void Construct(Warrior warrior, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _warrior = warrior;
            _soldier = warrior;
            _warrior.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _warrior.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.WarriorHero.UnitOpened);
            _warrior.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.WarriorHero.Hired);
            _warrior.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentCountCard);
            _warrior.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.WarriorHero);
        }

        public void SetComponent(Warrior warrior,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(warrior, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _warrior.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.StunningText, _warrior.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _warrior.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.StunningText, _warrior.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _warrior;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.WarriorHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _warrior.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepSpecialAttack);
            _warrior.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepSurvivability);
            _warrior.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WarriorHero.CurrentStepMelee);
            //_warrior.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.WarriorHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_warrior, _warrior.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _warrior.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _warrior.DurationRecoverySpecAttack, AssetPath.TextSec, _warrior.RestoringSpecialSkill.ToString(),
                AssetPath.DurationStunning);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _warrior);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _warrior, AssetPath.TextSec);
        }
    }
}
