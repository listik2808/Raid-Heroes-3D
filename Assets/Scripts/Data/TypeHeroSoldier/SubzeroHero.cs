using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class SubzeroHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public SubzeroHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Subzero;
            AllHerosType.AddType(this);
        }
    }
}