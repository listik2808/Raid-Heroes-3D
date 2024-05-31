using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataPrincess : UpgradeData
    {
        public const string FirstSkillValueText = "Перенос здоровья ";
        public const string SecondSkillValueText = "Восстановление ";
        public const string TextSec = " сек.";
        private Princess _princess;
        private IPersistenProgressService _progressService;

        public void Construct(Princess princess, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _princess = princess;
            _soldier = princess;
            _princess.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _princess.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.PrincessHero.UnitOpened);
            _princess.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.PrincessHero.Hired);
            _princess.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentCountCard);
            _princess.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.PrincessHero);
        }

        public void SetComponent(Princess princess,IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            Construct(princess, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _princess.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _princess.DurationRecoverySpecAttack, TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _princess.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _princess.DurationRecoverySpecAttack, TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _princess;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.PrincessHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _princess.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepSpecialAttack);
            _princess.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepSurvivability);
            _princess.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.PrincessHero.CurrentStepMelee);
            //_princess.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.PrincessHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_princess, _princess.BaseSpecialAttack, AssetPath.TextBaseDamage,
                _princess.MaxBaseSpecialAttack, AssetPath.TextMaxDamage, _princess.DurationRecoverySpecAttack, AssetPath.TextSec);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _princess);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _princess, TextSec);
        }
    }
}
