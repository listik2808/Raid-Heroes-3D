using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class KonungHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public KonungHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Konung;
            AllHerosType.AddType(this);
        }
    }
}