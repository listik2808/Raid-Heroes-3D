using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class UpRankAchievement : AchievementsAll
    {
        public const string WinRaid = "Повысить до ранга ";
        public const string WinRaid2 = " любого героев";
        private float AllValueRank = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 9, 62, 63, 64, 65};
                RevardCrystals = new List<float> { 10, 100, 200, 300, 500};
                Requirements = new List<float> { 1,2,3,4,5};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetUpRank;
            AllValueRank = ProgressService.Progress.Achievements.AllValueRankAch;
            MaxId();
        }

        public void SetUpRank()
        {
            AllValueRank = ProgressService.Progress.Achievements.AllValueRankAch;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueRank, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetUpRank = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
