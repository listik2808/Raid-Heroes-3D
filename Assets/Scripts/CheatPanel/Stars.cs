using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CheatPanel
{
    public class Stars : CheatPanelComponent
    {
        private IPersistenProgressService _persistenProgressService;
        public override void Start()
        {
            //PlayerData.Wallet.Stars.OnValueChanged +=
            //() => Text.text = PlayerData.Wallet.Stars.Count.ToString();
            _persistenProgressService = AllServices.Container.Single<IPersistenProgressService>();
        }
        public override void Change()
        {
            //PlayerData.Wallet.Stars.Add(IsCorrect(Input, out int value) ? value : 0);
            _persistenProgressService.Progress.Wallet.Stars.Add(IsCorrect(Input, out int value) ? value : 0);
        }
    }
}
