using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class Portals: CheatPanelComponent
    {
        private IPersistenProgressService _progressService;

        public override void Start()
        {
            //PlayerData.Portals.OnValueChanged +=
            //() => Text.text = PlayerData.Portals.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.Portals.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
