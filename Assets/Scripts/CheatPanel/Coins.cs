using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using UnityEngine;

namespace Assets.Scripts.CheatPanel
{
    public class Coins : CheatPanelComponent
    {
        private IPersistenProgressService _persistenProgressService;
        public override void Start()
        {
            //PlayerData.Wallet.Coins.OnValueChanged +=
            //() => Text.text = PlayerData.Wallet.Coins.Count.ToString();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        public override void Change()
        {
            //PlayerData.Wallet.Coins.Add(IsCorrect(Input, out int value) ? value : 0);
            _persistenProgressService.Progress.Wallet.Coins.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
