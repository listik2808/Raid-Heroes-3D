using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class KingHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public KingHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.King;
            AllHerosType.AddType(this);
        }
    }
}