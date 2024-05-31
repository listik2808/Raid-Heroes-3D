using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class AssassinHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public AssassinHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Assassin;
            AllHerosType.AddType(this);
        }
    }
}