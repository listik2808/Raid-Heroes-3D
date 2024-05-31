using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System.Collections.Generic;
using Scripts.Infrastructure.UIWindows.Screens;

namespace Scripts.Logic.TaskAchievements
{
    public class BuyGoodsStoreAchievement : AchievementsAll
    {
        public const string WinRaid = "Купить ";
        public const string WinRaid2 = " товара в Магазине";
        private float _numberPurchases = 0;

        public override void Construct(MainScreen mainScreen,ScreenTask screenTask)
        {
            ScreenTask = screenTask;
            MainScreen = mainScreen;
            if (ProgressService == null)
            {
                ProgressService = AllServices.Container.Single<IPersistenProgressService>();
                Id = new List<int> { 0,124,125,126,127,128,129 };
                RevardCrystals = new List<float> { 5,10,20,50,100,500,1000 };
                Requirements = new List<float> { 1,3,5,10,20,50,100};
            }
            CurrentIndexId = ProgressService.Progress.Achievements.AchievementGetCountNumberPurchases;
            MaxId();
        }

        public void RecordPurchase()
        {
            _numberPurchases++;
            ProgressService.Progress.Achievements.NumberPurchasesAchievement = _numberPurchases;
            ActivatedCard();
        }

        protected override void FillCard()
        {
            SetTextRequirements(CurrentIndexId, WinRaid, WinRaid2);
            Preparation(CurrentIndexId);
            OpenBatton = FillSlider(_numberPurchases, CurrentRequirement);
            ActivateSliderOrButton();
        }

        protected override void GetRewarded()
        {
            ProgressService.Progress.Wallet.Diamonds.Add(CurrentReward);
            SetNewId();
            ProgressService.Progress.Achievements.AchievementGetCountNumberPurchases = CurrentIndexId;
            ScreenTask.SortCard();
            MainScreen.SaveLoadService.SaveProgress();
        }
    }
}
