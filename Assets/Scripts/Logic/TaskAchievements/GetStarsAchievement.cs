using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class GetStarsAchievement : AchievementsAll
    {
        public const string WinRaid = "Получить ";
        public const string WinRaid2 = " звезд";
        private float AllValueStars = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 39,40,41,42,43,44,45,46 };
                RevardCrystals = new List<float> { 5,10,50,100,200,300,400,500,1000};
                Requirements = new List<float> { 1,5,25,100,1000,10000,100000, 1000000 , 10000000 };
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetStarsIndexId;
            AllValueStars = ProgressService.Progress.Achievements.AllStars;
            MaxId();
        }

        public void SetStars(float value)
        {
            AllValueStars += value;
            ProgressService.Progress.Achievements.AllDiamonds = AllValueStars;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueStars, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetStarsIndexId = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
