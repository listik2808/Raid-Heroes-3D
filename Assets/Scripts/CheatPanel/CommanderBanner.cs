using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.CheatPanel
{
    public class CommanderBanner : CheatPanelComponent
    {
        private IPersistenProgressService _progressService;
        public override void Start()
        {
            //PlayerData.Banners.CommanderBanner.OnValueChanged +=
            //() => Text.text = PlayerData.Banners.CommanderBanner.Count.ToString();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            _progressService.Progress.PlayerData.Banners.CommanderBanner.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
