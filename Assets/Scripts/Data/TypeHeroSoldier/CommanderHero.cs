using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class CommanderHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public CommanderHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Commander;
            AllHerosType.AddType(this);
        }
    }
}