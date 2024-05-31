using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class WitcherRedHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public WitcherRedHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.WitcherRed;
            AllHerosType.AddType(this);
        }
    }
}