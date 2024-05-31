using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class BerserkHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public BerserkHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Berserk;
            AllHerosType.AddType(this);
        }
    }
}