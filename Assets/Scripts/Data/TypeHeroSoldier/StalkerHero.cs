using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class StalkerHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public StalkerHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Stalker;
            AllHerosType.AddType(this);
        }
    }
}