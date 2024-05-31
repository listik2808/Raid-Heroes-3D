using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class WitcherBlueHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public WitcherBlueHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.WitcherBlue;
            AllHerosType.AddType(this);
        }
    }
}