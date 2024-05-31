using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class WitcherGreenHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public WitcherGreenHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.WitcherGreen;
            AllHerosType.AddType(this);
        }
    }
}