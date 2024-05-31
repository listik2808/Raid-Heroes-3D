using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Economics.Banners
{
    [System.Serializable]
    public class Banners
    {
        public LeaderBanner LeaderBanner;
        public CommanderBanner CommanderBanner;
        public KingBanner KingBanner;

        public Banners()
        {
            LeaderBanner = new LeaderBanner();
            CommanderBanner = new CommanderBanner();
            KingBanner = new KingBanner();
        }
    }
}
