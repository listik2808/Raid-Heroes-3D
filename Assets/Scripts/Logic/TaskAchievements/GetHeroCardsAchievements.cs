using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Army.AllCadsHeroes;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class GetHeroCardsAchievements : AchievementsAll
    {
        public const string WinRaid = "Получить ";
        public const string WinRaid2 = " карт героев";

        private int _countAllCard;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0, 8, 9, 10, 11, 12, 13,14,15};
                RevardCrystals = new List<float> { 1,5, 10, 30, 100, 200, 500, 500,1000 };
                Requirements = new List<float> { 6, 10, 20, 50, 100, 200, 400,800,1400};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCardHeroIndexId;
            _countAllCard = ProgressService.Progress.Achievements.AchievementAllCountCard;
            MaxId();
        }

        public void SetCountCard(IPersistenProgressService progressService)
        {
            _countAllCard = progressService.Progress.Achievements.AchievementAllCountCard;
            if (SceneManager.GetActiveScene().name == AssetPath.SceneMain)
                ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(_countAllCard, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetCardHeroIndexId = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
