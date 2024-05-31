using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.Army.TypesSoldiers;
using Scripts.Data.TypeHeroSoldier;
using Scripts.StaticData;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class HireHeroes : AchievementsAll
    {
        public const string WinRaid = "Нанять ";
        public const string WinRaid2 = " героев";
        private List<HeroTypeId> _soldierHire = new List<HeroTypeId>();

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 9, 17, 18, 19};
                RevardCrystals = new List<float> { 10, 100,200,500 };
                Requirements = new List<float> { 1, 3, 10, 20};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementHireHeroCount;
            if(ProgressService.Progress.Achievements.HeroTypeIds.Count > 0 )
            {
                _soldierHire = ProgressService.Progress.Achievements.HeroTypeIds;
            }
            MaxId();
        }

        public void SetCountHireSoldiers(IPersistenProgressService progressService)
        {
            _soldierHire = progressService.Progress.Achievements.HeroTypeIds;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(_soldierHire.Count, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementHireHeroCount = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
