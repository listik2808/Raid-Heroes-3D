using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class ArcherHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public ArcherHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Archer;
            AllHerosType.AddType(this);
        }
    }
}