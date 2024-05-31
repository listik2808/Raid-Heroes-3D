using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class WarriorHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public WarriorHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Warrior;
            AllHerosType.AddType(this);
        }
    }
}