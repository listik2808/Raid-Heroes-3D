using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataAssasin : UpgradeData
    {
        private Assassin _assassin;
        private IPersistenProgressService _progressService;

        public void Construct(Assassin assassin, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _assassin = assassin;
            _soldier = assassin;
            _assassin.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _assassin.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.AssassinHero.UnitOpened);
            _assassin.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.AssassinHero.Hired);
            _assassin.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentCountCard);
            _assassin.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.AssassinHero);
        }

        public void SetComponent(Assassin assassin,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
           Construct(assassin, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _assassin.CurrenValueSpecAttack,newSpecialDamage,sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _assassin.DurationRecoverySpecAttack, AssetPath.TextSec);
        }
        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _assassin.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueText, _assassin.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _assassin;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.AssassinHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _assassin.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepSpecialAttack);
            _assassin.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepSurvivability);
            _assassin.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.AssassinHero.CurrentStepMelee);
            //_assasin.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.AssassinHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_assassin, _assassin.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _assassin.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamageOneAttack, _assassin.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _assassin);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _assassin, AssetPath.TextSec);
        }
    }
}
