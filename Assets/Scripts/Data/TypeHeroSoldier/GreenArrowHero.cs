using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class GreenArrowHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public GreenArrowHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.GreenArrow;
            AllHerosType.AddType(this);
        }
    }
}