using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class GetDiamondsAchievement : AchievementsAll
    {
        public const string WinRaid = "Получить ";
        public const string WinRaid2 = " кристаллов";
        private float AllValueDiamonds = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 32, 33, 34, 35, 36, 37};
                RevardCrystals = new List<float> { 1, 5, 10, 50, 100, 200,500};
                Requirements = new List<float> { 10, 100, 1000, 10000, 100000,1000000, 10000000};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetDiamondsIndexId;
            AllValueDiamonds = ProgressService.Progress.Achievements.AllDiamonds;
            MaxId();
        }

        public void SetDiamonds(float value)
        {
            AllValueDiamonds += value;
            ProgressService.Progress.Achievements.AllDiamonds = AllValueDiamonds;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueDiamonds, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetDiamondsIndexId = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
