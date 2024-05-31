using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataCommander : UpgradeData
    {
        IPersistenProgressService _progressService;
        private Commander _commander;

        public void Construct(Commander commander, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _commander = commander;
            _soldier = commander;
            _commander.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _commander.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.CommanderHero.UnitOpened);
            _commander.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.CommanderHero.Hired);
            _commander.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentCountCard);
            _commander.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.CommanderHero);
        }

        public void SetComponent(Commander commander,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(commander, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextMultiplier, _commander.CurrenValueSpecAttack, newSpecialDamage, sing, AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _commander.CurrentDurationAction,_commander.NewDuarationAction,sing,AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.TextMultiplier, _commander.CurrenValueSpecAttack, AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _commander.CurrentDurationAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _commander;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.CommanderHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _commander.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepSpecialAttack);
            _commander.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepSurvivability);
            _commander.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.CommanderHero.CurrentStepMelee);
            //_commander.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.CommanderHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_commander, _commander.BaseMultiplier, AssetPath.BaseMultiplier,
                _commander.MaxMultiplier, AssetPath.MaxMultiplier, _commander.DurationRecoverySpecAttack, AssetPath.TextSec, _commander.MinDurationAction.ToString(),
                AssetPath.BaseDurationMultiplier, _commander.MaxDurationAction.ToString(), AssetPath.MaxDurationMultiplier);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _commander);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _commander, AssetPath.TextSec);
        }
    }
}
