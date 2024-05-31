using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class PoisonerHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public PoisonerHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Poisoner;
            AllHerosType.AddType(this);
        }
    }
}