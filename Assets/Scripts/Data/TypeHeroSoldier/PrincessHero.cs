using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class PrincessHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public PrincessHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Princess;
            AllHerosType.AddType(this);
        }
    }
}