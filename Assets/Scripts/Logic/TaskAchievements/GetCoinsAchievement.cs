using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class GetCoinsAchievement : AchievementsAll
    {
        public const string WinRaid = "Получить ";
        public const string WinRaid2 = " монет";
        private float AllValueCoins = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 21, 22, 23, 24, 25, 26,27,28,29,30 };
                RevardCrystals = new List<float> { 1, 5, 10, 50, 100, 150, 200,300,400,500,1000 };
                Requirements = new List<float> { 1000, 10000, 100000, 1000000, 10000000,
                    100000000, 1000000000,1000000000000, 1000000000000000, 1000000000000000000,1000000000000000000000f};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCoinsIndexId;
            AllValueCoins = ProgressService.Progress.Achievements.AllCoinsAch;
            MaxId();
        }

        public void SetCoins(float value)
        {
            AllValueCoins += value;
            ProgressService.Progress.Achievements.AllCoinsAch = AllValueCoins;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueCoins, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetCoinsIndexId = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
