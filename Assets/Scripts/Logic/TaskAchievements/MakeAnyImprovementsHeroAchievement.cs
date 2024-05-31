using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class MakeAnyImprovementsHeroAchievement : AchievementsAll
    {
        public const string WinRaid = "Сделать ";
        public const string WinRaid2 = " любых улучшений героев";
        private float AllValueUpHeroSkill = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 48, 49, 50, 51, 52, 53 };
                RevardCrystals = new List<float> { 1, 3, 5, 10, 100, 200, 500 };
                Requirements = new List<float> { 5, 10, 20, 100, 1000,5000, 10000};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetUpSkillHero;
            AllValueUpHeroSkill = ProgressService.Progress.Achievements.AllUpHeroSkill;
            MaxId();
        }

        public void SetUpSkill()
        {
            AllValueUpHeroSkill = ProgressService.Progress.Achievements.AllUpHeroSkill;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueUpHeroSkill, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetUpSkillHero = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
