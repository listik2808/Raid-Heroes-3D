using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class CharmerHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public CharmerHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Charmer;
            AllHerosType.AddType(this);
        }
    }
}