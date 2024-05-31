using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class BuildBuildingsCastleAchievement : AchievementsAll
    {
        public const string WinRaid = "Построить ";
        public const string WinRaid2 = " здания в замке";
        private float AllBuildBuildings = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 77,78,79,80,81,82,83,84};
                RevardCrystals = new List<float> { 1,5,10,20,30,40,50,100,200};
                Requirements = new List<float> { 1,2,3,4,5,6,7,8,9};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AhievementGetCountBuilding;
            AllBuildBuildings = ProgressService.Progress.Achievements.AllValueBuildBuildingAch;
            MaxId();
        }

        public void SetCountOpenCardBuilding()
        {
            AllBuildBuildings = ProgressService.Progress.Achievements.AllValueBuildBuildingAch;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllBuildBuildings, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AhievementGetCountBuilding = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
