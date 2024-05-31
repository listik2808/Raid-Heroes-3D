using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Assets.Scripts.CheatPanel
{
    public class ArenaPasses : CheatPanelComponent
    {
        private IPersistenProgressService _persistenProgressService;
        public override void Start()
        {
            //PlayerData.ArenaPasses.OnValueChanged +=
            //() => Text.text = PlayerData.ArenaPasses.Count.ToString();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _persistenProgressService.Progress.ArenaPasses.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
