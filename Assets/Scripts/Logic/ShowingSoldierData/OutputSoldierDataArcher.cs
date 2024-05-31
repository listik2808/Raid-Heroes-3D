using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataArcher : UpgradeData
    {
        private Archer _archer;
        private IPersistenProgressService _progressService;

        public void Construct(Archer archer, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _archer = archer;
            _soldier = archer;
            _archer.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _archer.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.ArcherHero.UnitOpened);
            _archer.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.ArcherHero.Hired);
            _archer.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentCountCard);
            _archer.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.ArcherHero);
        }

        public void SetComponent(Archer archer,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(archer,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _archer.CurrenValueSpecAttack,newSpecialDamage,sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _archer.CurrentRangeAttack,_archer.NewValueRange,sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.FirstSkillValueText, _archer.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _archer.CurrentRangeAttack);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _archer;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.ArcherHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _archer.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepSpecialAttack);
            _archer.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepSurvivability);
            _archer.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.ArcherHero.CurrentStepMelee);
            //_archer.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.ArcherHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_archer, _archer.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _archer.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _archer.DurationRecoverySpecAttack, AssetPath.TextSec,_archer.BaseRange.ToString(),
                AssetPath.TextBaseRange,_archer.MaxBaseRange.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite,_archer);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite,_archer, AssetPath.TextSec);
        }
    }
}
