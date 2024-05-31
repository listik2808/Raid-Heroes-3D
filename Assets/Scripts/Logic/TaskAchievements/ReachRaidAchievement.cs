using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class ReachRaidAchievement : AchievementsAll
    {
        public const string WinRaid = "Дойти до #";
        public const string WinRaid2 = "  в рейде";
        [SerializeField] private FightNumber _fightNumber;
        private float AllRaidCountReachRaid = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 95,96,97,98,99,148,149 };
                RevardCrystals = new List<float> { 1, 5, 10, 50, 100, 500,1000,5000 };
                Requirements = new List<float> { 5,15,30,60,120,200,300,400};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AhievementGetCountRaidCountReachRaid;
            AllRaidCountReachRaid = ProgressService.Progress.Achievements.AllRaidCountReachRaidAch;
            MaxId();
        }

        public void SetNamberRaid()
        {
            AllRaidCountReachRaid = _fightNumber.CurrentNumber;
            ProgressService.Progress.Achievements.AllRaidCountReachRaidAch = AllRaidCountReachRaid;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllRaidCountReachRaid, CurrentRequirement);
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
