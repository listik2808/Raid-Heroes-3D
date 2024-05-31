using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class BarbarianHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public BarbarianHero(AllHerosType allHerosType) 
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Barbarian;
            AllHerosType.AddType(this);
        }
    }
}
