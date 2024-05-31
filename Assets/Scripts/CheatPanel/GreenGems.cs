using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class GreenGems: CheatPanelComponent
    {
        private IPersistenProgressService _progressService;
        public override void Start()
        {
            //PlayerData.Gems.Green.OnValueChanged +=
            //() => Text.text = PlayerData.Gems.Green.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.Gems.Green.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
