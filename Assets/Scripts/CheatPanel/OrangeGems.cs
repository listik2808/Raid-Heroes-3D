using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class OrangeGems: CheatPanelComponent
    {
        private IPersistenProgressService _progressService;

        public override void Start()
        {
            //PlayerData.Gems.Orange.OnValueChanged +=
            //() => Text.text = PlayerData.Gems.Orange.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.Gems.Orange.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
