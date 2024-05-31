using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class MakeEvolutionHeroesAchievement : AchievementsAll
    {
        public const string WinRaid = "Сделать ";
        public const string WinRaid2 = " любых эволюцию героев";
        private float AllValueUpEvolutionHeroSkill = 0;

        public override void Construct(MainScreen mainScreen, ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> {48,55, 56, 57, 58, 59,60 };
                RevardCrystals = new List<float> { 1, 2, 3, 5, 10, 100, 500 };
                Requirements = new List<float> { 1, 3, 5, 10, 50, 500, 5000};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetUpEvolutionHero;
            AllValueUpEvolutionHeroSkill = ProgressService.Progress.Achievements.AllUpEvolutionSkill;
            MaxId();
        }

        public void SetUpEvolutionSkill()
        {
            AllValueUpEvolutionHeroSkill = ProgressService.Progress.Achievements.AllUpEvolutionSkill;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(AllValueUpEvolutionHeroSkill, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetUpEvolutionHero = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
