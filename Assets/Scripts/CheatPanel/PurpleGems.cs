using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class PurpleGems:CheatPanelComponent
    {
        private IPersistenProgressService _progressService;

        public override void Start()
        {
            //PlayerData.Gems.Purple.OnValueChanged +=
            //() => Text.text = PlayerData.Gems.Purple.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.Gems.Purple.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
