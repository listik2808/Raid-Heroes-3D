using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataKing : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private King _king;

        public void Construct(King king, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _king = king;
            _soldier = king;
            _king.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _king.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.KingHero.UnitOpened);
            _king.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.KingHero.Hired);
            _king.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.KingHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.KingHero.CurrentCountCard);
            _king.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.KingHero);
        }

        public void SetComponent(King king,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(king, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.DamageEndBlokText, _king.CurrenValueSpecAttack, newSpecialDamage, sing,AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuratinKing, _king.CurrentDuationAction, _king.NewDurationAction, sing, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {

            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.DamageEndBlokText, _king.CurrenValueSpecAttack,AssetPath.Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuratinKing, _king.CurrentDuationAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _king;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.KingHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.KingHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.KingHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.KingHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _king.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepSpecialAttack);
            _king.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepSurvivability);
            _king.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KingHero.CurrentStepMelee);
            //_king.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.KingHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_king, _king.BaseMultiplier, AssetPath.BaseMultiplier,
                _king.MaxBaseMultiplier, AssetPath.MaxMultiplier, _king.DurationRecoverySpecAttack, AssetPath.TextSec, _king.MinDurationAction.ToString(),
                AssetPath.BaseDurationMultiplier, _king.MaxDurationAction.ToString(), AssetPath.MaxDurationMultiplier);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _king);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _king, AssetPath.TextSec);
        }
    }
}
