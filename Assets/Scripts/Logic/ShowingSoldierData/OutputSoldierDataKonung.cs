using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Scripts.Logic.ShowingSoldierData
{
    public class OutputSoldierDataKonung : UpgradeData
    {
        public const string Proc = "%";
        public const string FirstSkillValueText = "Множитель";
        public const string SecondSkillValueText = "Длительность ";
        public const string TextSec = " сек.";
        private IPersistenProgressService _progressService;
        private Konung _konung;

        public void Construct(Konung konung, IPersistenProgressService persistenProgressService, CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _progressService = persistenProgressService;
            _konung = konung;
            _soldier = konung;
            _konung.BaseData();
            UploadSkillLevels();
            UploadSkillStep();
            _konung.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.KonungHero.UnitOpened);
            _konung.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.KonungHero.Hired);
            _konung.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentCountCard);
            _konung.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.KonungHero);
        }

        public void SetComponent(Konung konung,IPersistenProgressService persistenProgressService,CameraParent cameraParent)
        {
            Construct(konung, persistenProgressService, cameraParent);
            //CameraParent = cameraParent;
            //_progressService = persistenProgressService;
            //_konung = konung;
            //_soldier = konung;
            //_konung.BaseData();
            //UploadSkillLevels();
            //UploadSkillStep();
            //_konung.OpenHeroCard(_progressService.Progress.PlayerData.TypeHero.KonungHero.UnitOpened);
            //_konung.DataSoldier.LoadHired(_progressService.Progress.PlayerData.TypeHero.KonungHero.Hired);
            //_konung.Rank.SetLevelHero(_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentLevelHero, _progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentCountCard);
            //_konung.SetAbilityValues(_progressService.Progress.PlayerData.TypeHero.KonungHero);
            SetDataSoldier(CameraParent, _progressService);
        }

        public override void FillSpecialSkill(float newSpecialDamage, string sing = null)
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _konung.CurrenValueSpecAttack, newSpecialDamage, sing,Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _konung.CurrenDurationAction);
        }

        public override void FillSpecialSkill()
        {
            _screenUprageSoldier.SpecialSkill.SetFirstSkillValue(FirstSkillValueText, _konung.CurrenValueSpecAttack, Proc);
            _screenUprageSoldier.SpecialSkill.SetSecondSkillValue(SecondSkillValueText, _konung.CurrenDurationAction);
        }

        public override void SetSoldier(CameraParent cameraParent)
        {
            CameraParent = cameraParent;
            _soldier = _konung;
        }

        private void UploadSkillLevels()
        {
            _currentLevelSpecealSkill = (int)_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenSpecialAttackLevel;
            _currentSurvivabilityLevel = (int)_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenSurvivabilityLevel;
            _currentMelleDamageLevel = (int)_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrenMeleeLevel;
            //_currentSpeedLevel = (int)PlayerData.TypeHero.KonungHero.CurrenSpeedLevel;
        }

        private void UploadSkillStep()
        {
            _konung.SpecialSkillLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepSpecialAttack);
            _konung.SurvivabilityLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepSurvivability);
            _konung.MeleeDamageLevelData.LoadStepCurrent(_progressService.Progress.PlayerData.TypeHero.KonungHero.CurrentStepMelee);
            //_konung.SpeedSkillLevelData.LoadStepCurrent(PlayerData.TypeHero.KonungHero.CurrentStepMobility);
        }

        public override void SetTextSpecialSkill()
        {
            CurrentIdex = 1;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSpecialSkill(_konung, _konung.BaseMultiplier, AssetPath.BaseMultiplier,
                _konung.MaxBaseMultiplier, AssetPath.MaxMultiplier, _konung.DurationRecoverySpecAttack, AssetPath.TextSec, _konung.BaseDurationAction.ToString(),
                AssetPath.BaseDurationMultiplier, _konung.MaxDurationAction.ToString(), AssetPath.MaxDurationMultiplier);
        }

        public override void SetTextSurwability()
        {
            CurrentIdex = 2;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetSurwability(_screenUprageSoldier.Survivability.IconSkill.sprite, _konung);
        }

        public override void SetTextMeleeDamage()
        {
            CurrentIdex = 3;
            ScreenSkill.Container.SetActive(true);
            ScreenSkill.SetMeleeDamage(_screenUprageSoldier.MeleeDamage.IconSkill.sprite, _konung, TextSec);
        }
    }
}
