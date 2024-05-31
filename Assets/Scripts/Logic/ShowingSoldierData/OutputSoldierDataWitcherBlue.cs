using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataWitcherBlue : UpgradeData
    {
        private WitcherBlue _witcherBlue;
        private IPersistenProgressService _progressService;

        public void Construct(WitcherBlue witcherBlue, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _witcherBlue = witcherBlue;
            _soldier = witcherBlue;
            _witcherBlue.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _witcherBlue.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.UnitOpened);
            _witcherBlue.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.Hired);
            _witcherBlue.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentCountCard);
            _witcherBlue.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero);
        }

        public void SetComponent(WitcherBlue witcherBlue,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(witcherBlue,persistenProgressService,cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _witcherBlue.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _witcherBlue.RadiusAction, _witcherBlue.NewRadius, sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _witcherBlue.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _witcherBlue.RadiusAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _witcherBlue;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.WitcherBlueHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _witcherBlue.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepSpecialAttack);
            _witcherBlue.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepSurvivability);
            _witcherBlue.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.WitcherBlueHero.CurrentStepMelee);
            //_witcherBlue.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.WitcherBlueHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_witcherBlue, _witcherBlue.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _witcherBlue.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _witcherBlue.DurationRecoverySpecAttack, AssetPath.TextSec, _witcherBlue.BaseRadius.ToString(),
                AssetPath.TextBaseRange, _witcherBlue.MaxRadius.ToString(), AssetPath.TextMaxRang);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _witcherBlue);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _witcherBlue, AssetPath.TextSec);
        }
    }
}
