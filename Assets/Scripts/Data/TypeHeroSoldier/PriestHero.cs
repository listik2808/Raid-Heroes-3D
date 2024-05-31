using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class PriestHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public PriestHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Priest;
            AllHerosType.AddType(this);
        }
    }
}