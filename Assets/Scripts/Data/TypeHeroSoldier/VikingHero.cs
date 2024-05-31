using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class VikingHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public VikingHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Viking;
            AllHerosType.AddType(this);
        }
    }
}