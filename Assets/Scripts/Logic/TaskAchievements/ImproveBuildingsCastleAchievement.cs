using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class ImproveBuildingsCastleAchievement : AchievementsAll
    {
        public const string WinRaid = "Улучшить здания ";
        public const string WinRaid2 = " раз";
        private float AllBuildBuildingsImpruvement = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 77,85,86,87,88,89,90,91,92,93};
                RevardCrystals = new List<float> { 1,2,3,5,10,20,30,50,100,500};
                Requirements = new List<float> { 1,3,5,10,15,30,50,100,500,1000};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCountImpruvementBuilding;
            AllBuildBuildingsImpruvement = ProgressService.Progress.Achievements.AllValueImpruvementBuilding;
            MaxId();
        }

        public void GetCountImpruvementBuilding()
        {
            AllBuildBuildingsImpruvement = ProgressService.Progress.Achievements.AllValueImpruvementBuilding;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllBuildBuildingsImpruvement, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetCountImpruvementBuilding = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
