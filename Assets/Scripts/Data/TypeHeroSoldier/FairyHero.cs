using Scripts.StaticData;
using System;
namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class FairyHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public FairyHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Fairy;
            AllHerosType.AddType(this);
        }
    }
}