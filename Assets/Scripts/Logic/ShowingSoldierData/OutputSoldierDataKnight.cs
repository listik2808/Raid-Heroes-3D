using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataKnight : UpgradeData
    {
        private IPersistenProgressService _progressService;
        private Knight _knight;
        public void Construct(Knight knight, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _knight = knight;
            _soldier = knight;
            _knight.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _knight.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.KnightHero.UnitOpened);
            _knight.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.KnightHero.Hired);
            _knight.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentCountCard);
            _knight.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.KnightHero);
        }

        public void SetComponent(Knight knight,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(knight, persistenProgressService, cameraParent);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _knight.CurrenValueSpecAttack, newSpecialDamage, sing);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _knight.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(AssetPath.OneSkillValueTextDamage, _knight.CurrenValueSpecAttack);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(AssetPath.TextDuration, _knight.RestoringSpecialSkill, AssetPath.TextSec);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _knight;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrenMeleeLevel;
           // _currentSpeedLevel = (int)PlayerData.TypeHero.KnightHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _knight.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepSpecialAttack);
            _knight.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepSurvivability);
            _knight.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KnightHero.CurrentStepMelee);
            //_knight.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.KnightHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_knight, _knight.BaseDamageSpecialAttack, AssetPath.TextBaseDamage,
                _knight.MaxBaseDamageSpecialAttack, AssetPath.TextMaxDamage, _knight.DurationRecoverySpecAttack, AssetPath.TextSec, _knight.RestoringSpecialSkill.ToString(),
                AssetPath.DurationStunning);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _knight);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _knight, AssetPath.TextSec);
        }
    }
}
