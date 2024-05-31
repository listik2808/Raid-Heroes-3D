using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataBerserk : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Berserk _berserk;

        public void Construct(Berserk berserk, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _berserk = berserk;
            _soldier = berserk;
            _berserk.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _berserk.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.BerserkHero.UnitOpened);
            _berserk.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.BerserkHero.Hired);
            _berserk.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentCountCard);
            _berserk.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.BerserkHero);
        }

        public void SetComponent(Berserk berserk,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(berserk, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.DamageEndBlokText, _berserk.CurrenValueSpecAttack, newSpecialDamage, sing, AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuratinKing, _berserk.CurrentDurationAction, _berserk.NewDuarationAction, sing);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.DamageEndBlokText, _berserk.CurrenValueSpecAttack,AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.SecondSkillValueTextRange, _berserk.CurrentDurationAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _berserk;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.BerserkHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _berserk.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepSpecialAttack);
            _berserk.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepSurvivability);
            _berserk.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.BerserkHero.CurrentStepMelee);
            //_berserk.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.BerserkHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_berserk, _berserk.BaseMultiplier, AssetPath.BaseMultiplier,
                _berserk.MaxBaseMultiplier, AssetPath.MaxMultiplier, _berserk.DurationRecoverySpecAttack, AssetPath.TextSec, _berserk.MinDurationAction.ToString(),
                AssetPath.BaseDurationMultiplier,_berserk.MaxDurationAction.ToString(),AssetPath.MaxDurationMultiplier);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _berserk);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _berserk, AssetPath.TextSec);
        }
    }
}
