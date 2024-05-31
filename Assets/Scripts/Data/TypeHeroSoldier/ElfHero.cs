using Scripts.StaticData;
using System;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class ElfHero : DataLevelSkill
    {
        public AllHerosType AllHerosType;

        public ElfHero(AllHerosType allHerosType)
        {
            AllHerosType = allHerosType;
            TypeId = HeroTypeId.Elf;
            AllHerosType.AddType(this);
        }
    }
}