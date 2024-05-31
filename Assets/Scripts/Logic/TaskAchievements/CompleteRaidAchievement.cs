using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class CompleteRaidAchievement : AchievementsAll
    {
        public const string WinRaid = "Завершить рейд на ";
        public const string WinRaid2 = "%";
        private float _currentRaidProgress = 0;
        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0};
                RevardCrystals = new List<float> {5000 };
                Requirements = new List<float> { 200 };//{ 400 };
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AhievementGetCountRaidCountReachRaid;
            MaxID = (int)Requirements[0];
            //MaxId();
        }

        public void SetProgressAch(float currentnamberIdBattle)
        {
            ProgressService.Progress.Achievements.ProgressRaid = currentnamberIdBattle;
            _currentRaidProgress = currentnamberIdBattle;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            TextRequirements.text = WinRaid + 100 + WinRaid2;
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(_currentRaidProgress, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AhievementGetCountRaidCountReachRaid = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
