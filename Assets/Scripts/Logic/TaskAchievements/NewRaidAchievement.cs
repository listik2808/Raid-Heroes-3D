using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class NewRaidAchievement : AchievementsAll
    {
        public const string WinRaid = "Начать новый рейд ";
        public const string WinRaid2 = " раз";
        private float AllRaidCountNew = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> {95, 103,104, 105,106,107};
                RevardCrystals = new List<float> { 1, 3,5,10,20,30};
                Requirements = new List<float> { 1,3,5,10,50,100};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetNewNamberRaid;
            AllRaidCountNew = ProgressService.Progress.Achievements.AllNewCountRaid;
            MaxId();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllRaidCountNew, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetNewNamberRaid = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
