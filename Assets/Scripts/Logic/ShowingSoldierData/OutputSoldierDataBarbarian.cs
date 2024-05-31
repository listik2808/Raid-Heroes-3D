using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataBarbarian : UpgradeData
    {
        private IPersistenProgressService _persistenProgressService;
        private Barbarian _barbarian;

        public void Construct(Barbarian barbarian, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _persistenProgressService = persistenProgressService;
            _barbarian = barbarian;
            _soldier = barbarian;
            _barbarian.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _barbarian.OpenHeroCard(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.UnitOpened);
            _barbarian.DataSoldier.LoadHired(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.Hired);
            _barbarian.Rank.SetLevelHero(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentLevelHero,
                _persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentCountCard);
            _barbarian.SetAbilityValues(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing =null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _barbarian.CurrenValueSpecAttack, newSpecialDamage,sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.StunningText, _barbarian.DurationSpecialSkill, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _barbarian.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.StunningText, _barbarian.DurationSpecialSkill, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _barbarian;
        }

        public void SetComponent(Barbarian barbarian,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(barbarian, _persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent,_persistenProgressService);
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.BarbarianHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _barbarian.SpecialSkillLevelData.LoadStepCurrent(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepSpecialAttack);
            _barbarian.SurvivabilityLevelData.LoadStepCurrent(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepSurvivability);
            _barbarian.MeleeDamageLevelData.LoadStepCurrent(_persistenProgressService.Progress.PlayerData.TypeHero.BarbarianHero.CurrentStepMelee);
            //_barbarian.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.BarbarianHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_barbarian, _barbarian.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _barbarian.MaxBaseDamageSpecialAttack,AssetPath.TextMaxDamage,_barbarian.DurationRecoverySpecAttack,AssetPath.TextSec,_barbarian.DurationSpecialSkill.ToString(),
                AssetPath.DurationStunning);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _barbarian);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _barbarian, AssetPath.TextSec);
        }
    }
}
