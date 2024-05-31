using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class LeaderBanner : CheatPanelComponent
    {
        private IPersistenProgressService _progressService;
        public override void Start()
        {
            //PlayerData.Banners.LeaderBanner.OnValueChanged +=
            //() => Text.text = PlayerData.Banners.LeaderBanner.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.PlayerData.Banners.LeaderBanner.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
