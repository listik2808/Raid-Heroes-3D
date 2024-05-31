using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class ClearTerritoriesAch : AchievementsAll
    {
        public const string WinRaid = "Очистить ";
        public const string WinRaid2 = " территорий";
        private float AllValueClearTerritories = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 1, 67, 68, 69, 70, 71, 72,73,74,75 };
                RevardCrystals = new List<float> { 1, 3, 5, 10, 20, 30, 50,100,200,500 };
                Requirements = new List<float> { 2,4,8,16,32,64,128,256,512,1024};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCountClierTerritories;
            AllValueClearTerritories = ProgressService.Progress.Achievements.AllValueClearTerritoriesAch;
            MaxId();
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueClearTerritories, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ScreenTask.SortCard();
            ProgressService.Progress.Achievements.AchievementGetCountClierTerritories = CurrentIndexId;
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
