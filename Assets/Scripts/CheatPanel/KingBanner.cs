using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class KingBanner : CheatPanelComponent
    {
        private IPersistenProgressService _progressService;

        public override void Start()
        {
            //PlayerData.Banners.KingBanner.OnValueChanged +=
            //() => Text.text = PlayerData.Banners.KingBanner.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.PlayerData.Banners.KingBanner.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
