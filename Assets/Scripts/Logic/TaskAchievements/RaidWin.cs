using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Logic.TaskAchievements
{
    public class RaidWin : AchievementsAll
    {
        public const string WinRaid = "Одержать ";
        public const string WinRaid2 = " побед в рейде";

        public float AllCountFight = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                RevardCrystals = new List<float> { 1, 2, 5, 10, 50, 100, 500 };
                Requirements = new List<float> { 10, 20, 40, 80, 200, 1000, 10000 };
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementRaidWinIndexId;
            AllCountFight = ProgressService.Progress.Achievements.AllCountFightNumber;
            MaxId();
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllCountFight, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementRaidWinIndexId = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
