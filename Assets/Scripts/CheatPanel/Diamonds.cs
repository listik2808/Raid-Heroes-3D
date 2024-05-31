using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using UnityEngine;

namespace Assets.Scripts.CheatPanel
{
    public class Diamonds: CheatPanelComponent
    {
        private IPersistenProgressService _persistenProgressService;
        public override void Start()
        {
            //PlayerData.Wallet.Diamonds.OnValueChanged +=
            //() => Text.text = PlayerData.Wallet.Diamonds.Count.ToString();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        public override void Change()
        {
           // PlayerData.Wallet.Diamonds.Add(IsCorrect(Input, out int value) ? value : 0);
            _persistenProgressService.Progress.Wallet.Diamonds.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
