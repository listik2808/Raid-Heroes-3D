using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class Medals : CheatPanelComponent
    {
        private IPersistenProgressService _progressService;
        public override void Start()
        {
            //PlayerData.Medals.OnValueChanged +=
            //() => Text.text = PlayerData.Medals.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.PlayerData.Medals.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
