using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class WatchCommercialsAchievement : AchievementsAll
    {
        public const string WinRaid = "Посмотреть ";
        public const string WinRaid2 = " рекламных роликов";
        private float _currentShowCommercials = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0 ,114,115,116,117,118,119,120,121,122};
                RevardCrystals = new List<float> { 1,2,3,5,10,20,30,50,100,500};
                Requirements = new List<float> { 1,3,5,10,15,30,50,100,500,1000};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCountWatchCommercials;
            _currentShowCommercials = ProgressService.Progress.Achievements.AllCountShowCommercials;
            MaxId();
        }

        public void SetCountShowCommercial()
        {
            _currentShowCommercials++;
            ProgressService.Progress.Achievements.AllCountShowCommercials = _currentShowCommercials;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(_currentShowCommercials, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetCountWatchCommercials = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
