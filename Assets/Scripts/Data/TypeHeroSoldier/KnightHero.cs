using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class KnightHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public KnightHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Knight;
            AllHerosType.AddType(this);
        }
    }
}